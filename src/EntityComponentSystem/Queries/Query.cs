using System.Reflection;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Queries the World for entities filtered by the type parameters of the <see cref="Query"/>.
/// </summary>
public class Query
{
    private readonly record struct QueryParamInfo(Type ParamType);

    internal static Type GetQueryTypeFromDelegate(Delegate del)
    {
        // TODO: use some form of cache to avoid GCing the list on each call
        var queryParams = new List<QueryParamInfo>();
        var parameters = del.Method.GetParameters();
        foreach (var param in parameters)
        {
            ParseTypeInto(queryParams, ParamToType(param));
        }
        if (del.Method.ReturnType != typeof(void))
        {
            if (del.Method.ReturnType.IsByRef || del.Method.ReturnType.IsByRefLike)
            {
                throw new NotSupportedException("Cannot create a query from a delegate that returns a ref, or refstruct value.");
            }

            ParseTypeInto(queryParams, typeof(OutRef<>).MakeGenericType(del.Method.ReturnType));
        }
        if (queryParams.Count == 0)
        {
            return typeof(Query);
        }

        if (queryParams.Count == 1)
        {
            return typeof(Query<>).MakeGenericType(queryParams[0].ParamType);
        }

        var recursiveDepth = (queryParams.Count - 2) / 7 + 1;
        var lastDepthCount = queryParams.Count - (recursiveDepth - 1) * 7;

        var previousQueryParamType = lastDepthCount switch
        {
            2 => typeof(QueryParams<,>).MakeGenericType(queryParams[^2].ParamType, queryParams[^1].ParamType),
            3 => typeof(QueryParams<,,>).MakeGenericType(queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            4 => typeof(QueryParams<,,,>).MakeGenericType(queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            5 => typeof(QueryParams<,,,,>).MakeGenericType(queryParams[^5].ParamType, queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            6 => typeof(QueryParams<,,,,,>).MakeGenericType(queryParams[^6].ParamType, queryParams[^5].ParamType, queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            7 => typeof(QueryParams<,,,,,,>).MakeGenericType(queryParams[^7].ParamType, queryParams[^6].ParamType, queryParams[^5].ParamType, queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            8 => typeof(QueryParams<,,,,,,,>).MakeGenericType(queryParams[^8].ParamType, queryParams[^7].ParamType, queryParams[^6].ParamType, queryParams[^5].ParamType, queryParams[^4].ParamType, queryParams[^3].ParamType, queryParams[^2].ParamType, queryParams[^1].ParamType),
            _ => null!,
        };

        recursiveDepth--;
        while (recursiveDepth > 0)
        {
            var offset = recursiveDepth * 7;
            previousQueryParamType = typeof(QueryParams<,,,,,,,>).MakeGenericType(
                queryParams[offset + 0].ParamType,
                queryParams[offset + 1].ParamType,
                queryParams[offset + 2].ParamType,
                queryParams[offset + 3].ParamType,
                queryParams[offset + 4].ParamType,
                queryParams[offset + 5].ParamType,
                queryParams[offset + 6].ParamType,
                previousQueryParamType);

            recursiveDepth--;
        }

        return typeof(Query<>).MakeGenericType(previousQueryParamType);
    }

    internal void AddSystem(Delegate system)
    {

    }

    private static Type ParamToType(ParameterInfo param)
    {
        var elementType = param.ParameterType;
        if (elementType.IsByRef)
        {
            elementType = elementType.GetElementType()!;
        }

        if (!elementType.IsValueType)
        {
            if ((!param.ParameterType.IsByRef || param.IsIn) && param.GetCustomAttribute<ThreadSafeAttribute>() is not null)
            {
                return typeof(ThreadSafe<>).MakeGenericType(elementType);
            }

            return elementType;
        }

        if (elementType.IsByRefLike)
        {
            if (param.ParameterType.IsByRef)
            {
                throw new NotSupportedException("Cannot build query with a ref struct parameter passed via ref/out/in");
            }

            return elementType;
        }

        if (!param.ParameterType.IsByRef)
        {
            return elementType;
        }

        if (param.IsIn)
        {
            return typeof(RefReadonly<>).MakeGenericType(elementType);
        }

        if (param.IsOut)
        {
            return typeof(OutRef<>).MakeGenericType(elementType);
        }

        return typeof(Ref<>).MakeGenericType(elementType);
    }

    private static void ParseTypeInto(List<QueryParamInfo> queryParams, Type param)
    {

        queryParams.Add(new(param));
    }
}

/// <inheritdoc cref="Query"/>
public class Query<T1> : Query
    where T1 : allows ref struct
{
}

/// <inheritdoc cref="Query"/>
public class Query<T1, T2> : Query<QueryParams<T1, T2>>
    where T1 : allows ref struct
    where T2 : allows ref struct;

/// <inheritdoc cref="Query"/>
public class Query<T1, T2, T3> : Query<QueryParams<T1, T2, T3>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct;

/// <inheritdoc cref="Query"/>
public class Query<T1, T2, T3, T4> : Query<QueryParams<T1, T2, T3, T4>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct;

/// <inheritdoc cref="Query"/>
public class Query<T1, T2, T3, T4, T5> : Query<QueryParams<T1, T2, T3, T4, T5>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct;

/// <inheritdoc cref="Query"/>
public class Query<T1, T2, T3, T4, T5, T6> : Query<QueryParams<T1, T2, T3, T4, T5, T6>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct;

/// <inheritdoc cref="Query"/>
public class Query<T1, T2, T3, T4, T5, T6, T7> : Query<QueryParams<T1, T2, T3, T4, T5, T6, T7>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct;

/// <inheritdoc cref="Query"/>
public class Query<T1, T2, T3, T4, T5, T6, T7, T8> : Query<QueryParams<T1, T2, T3, T4, T5, T6, T7, T8>>
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct
    where T8 : allows ref struct;
