using Tourmi.Coroutines.Contexts;

namespace Tourmi.Coroutines;
internal static class CoroutineContextExtensions
{
    public static void EnterExit(this CoroutineContext context, int amount = 1)
    {
        var count = 0;
        while (count < amount)
        {
            using var entry = context.Enter();
            count++;
        }
    }
}
