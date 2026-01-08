using System.Diagnostics.CodeAnalysis;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query Parameter that marks the component <typeparamref name="T"/> as required, 
/// without including its value.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Needed for terse queries.")]
public readonly ref struct With<T> : IQueryParam<With<T>>
{
    static With<T> IQueryParam<With<T>>.CreateFrom(QueryParamEntityInfo entry) => default;

    static void IQueryParam<With<T>>.UpdateFilter(EntityFilter filter) => filter.Requires<T>();
}
