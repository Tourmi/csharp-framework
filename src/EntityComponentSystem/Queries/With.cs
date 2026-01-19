namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query Parameter that marks the component <typeparamref name="T"/> as required, 
/// without including its value.
/// </summary>
public readonly ref struct With<T> : IQueryParam<With<T>>
{
    static With<T> IQueryParam<With<T>>.CreateFrom(QueryParamEntityInfo entry) => default;

    static void IQueryParam<With<T>>.UpdateFilter(EntityFilter filter) => filter.Requires<T>();
}
