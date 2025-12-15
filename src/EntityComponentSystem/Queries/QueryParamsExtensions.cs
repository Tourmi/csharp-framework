namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Extension methods for query parameter groups
/// </summary>
public static class ParamGroupExtensions
{
    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2>(QueryParams<T1, T2> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
    {
        /// <summary>
        /// Deconstructs the values associated with this parameter group.
        /// </summary>
        public void Deconstruct(out T1 value1, out T2 value2)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3>(QueryParams<T1, T2, T3> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4>(QueryParams<T1, T2, T3, T4> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5>(QueryParams<T1, T2, T3, T4, T5> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6>(QueryParams<T1, T2, T3, T4, T5, T6> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7>(QueryParams<T1, T2, T3, T4, T5, T6, T7> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6, out T7 value7)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8>(QueryParams<T1, T2, T3, T4, T5, T6, T7, T8> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6, out T7 value7, out T8 value8)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
            value8 = queryParams.Value8;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9>(QueryParams<T1, T2, T3, T4, T5, T6, T7, QueryParams<T8, T9>> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6, out T7 value7, out T8 value8, out T9 value9)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
            (value8, value9) = queryParams.Value8;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(QueryParams<T1, T2, T3, T4, T5, T6, T7, QueryParams<T8, T9, T10>> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6, out T7 value7, out T8 value8, out T9 value9, out T10 value10)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
            (value8, value9, value10) = queryParams.Value8;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(QueryParams<T1, T2, T3, T4, T5, T6, T7, QueryParams<T8, T9, T10, T11>> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(
            out T1 value1,
            out T2 value2,
            out T3 value3,
            out T4 value4,
            out T5 value5,
            out T6 value6,
            out T7 value7,
            out T8 value8,
            out T9 value9,
            out T10 value10,
            out T11 value11)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
            (value8, value9, value10, value11) = queryParams.Value8;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(QueryParams<T1, T2, T3, T4, T5, T6, T7, QueryParams<T8, T9, T10, T11, T12>> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(
            out T1 value1,
            out T2 value2,
            out T3 value3,
            out T4 value4,
            out T5 value5,
            out T6 value6,
            out T7 value7,
            out T8 value8,
            out T9 value9,
            out T10 value10,
            out T11 value11,
            out T12 value12)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
            (value8, value9, value10, value11, value12) = queryParams.Value8;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(QueryParams<T1, T2, T3, T4, T5, T6, T7, QueryParams<T8, T9, T10, T11, T12, T13>> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
        where T13 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(
            out T1 value1,
            out T2 value2,
            out T3 value3,
            out T4 value4,
            out T5 value5,
            out T6 value6,
            out T7 value7,
            out T8 value8,
            out T9 value9,
            out T10 value10,
            out T11 value11,
            out T12 value12,
            out T13 value13)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
            (value8, value9, value10, value11, value12, value13) = queryParams.Value8;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(QueryParams<T1, T2, T3, T4, T5, T6, T7, QueryParams<T8, T9, T10, T11, T12, T13, T14>> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
        where T13 : allows ref struct
        where T14 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(
            out T1 value1,
            out T2 value2,
            out T3 value3,
            out T4 value4,
            out T5 value5,
            out T6 value6,
            out T7 value7,
            out T8 value8,
            out T9 value9,
            out T10 value10,
            out T11 value11,
            out T12 value12,
            out T13 value13,
            out T14 value14)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
            (value8, value9, value10, value11, value12, value13, value14) = queryParams.Value8;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(QueryParams<T1, T2, T3, T4, T5, T6, T7, QueryParams<T8, T9, T10, T11, T12, T13, T14, T15>> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
        where T13 : allows ref struct
        where T14 : allows ref struct
        where T15 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(
            out T1 value1,
            out T2 value2,
            out T3 value3,
            out T4 value4,
            out T5 value5,
            out T6 value6,
            out T7 value7,
            out T8 value8,
            out T9 value9,
            out T10 value10,
            out T11 value11,
            out T12 value12,
            out T13 value13,
            out T14 value14,
            out T15 value15)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
            (value8, value9, value10, value11, value12, value13, value14, value15) = queryParams.Value8;
        }
    }

    /// <param name="queryParams">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(QueryParams<T1, T2, T3, T4, T5, T6, T7, QueryParams<T8, T9, T10, T11, T12, T13, T14, QueryParams<T15, T16>>> queryParams)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
        where T9 : allows ref struct
        where T10 : allows ref struct
        where T11 : allows ref struct
        where T12 : allows ref struct
        where T13 : allows ref struct
        where T14 : allows ref struct
        where T15 : allows ref struct
        where T16 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(QueryParams{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(
            out T1 value1,
            out T2 value2,
            out T3 value3,
            out T4 value4,
            out T5 value5,
            out T6 value6,
            out T7 value7,
            out T8 value8,
            out T9 value9,
            out T10 value10,
            out T11 value11,
            out T12 value12,
            out T13 value13,
            out T14 value14,
            out T15 value15,
            out T16 value16)
        {
            value1 = queryParams.Value1;
            value2 = queryParams.Value2;
            value3 = queryParams.Value3;
            value4 = queryParams.Value4;
            value5 = queryParams.Value5;
            value6 = queryParams.Value6;
            value7 = queryParams.Value7;
            (value8, value9, value10, value11, value12, value13, value14, value15, value16) = queryParams.Value8;
        }
    }
}
