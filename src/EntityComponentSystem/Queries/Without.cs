namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Query Parameter that marks the component <typeparamref name="T"/> as excluded, 
/// filtering out entities that have the component.
/// </summary>
public readonly ref struct Without<T> : IQueryParam<Without<T>>
{
    static Without<T> IQueryParam<Without<T>>.CreateFrom(QueryParamEntityInfo entry) => default;

    static void IQueryParam<Without<T>>.UpdateFilter(EntityFilter filter) => filter.Excludes<T>();
}
