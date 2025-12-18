namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Extension methods for query parameter groups
/// </summary>
public static class ParamGroupExtensions
{
    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2>(ParamGroup<T1, T2> paramGroup)
        where T1 : allows ref struct
        where T2 : allows ref struct
    {
        /// <summary>
        /// Deconstructs the values associated with this parameter group.
        /// </summary>
        public void Deconstruct(out T1 value1, out T2 value2)
        {
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3>(ParamGroup<T1, T2, T3> paramGroup)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3)
        {
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4>(ParamGroup<T1, T2, T3, T4> paramGroup)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4)
        {
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5>(ParamGroup<T1, T2, T3, T4, T5> paramGroup)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5)
        {
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6>(ParamGroup<T1, T2, T3, T4, T5, T6> paramGroup)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6)
        {
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7>(ParamGroup<T1, T2, T3, T4, T5, T6, T7> paramGroup)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6, out T7 value7)
        {
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8>(ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8> paramGroup)
        where T1 : allows ref struct
        where T2 : allows ref struct
        where T3 : allows ref struct
        where T4 : allows ref struct
        where T5 : allows ref struct
        where T6 : allows ref struct
        where T7 : allows ref struct
        where T8 : allows ref struct
    {
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6, out T7 value7, out T8 value8)
        {
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
            value8 = paramGroup.Value8;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9>(ParamGroup<T1, T2, T3, T4, T5, T6, T7, ParamGroup<T8, T9>> paramGroup)
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
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6, out T7 value7, out T8 value8, out T9 value9)
        {
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
            (value8, value9) = paramGroup.Value8;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(ParamGroup<T1, T2, T3, T4, T5, T6, T7, ParamGroup<T8, T9, T10>> paramGroup)
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
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
        public void Deconstruct(out T1 value1, out T2 value2, out T3 value3, out T4 value4, out T5 value5, out T6 value6, out T7 value7, out T8 value8, out T9 value9, out T10 value10)
        {
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
            (value8, value9, value10) = paramGroup.Value8;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(ParamGroup<T1, T2, T3, T4, T5, T6, T7, ParamGroup<T8, T9, T10, T11>> paramGroup)
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
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
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
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
            (value8, value9, value10, value11) = paramGroup.Value8;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(ParamGroup<T1, T2, T3, T4, T5, T6, T7, ParamGroup<T8, T9, T10, T11, T12>> paramGroup)
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
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
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
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
            (value8, value9, value10, value11, value12) = paramGroup.Value8;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(ParamGroup<T1, T2, T3, T4, T5, T6, T7, ParamGroup<T8, T9, T10, T11, T12, T13>> paramGroup)
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
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
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
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
            (value8, value9, value10, value11, value12, value13) = paramGroup.Value8;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(ParamGroup<T1, T2, T3, T4, T5, T6, T7, ParamGroup<T8, T9, T10, T11, T12, T13, T14>> paramGroup)
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
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
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
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
            (value8, value9, value10, value11, value12, value13, value14) = paramGroup.Value8;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(ParamGroup<T1, T2, T3, T4, T5, T6, T7, ParamGroup<T8, T9, T10, T11, T12, T13, T14, T15>> paramGroup)
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
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
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
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
            (value8, value9, value10, value11, value12, value13, value14, value15) = paramGroup.Value8;
        }
    }

    /// <param name="paramGroup">The target parameter group</param>
    extension<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>(ParamGroup<T1, T2, T3, T4, T5, T6, T7, ParamGroup<T8, T9, T10, T11, T12, T13, T14, ParamGroup<T15, T16>>> paramGroup)
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
        /// <inheritdoc cref="Deconstruct{T1, T2}(ParamGroup{T1, T2}, out T1, out T2)"/>
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
            value1 = paramGroup.Value1;
            value2 = paramGroup.Value2;
            value3 = paramGroup.Value3;
            value4 = paramGroup.Value4;
            value5 = paramGroup.Value5;
            value6 = paramGroup.Value6;
            value7 = paramGroup.Value7;
            (value8, value9, value10, value11, value12, value13, value14, value15, value16) = paramGroup.Value8;
        }
    }
}
