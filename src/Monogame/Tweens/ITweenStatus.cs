namespace Tourmi.Monogame.Tweens;

/// <summary>
/// Status of a tween
/// </summary>
public interface ITweenStatus
{
    /// <summary>
    /// Whether a tween has completed or not. 
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Whether the tween is infinitely looping or not.
    /// </summary>
    bool IsLooping { get; }

    /// <summary>
    /// Amount of time left for the tween.
    /// </summary>
    TimeSpan TimeLeft { get; }

    /// <summary>
    /// Cancels the tween.
    /// </summary>
    void Cancel();
}
