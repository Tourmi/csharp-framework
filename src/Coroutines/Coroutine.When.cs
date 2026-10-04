using Tourmi.Coroutines.Sources;

namespace Tourmi.Coroutines;

public readonly partial struct Coroutine
{
    /// <summary>
    /// Returns when all given coroutines complete.
    /// </summary>
    public static Coroutine WhenAll(params ReadOnlySpan<Coroutine> coroutines)
    {
        List<Exception>? exceptions = null;
        var anyCanceled = false;
        var areAllComplete = true;
        foreach (var coroutine in coroutines)
        {
            if (coroutine.Status is CoroutineStatus.Running)
            {
                areAllComplete = false;
                break;
            }
            else if (coroutine.Status is CoroutineStatus.Failed)
            {
                exceptions ??= [];
                exceptions.Add(coroutine.Exception!);
            }
            else if (coroutine.Status is CoroutineStatus.Canceled)
            {
                anyCanceled = true;
            }
        }

        if (areAllComplete)
        {
            if (exceptions is not null)
            {
                return FromException(new AggregateException("One or many coroutines failed.", exceptions));
            }

            if (anyCanceled)
            {
                return FromCanceled();
            }

            return default;
        }

        return WhenAllCoroutineSource.Create(coroutines).Coroutine;
    }

    /// <summary>
    /// Returns a coroutine that completes when any of the given <paramref name="coroutines"/> complete.
    /// The result will be the coroutine that was completed first, be it successfully or not.
    /// </summary>
    public static Coroutine<Coroutine<TResult>> WhenAny<TResult>(params ReadOnlySpan<Coroutine<TResult>> coroutines)
    {
        if (coroutines.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coroutines), "At least one coroutine was expected.");
        }

        foreach (var coroutine in coroutines)
        {
            if (coroutine.Status is not CoroutineStatus.Running)
            {
                return FromResult(coroutine);
            }
        }

        return WhenAnyCoroutineSource<TResult>.Create(coroutines).Coroutine;
    }

    /// <inheritdoc cref="WhenAny{TResult}(ReadOnlySpan{Coroutine{TResult}})"/>
    public static Coroutine<Coroutine> WhenAny(params ReadOnlySpan<Coroutine> coroutines)
    {
        if (coroutines.Length == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(coroutines), "At least one coroutine was expected.");
        }

        foreach (var coroutine in coroutines)
        {
            if (coroutine.Status is not CoroutineStatus.Running)
            {
                return FromResult(coroutine);
            }
        }

        return WhenAnyCoroutineSource.Create(coroutines).Coroutine;
    }
}
