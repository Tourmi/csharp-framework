namespace Tourmi.Monogame.Tweens;

/// <summary>
/// Extension methods for <see cref="TweenFactory"/>
/// </summary>
public static class TweenFactoryExtensions
{
    /// <summary>
    /// Creates a new tween for a float property.
    /// </summary>
    public static ITweenStatus Create<T>(
        this TweenFactory factory,
        TimeSpan duration,
        Action<float> setter,
        float min = 0f,
        float max = 1f,
        Curve? tweenCurve = null,
        bool looping = false,
        float startPercent = 0)
    {
        _ = factory.ThrowIfNull();

        return factory.Create(
            min,
            max,
            duration,
            setter,
            static (min, max, percent) => min + (max - min) * percent,
            tweenCurve,
            looping,
            startPercent);
    }

    /// <summary>
    /// Creates a new tween for a double property.
    /// </summary>
    public static ITweenStatus Create<T>(
        this TweenFactory factory,
        TimeSpan duration,
        Action<double> setter,
        double min = 0f,
        double max = 1f,
        Curve? tweenCurve = null,
        bool looping = false,
        float startPercent = 0)
    {
        _ = factory.ThrowIfNull();

        return factory.Create(
            min,
            max,
            duration,
            setter,
            static (min, max, percent) => min + (max - min) * percent,
            tweenCurve,
            looping,
            startPercent);
    }

    /// <summary>
    /// Creates a new tween for a <see cref="Vector2"/> property
    /// </summary>
    public static ITweenStatus Create<T>(
        this TweenFactory factory,
        TimeSpan duration,
        Action<Vector2> setter,
        Vector2 min,
        Vector2 max,
        Curve? tweenCurve = null,
        bool looping = false,
        float startPercent = 0)
    {
        _ = factory.ThrowIfNull();

        return factory.Create(
            min,
            max,
            duration,
            setter,
            static (min, max, percent) => min + (max - min) * percent,
            tweenCurve,
            looping,
            startPercent);
    }

    /// <summary>
    /// Creates a new tween for a <see cref="Vector3"/> property
    /// </summary>
    public static ITweenStatus Create<T>(
        this TweenFactory factory,
        TimeSpan duration,
        Action<Vector3> setter,
        Vector3 min,
        Vector3 max,
        Curve? tweenCurve = null,
        bool looping = false,
        float startPercent = 0)
    {
        _ = factory.ThrowIfNull();

        return factory.Create(
            min,
            max,
            duration,
            setter,
            static (min, max, percent) => min + (max - min) * percent,
            tweenCurve,
            looping,
            startPercent);
    }

    /// <summary>
    /// Creates a new tween for a <see cref="Vector4"/> property
    /// </summary>
    public static ITweenStatus Create<T>(
        this TweenFactory factory,
        TimeSpan duration,
        Action<Vector4> setter,
        Vector4 min,
        Vector4 max,
        Curve? tweenCurve = null,
        bool looping = false,
        float startPercent = 0)
    {
        _ = factory.ThrowIfNull();

        return factory.Create(
            min,
            max,
            duration,
            setter,
            static (min, max, percent) => min + (max - min) * percent,
            tweenCurve,
            looping,
            startPercent);
    }
}
