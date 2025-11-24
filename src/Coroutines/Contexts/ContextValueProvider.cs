namespace Tourmi.Coroutines.Contexts;

internal class ContextValueProvider<TValue>(TValue value)
    : IContextValueProvider<TValue>
{
    public TValue Value { get; set; } = value;
}
