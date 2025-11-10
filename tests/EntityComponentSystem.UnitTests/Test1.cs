namespace Tourmi.EntityComponentSystem;

[TestFixture]
internal class Test1
{
    [Test]
    public void Test()
    {
        var ecs = World.Create();

        var someEntity = ecs.CreateEntity();
        Assert.That(ecs.IsAlive(someEntity), Is.True);

        ecs.Kill(someEntity);
        Assert.That(ecs.IsAlive(someEntity), Is.False);

        var newEntity = ecs.CreateEntity();
        Assert.That(newEntity.Id.ShortId, Is.EqualTo(someEntity.Id.ShortId));
        Assert.That(newEntity.Id.Version, Is.EqualTo(1));

        var secondEntity = ecs.CreateEntity();
        Assert.That(secondEntity.Id.ShortId, Is.EqualTo(newEntity.Id.ShortId + 1));
    }
}
