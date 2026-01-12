namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Actions that is executed on query parameters.
/// </summary>
public delegate void QueryParamAction<in T1>(
    T1 param1)
    where T1 : IQueryParam<T1>, allows ref struct;

/// <inheritdoc cref="QueryParamAction{T1}"/>
public delegate void QueryParamAction<in T1, in T2>(
    T1 param1,
    T2 param2)
    where T1 : IQueryParam<T1>, allows ref struct
    where T2 : IQueryParam<T2>, allows ref struct;

/// <inheritdoc cref="QueryParamAction{T1}"/>
public delegate void QueryParamAction<in T1, in T2, in T3>(
    T1 param1,
    T2 param2,
    T3 param3)
    where T1 : IQueryParam<T1>, allows ref struct
    where T2 : IQueryParam<T2>, allows ref struct
    where T3 : IQueryParam<T3>, allows ref struct;

/// <inheritdoc cref="QueryParamAction{T1}"/>
public delegate void QueryParamAction<in T1, in T2, in T3, in T4>(
    T1 param1,
    T2 param2,
    T3 param3,
    T4 param4)
    where T1 : IQueryParam<T1>, allows ref struct
    where T2 : IQueryParam<T2>, allows ref struct
    where T3 : IQueryParam<T3>, allows ref struct
    where T4 : IQueryParam<T4>, allows ref struct;

/// <inheritdoc cref="QueryParamAction{T1}"/>
public delegate void QueryParamAction<in T1, in T2, in T3, in T4, in T5>(
    T1 param1,
    T2 param2,
    T3 param3,
    T4 param4,
    T5 param5)
    where T1 : IQueryParam<T1>, allows ref struct
    where T2 : IQueryParam<T2>, allows ref struct
    where T3 : IQueryParam<T3>, allows ref struct
    where T4 : IQueryParam<T4>, allows ref struct
    where T5 : IQueryParam<T5>, allows ref struct;

/// <inheritdoc cref="QueryParamAction{T1}"/>
public delegate void QueryParamAction<in T1, in T2, in T3, in T4, in T5, in T6>(
    T1 param1,
    T2 param2,
    T3 param3,
    T4 param4,
    T5 param5,
    T6 param6)
    where T1 : IQueryParam<T1>, allows ref struct
    where T2 : IQueryParam<T2>, allows ref struct
    where T3 : IQueryParam<T3>, allows ref struct
    where T4 : IQueryParam<T4>, allows ref struct
    where T5 : IQueryParam<T5>, allows ref struct
    where T6 : IQueryParam<T6>, allows ref struct;

/// <inheritdoc cref="QueryParamAction{T1}"/>
public delegate void QueryParamAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7>(
    T1 param1,
    T2 param2,
    T3 param3,
    T4 param4,
    T5 param5,
    T6 param6,
    T7 param7)
    where T1 : IQueryParam<T1>, allows ref struct
    where T2 : IQueryParam<T2>, allows ref struct
    where T3 : IQueryParam<T3>, allows ref struct
    where T4 : IQueryParam<T4>, allows ref struct
    where T5 : IQueryParam<T5>, allows ref struct
    where T6 : IQueryParam<T6>, allows ref struct
    where T7 : IQueryParam<T7>, allows ref struct;

/// <inheritdoc cref="QueryParamAction{T1}"/>
public delegate void QueryParamAction<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8>(
    T1 param1,
    T2 param2,
    T3 param3,
    T4 param4,
    T5 param5,
    T6 param6,
    T7 param7,
    T8 param8)
    where T1 : IQueryParam<T1>, allows ref struct
    where T2 : IQueryParam<T2>, allows ref struct
    where T3 : IQueryParam<T3>, allows ref struct
    where T4 : IQueryParam<T4>, allows ref struct
    where T5 : IQueryParam<T5>, allows ref struct
    where T6 : IQueryParam<T6>, allows ref struct
    where T7 : IQueryParam<T7>, allows ref struct
    where T8 : IQueryParam<T8>, allows ref struct;
