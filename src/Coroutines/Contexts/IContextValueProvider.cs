namespace Tourmi.Coroutines.Contexts;

/// <summary>
/// Provides a value for any coroutines within the current context.
/// </summary>
public interface IContextValueProvider<TValue>
{
    /// <summary>
    /// The value for the current context.
    /// </summary>
    TValue Value { get; set; }
}
