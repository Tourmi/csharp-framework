using System.Diagnostics.CodeAnalysis;
using Tourmi.Coroutines.Sources;
using Tourmi.Framework.Disposables;

namespace Tourmi.Coroutines.Contexts;

/// <summary>
/// A synchronization context for <see cref="Coroutine{TResult}"/>s.
/// A new one should be instantiated for each use-case (ie: update loop, physics loop, etc).
/// A Context should not be used on a different thread.
/// </summary>
[SuppressMessage("Design", "CA1001:Types that own disposable fields should be disposable", Justification = "The CallbackDisposable can be disposed multiple times")]
public class CoroutineContext
{
    [ThreadStatic]
    private static CoroutineContext? _current;

    private readonly CallbackDisposable _exitContextDisposable;
    private readonly Dictionary<Type, object> _valueProviders = [];

    private Queue<IQueueAble> _queuedCoroutines = [];
    private Queue<IQueueAble> _coroutinesToReQueue = [];

    private CoroutineContext? _contextToRestore;

    /// <inheritdoc cref="CoroutineContext"/>
    public CoroutineContext()
    {
        _exitContextDisposable = new(ExitInternal);
    }

    /// <summary>
    /// The currently running CoroutineContext.
    /// </summary>
    public static CoroutineContext Current => _current ?? throw new InvalidOperationException("Cannot fetch the current context, since no contexts were entered on the current thread.");

    /// <summary>
    /// Queues the given item to the context execution queue.
    /// </summary>
    public void Queue(IQueueAble queueable)
    {
        lock (_queuedCoroutines)
        {
            _queuedCoroutines.Enqueue(queueable);
        }
    }

    /// <summary>
    /// Provides a value for the context.
    /// The value is unique for the type, and will overwrite the previous one.
    /// </summary>
    public void ProvideValue<TValue>(TValue value)
    {
        var valueType = typeof(TValue);
        var valueProvider = GetValueProviderOrDefault<TValue>(valueType);
        if (valueProvider is not null)
        {
            valueProvider.Value = value;
            return;
        }

        valueProvider = new ContextValueProvider<TValue>(value);
        _valueProviders[valueType] = valueProvider;
    }

    /// <summary>
    /// Returns a value from the context.
    /// </summary>
    public TValue GetValue<TValue>()
    {
        var valueType = typeof(TValue);
        var valueProvider = GetValueProviderOrDefault<TValue>(valueType);
        if (valueProvider is null)
        {
            throw new InvalidOperationException(
                $"The value provider for the given type {valueType.FullName} did not exist. " +
                $"Either {nameof(ProvideValue)} was not called, or the wrong {nameof(CoroutineContext)} is in use.");
        }

        return valueProvider.Value;
    }

    /// <summary>
    /// Enters the current context. 
    /// Should always be used within a <see langword="using"/> clause. 
    /// Once disposed, the context is restored.
    /// </summary>
    public IDisposable Enter()
    {
        if (_current == this)
        {
            throw new InvalidOperationException("The current context has already been entered.");
        }

        OnEntering();

        _contextToRestore = _current;
        _current = this;

        OnEntered();

        return _exitContextDisposable;
    }

    /// <summary>
    /// Exits the current context.
    /// Avoid using this manually, and use <see cref="Enter"/> with a <see langword="using"/> clause instead.
    /// </summary>
    [Obsolete($"Try to use {nameof(Enter)} within a using() clause instead of calling Exit() manually.")]
    public void Exit() => ExitInternal();

    /// <summary>
    /// Called before the current context gets entered.
    /// </summary>
    protected virtual void OnEntering()
    {
    }

    /// <summary>
    /// Called after the current context has been entered.
    /// </summary>
    protected virtual void OnEntered()
    {
    }

    /// <summary>
    /// Called before the current context is exited, and before any queued coroutine is resumed.
    /// </summary>
    protected virtual void OnExiting()
    {
    }

    /// <summary>
    /// Called after the current context has exited.
    /// </summary>
    protected virtual void OnExited()
    {
    }

    private IContextValueProvider<T>? GetValueProviderOrDefault<T>(Type valueType) => _valueProviders.TryGetValue(valueType, out var valueProvider)
        ? (IContextValueProvider<T>)valueProvider
        : null;

    private void ExitInternal()
    {
        if (_current != this)
        {
            throw new InvalidOperationException("Cannot exit this context, since the current context is a different one.");
        }

        try
        {
            OnExiting();

            var hasCoroutinesLeft = false;
            do
            {
                IQueueAble? coroutine = null;
                lock (_queuedCoroutines)
                {
                    hasCoroutinesLeft = _queuedCoroutines.TryDequeue(out coroutine);
                }

                if (coroutine is not null)
                {
                    if (!coroutine.TryComplete())
                    {
                        _coroutinesToReQueue.Enqueue(coroutine);
                    }
                }

            } while (hasCoroutinesLeft);
        }
        finally
        {
            lock (_queuedCoroutines)
            {
                (_queuedCoroutines, _coroutinesToReQueue) = (_coroutinesToReQueue, _queuedCoroutines);
            }

            _current = _contextToRestore;
            _contextToRestore = null;
        }

        OnExited();
    }
}
