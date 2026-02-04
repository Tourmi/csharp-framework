namespace Tourmi.Samples.Particles;

internal sealed class FrameTimeTracker(int bufferSize = 600)
{
    private readonly float[] _frameTimes = new float[bufferSize.ThrowIfNegativeOrZero()];

    private float _averageFrameTime;
    private int _currentIndex;
    private int _warmupCount;

    public float AverageFrameTimeMilliseconds => _averageFrameTime / MathF.Max(1, MathF.Min(_frameTimes.Length, _warmupCount));

    public float WorstFrameTime
    {
        get
        {
            var time = 0f;
            for (var i = 0; i < _frameTimes.Length; i++)
            {
                time = MathF.Max(_frameTimes[i], time);
            }

            return time;
        }
    }

    public float AverageFrameRateSeconds => 1000f / AverageFrameTimeMilliseconds;

    public void AddFrameTime(TimeSpan frameTime)
    {
        _averageFrameTime -= _frameTimes[_currentIndex];

        _frameTimes[_currentIndex] = (float)frameTime.TotalMilliseconds;
        _averageFrameTime += (float)frameTime.TotalMilliseconds;

        _currentIndex = (_currentIndex + 1) % _frameTimes.Length;
        _warmupCount++;
    }
}
