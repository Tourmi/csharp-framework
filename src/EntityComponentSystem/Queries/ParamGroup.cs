namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Wrapper type for multiple query parameters.
/// Can be used to group parameters of a query together for better legibility, or when a <see cref="Query"/> has too many parameters.
/// </summary>
public readonly ref struct ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8) : IParamGroup
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct
    where T8 : allows ref struct
{
    /// <summary>
    /// First value of the parameter group.
    /// </summary>
    public T1 Value1 { get; } = param1;

    /// <summary>
    /// Second value of the parameter group.
    /// </summary>
    public T2 Value2 { get; } = param2;

    /// <summary>
    /// Third value of the parameter group.
    /// </summary>
    public T3 Value3 { get; } = param3;

    /// <summary>
    /// Fourth value of the parameter group.
    /// </summary>
    public T4 Value4 { get; } = param4;

    /// <summary>
    /// Fifth value of the parameter group.
    /// </summary>
    public T5 Value5 { get; } = param5;

    /// <summary>
    /// Sixth value of the parameter group.
    /// </summary>
    public T6 Value6 { get; } = param6;

    /// <summary>
    /// Seventh value of the parameter group.
    /// </summary>
    public T7 Value7 { get; } = param7;

    /// <summary>
    /// Eighth value of the parameter group.
    /// </summary>
    public T8 Value8 { get; } = param8;

    /// <inheritdoc/>
    public int Count => 8;
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<T1, T2, T3, T4, T5, T6, T7>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7) : IParamGroup
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
    where T7 : allows ref struct
{
    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value4"/>
    public T4 Value4 { get; } = param4;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value5"/>
    public T5 Value5 { get; } = param5;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value6"/>
    public T6 Value6 { get; } = param6;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value7"/>
    public T7 Value7 { get; } = param7;

    /// <inheritdoc/>
    public int Count => 7;
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<T1, T2, T3, T4, T5, T6>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6) : IParamGroup
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
    where T6 : allows ref struct
{
    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value4"/>
    public T4 Value4 { get; } = param4;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value5"/>
    public T5 Value5 { get; } = param5;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value6"/>
    public T6 Value6 { get; } = param6;

    /// <inheritdoc/>
    public int Count => 6;
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<T1, T2, T3, T4, T5>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5) : IParamGroup
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
    where T5 : allows ref struct
{
    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value4"/>
    public T4 Value4 { get; } = param4;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value5"/>
    public T5 Value5 { get; } = param5;

    /// <inheritdoc/>
    public int Count => 5;
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4) : IParamGroup
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
    where T4 : allows ref struct
{
    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value4"/>
    public T4 Value4 { get; } = param4;

    /// <inheritdoc/>
    public int Count => 4;
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<T1, T2, T3>(T1 param1, T2 param2, T3 param3) : IParamGroup
    where T1 : allows ref struct
    where T2 : allows ref struct
    where T3 : allows ref struct
{
    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value3"/>
    public T3 Value3 { get; } = param3;


    /// <inheritdoc/>
    public int Count => 3;
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<T1, T2>(T1 param1, T2 param2) : IParamGroup
    where T1 : allows ref struct
    where T2 : allows ref struct
{
    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value2"/>
    public T2 Value2 { get; } = param2;

    /// <inheritdoc/>
    public int Count => 2;
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup<T1>(T1 param1) : IParamGroup
    where T1 : allows ref struct
{
    /// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}.Value1"/>
    public T1 Value1 { get; } = param1;

    /// <inheritdoc/>
    public int Count => 1;
}

/// <inheritdoc cref="ParamGroup{T1, T2, T3, T4, T5, T6, T7, T8}"/>
public readonly ref struct ParamGroup() : IParamGroup
{
    /// <inheritdoc/>
    public int Count => 0;
}
