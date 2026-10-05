using System.Collections.ObjectModel;
using Tourmi.EntityComponentSystem.Queries.Parameters.Overrides;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <inheritdoc cref="ParamCallbacks{T}"/>
internal static class ParamCallbacks
{
    private static readonly Type[] _overrideTypes = [
        typeof(IdentifierOverride),
        typeof(EntityActionsOverride),
        typeof(EntityOverride),
        typeof(ComponentEntityOverride),
        typeof(PrefabEntityOverride),
        typeof(SystemEntityOverride),
    ];
    private static readonly IReadOnlyDictionary<Type, Type> _overrideMappings = GetOverrideMappings();

    /// <summary>
    /// Returns the callbacks for parameter <typeparamref name="T"/>
    /// </summary>
    public static ParamCallbacks<T> For<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T>()
        where T : allows ref struct
        => ParamCallbacks<T>.Instance;

    /// <summary>
    /// Returns the query param overrides for type <typeparamref name="T"/>
    /// </summary>
    internal static Type? GetOverridingTypeOrDefaultFor<T>()
        where T : allows ref struct
        => _overrideMappings.GetValueOrDefault(typeof(T));

    private static ReadOnlyDictionary<Type, Type> GetOverrideMappings()
    {
        var mappings = new Dictionary<Type, Type>();

        foreach (var type in _overrideTypes)
        {
#pragma warning disable IL2075 // 'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
            var overrideInterfaceTypes = type
                .GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryParamOverride<>));
#pragma warning restore IL2075 // 'this' argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
            foreach (var overrideType in overrideInterfaceTypes)
            {
                mappings[overrideType.GenericTypeArguments[0]] = type;
            }
        }

        return mappings.AsReadOnly();
    }
}
