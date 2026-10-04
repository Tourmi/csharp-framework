namespace Tourmi.Samples.Particles;

internal sealed class FrameTimeTracker(int bufferSize = 600)
{
    private readonly float[] _frameTimes = new float[bufferSize.ThrowIfNegativeOrZero()];

    private int _currentIndex;
    private int _warmupCount;

    public float AverageFrameTimeMilliseconds { get; private set; }

    public float AverageFrameRateSeconds => MathF.Min(99999, 1000f / AverageFrameTimeMilliseconds);

    public float WorstFrameTime { get; private set; }

    public void AddFrameTime(TimeSpan frameTime)
    {
        _frameTimes[_currentIndex] = (float)frameTime.TotalMilliseconds;

        var sampleCount = MathF.Min(50, _warmupCount);
        AverageFrameTimeMilliseconds = (AverageFrameTimeMilliseconds * sampleCount + (float)frameTime.TotalMilliseconds) / (sampleCount + 1);

        _currentIndex = (_currentIndex + 1) % _frameTimes.Length;
        _warmupCount++;
    }

    public void Refresh()
    {
        var worstTime = 0f;
        for (var i = 0; i < _frameTimes.Length; i++)
        {
            worstTime = MathF.Max(_frameTimes[i], worstTime);
        }

        WorstFrameTime = worstTime;
    }
}
