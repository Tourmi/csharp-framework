using System.Reflection;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Extensions for reflection related types.
/// </summary>
internal static class ReflectionExtensions
{
    extension(ParameterInfo paramInfo)
    {
        public Type ToType()
        {
            var elementType = paramInfo.ParameterType;
            if (elementType.IsByRef)
            {
                elementType = elementType.GetElementType()!;
            }

            if (!elementType.IsValueType)
            {
                if ((!paramInfo.ParameterType.IsByRef || paramInfo.IsIn) && paramInfo.GetCustomAttribute<ThreadSafeAttribute>() is not null)
                {
                    return typeof(ThreadSafe<>).MakeGenericType(elementType);
                }

                return elementType;
            }

            if (elementType.IsByRefLike)
            {
                if (paramInfo.ParameterType.IsByRef)
                {
                    throw new NotSupportedException("Cannot build query with a ref struct parameter passed via ref/out/in");
                }

                return elementType;
            }

            if (!paramInfo.ParameterType.IsByRef)
            {
                return elementType;
            }

            if (paramInfo.IsIn)
            {
                return typeof(RefReadonly<>).MakeGenericType(elementType);
            }

            if (paramInfo.IsOut)
            {
                return typeof(OutRef<>).MakeGenericType(elementType);
            }

            return typeof(Ref<>).MakeGenericType(elementType);
        }
    }
}