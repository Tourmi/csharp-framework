using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework.Disposables;

/// <summary>
/// Disposable object that invokes the given <paramref name="callback"/> every time it is disposed.
/// </summary>
[SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "Does not contain unmanaged resources.")]
[SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Not a real dispose implementation.")]
public class CallbackDisposable(Action callback) : IDisposable
{
    private readonly Action _callback = callback.ThrowIfNull();

    /// <summary>
    /// Invokes the callback.
    /// </summary>
    public void Dispose() => _callback.Invoke();
}
