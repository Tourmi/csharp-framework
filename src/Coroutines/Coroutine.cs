using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Tourmi.Coroutines.CompilerServices;
using Tourmi.Coroutines.Sources;

namespace Tourmi.Coroutines;

/// <summary>
/// Represents a task that will continue its execution on the same thread later on.
/// </summary>
[AsyncMethodBuilder(typeof(AsyncCoroutineMethodBuilder))]
public readonly partial struct Coroutine() : IEquatable<Coroutine>
{
    private readonly Exception? _exception;
    private readonly CancellationToken _cancellationToken;
    private readonly ICoroutineSource<CoroutineUnit>? _coroutineSource;

    /// <inheritdoc cref="Coroutine{TResult}"/>
    public Coroutine(Exception exception)
        : this()
    {
        _exception = exception;
    }

    /// <inheritdoc cref="Coroutine{TResult}"/>
    public Coroutine(CancellationToken cancellationToken)
        : this()
    {
        _cancellationToken = cancellationToken;
    }

    /// <inheritdoc cref="Coroutine{TResult}"/>
    public Coroutine(ICoroutineSource<CoroutineUnit>? completionSource)
        : this()
    {
        _coroutineSource = completionSource;
    }

    internal Coroutine(ICoroutineSource<CoroutineUnit>? completionSource, Exception? exception, CancellationToken cancellationToken)
        : this(cancellationToken)
    {
        _coroutineSource = completionSource;
        _exception = exception;
    }

    /// <summary>
    /// Represents an already completed coroutine.
    /// </summary>
    public static Coroutine CompletedCoroutine => default;

    /// <summary>
    /// Current state of the coroutine
    /// </summary>
    public CoroutineStatus Status
    {
        get
        {
            if (_coroutineSource is not null)
            {
                return _coroutineSource.GetStatus();
            }

            if (_exception is not null)
            {
                return CoroutineStatus.Failed;
            }

            if (_cancellationToken.IsCancellationRequested)
            {
                return CoroutineStatus.Canceled;
            }

            return CoroutineStatus.Succeeded;
        }
    }

    internal ICoroutineSource<CoroutineUnit>? CoroutineSource => _coroutineSource;

    internal Exception? Exception => _exception;

    internal CancellationToken CancellationToken => _cancellationToken;

    /// <inheritdoc cref="CoroutineExtensions.ToCoroutine(Coroutine{CoroutineUnit})"/>
    [SuppressMessage("Usage", "CA2225:Operator overloads have named alternates", Justification = "The method exists in CoroutineExtensions.")]
    public static implicit operator Coroutine(Coroutine<CoroutineUnit> coroutine) => coroutine.ToCoroutine();

    /// <inheritdoc/>
    public static bool operator ==(Coroutine left, Coroutine right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Coroutine left, Coroutine right) => !(left == right);

    /// <summary>
    /// Returns a new Coroutine that is already complete with the given <paramref name="result"/>.
    /// </summary>
    public static Coroutine<TResult> FromResult<TResult>(TResult result) => new(result);

    /// <summary>
    /// Creates a new Coroutine that failed execution due to the given exception.
    /// </summary>
    public static Coroutine FromException(Exception e) => new(e);

    /// <summary>
    /// Creates a new Coroutine from the given <paramref name="cancellationToken"/>.
    /// </summary>
    public static Coroutine FromCanceled(CancellationToken cancellationToken = default) => new(cancellationToken);

    /// <summary>
    /// Returns the default awaiter for a coroutine.
    /// </summary>
    public CoroutineAwaiter GetAwaiter() => new(this);

    /// <inheritdoc/>
    public bool Equals(Coroutine other) => _coroutineSource == other._coroutineSource && _exception == other._exception;

    /// <inheritdoc/>
    public override bool Equals(object? obj) => obj is Coroutine coroutine && Equals(coroutine);

    /// <inheritdoc/>
    public override int GetHashCode() => _coroutineSource?.GetHashCode() ?? _exception?.GetHashCode() ?? 0;

    internal void GetResult()
    {
        if (_coroutineSource is not null)
        {
            _ = _coroutineSource.GetResult();
            return;
        }

        if (_exception is not null)
        {
            throw _exception;
        }

        _cancellationToken.ThrowIfCancellationRequested();
    }

    internal void OnCompleted(Action continuation)
    {
        if (_coroutineSource is not null)
        {
            _coroutineSource.OnCompleted(continuation);
            return;
        }

        continuation();
    }
}
