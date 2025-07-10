namespace Tourmi.Monogame.Tweens;

/// <summary>
/// Factory that takes care of creating and keeping track of existing tweens.
/// </summary>
public class TweenFactory
{
    /// <summary>
    /// Interpolates between the <paramref name="min"/> and <paramref name="max"/> based on the <paramref name="percent"/>
    /// </summary>
    public delegate T Interpolate<T>(T min, T max, float percent);

    private abstract class TweenStatus : ITweenStatus
    {
        public bool IsCompleted { get; set; }
        public TimeSpan CurrTime { get; set; }
        public required bool IsLooping { get; init; }
        public required TimeSpan TargetTime { get; init; }
        public required Curve TweenCurve { get; init; }
        public float PercentDone { get; set; }

        public TimeSpan TimeLeft => TargetTime - CurrTime;

        public abstract void UpdateValue(TimeSpan delta);
        public void Cancel() => IsCompleted = true;
    }

    private sealed class GenericTweenStatus<T> : TweenStatus
    {
        public required T StartValue { get; init; }
        public required T EndValue { get; init; }

        public required Action<T> Set { get; init; }
        public required Func<T, T, float, T> Interpolate { get; init; }

        public override void UpdateValue(TimeSpan delta) => Set(Interpolate(StartValue, EndValue, TweenCurve.Evaluate(PercentDone)));
    }

    private readonly List<TweenStatus> _tweens = [];

    /// <summary>
    /// Starts a new tween and returns its status.
    /// </summary>
    public ITweenStatus Create<T>(
        T startValue,
        T endValue,
        TimeSpan duration,
        Action<T> setter,
        Interpolate<T> interpolater,
        Curve? tweenCurve = null,
        bool looping = false,
        float startPercent = 0)
    {
        var newTween = new GenericTweenStatus<T>()
        {
            CurrTime = duration * startPercent.Clamp(0f, 1f),
            IsLooping = looping,
            TargetTime = duration,
            StartValue = startValue,
            EndValue = endValue,
            Set = setter,
            Interpolate = interpolater.ThrowIfNull().Invoke,
            TweenCurve = tweenCurve ?? CurveFactory.Linear(),
        };

        _tweens.Add(newTween);
        return newTween;
    }

    /// <summary>
    /// Updates all of the currently running tweens
    /// </summary>
    public void Update(TimeSpan deltaTime)
    {
        for (var i = 0; i < _tweens.Count; i++)
        {
            var curr = _tweens[i];
            UpdateTween(curr, deltaTime);

            if (curr.IsCompleted)
            {
                curr.Cancel();
                _tweens.RemoveAt(i);
                i--;
            }
        }
    }

    private static void UpdateTween(TweenStatus tween, TimeSpan deltaTime)
    {
        if (tween.IsCompleted)
        {
            return;
        }

        tween.CurrTime += deltaTime;
        if (tween.CurrTime >= tween.TargetTime)
        {
            if (tween.IsLooping)
            {
                tween.CurrTime -= tween.TargetTime;
            }
            else
            {
                tween.CurrTime = tween.TargetTime;
                tween.IsCompleted = true;
            }
        }

        tween.PercentDone = (float)(tween.CurrTime / tween.TargetTime);
        tween.UpdateValue(deltaTime);
    }
}
