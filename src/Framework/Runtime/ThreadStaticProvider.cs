using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.Runtime;

/// <summary>
/// Provides a value of type <typeparamref name="T"/>, that is automatically initialized.
/// </summary>
public static class ThreadStaticProvider<T>
    where T : class, new()
{
    /// <summary>
    /// Return the thread static value, initializing it if not yet created.
    /// </summary>
    [field: ThreadStatic]
    [SuppressMessage("Design", "CA1000:Do not declare static members on generic types", Justification = "ThreadStatic singleton cache")]
    public static T Value => field ??= new();
}
