using Tourmi.Framework.Collections.Generic;

namespace Tourmi.Coroutines.Collections;

internal static class ThreadStaticPools<T>
    where T : class
{
    [ThreadStatic]
    private static Pool<T>? _pool;

    public static Pool<T> Get() => _pool ??= new();
}
