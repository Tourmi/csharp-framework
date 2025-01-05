namespace Tourmi.Coroutines;

public class Tests
{
    [Test]
    public async Coroutine Test1()
    {
        var test1 = new Coroutine();

        await new Coroutine();

        var test2 = new Coroutine();

        await new Coroutine();

        var test3 = new Coroutine();
    }
}
