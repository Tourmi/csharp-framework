using Tourmi.Coroutines.Sources;

namespace Tourmi.Coroutines.Runners;

internal interface ICoroutineRunner<TResult> : ICoroutineSource<TResult>, IQueueAble
{
    Action Continuation { get; }
}
