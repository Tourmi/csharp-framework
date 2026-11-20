namespace Tourmi.EntityComponentSystem.Features;

[TestFixture]
internal class IsATests
{
    [Test]
    public void DirectAccessInherit()
    {
        var ecs = World.Create();

        var component = ecs.CreateComponent<int>().OnExtendInherit();

        var superSuperEntity = ecs.CreateEntity();
        ecs.Set(superSuperEntity, component, 1);

        var superEntity = ecs.CreateEntity();
        superEntity.IsA(superSuperEntity);

        var entity = ecs.CreateEntity();
        entity.IsA(superEntity);

        Assert.That(entity.Has(component));
        Assert.That(entity.Owns(component), Is.False);
        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(1));

        ecs.GetMutable<int>(entity, component) = 2;
        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(2));

        ecs.Set<int>(entity, component, 3);

        Assert.That(entity.Owns(component));
        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(3));
        Assert.That(ecs.Get<int>(superSuperEntity, component), Is.EqualTo(2));

        ecs.Remove(entity, component);

        Assert.That(entity.Has(component));
        Assert.That(entity.Owns(component), Is.False);
        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(2));
    }

    [Test]
    public void DirectAccessCopy()
    {
        var ecs = World.Create();

        var component = ecs.CreateComponent<int>().OnExtendCopy();

        var superSuperEntity = ecs.CreateEntity();
        ecs.Set(superSuperEntity, component, 1);

        var superEntity = ecs.CreateEntity();
        superEntity.IsA(superSuperEntity);

        var entity = ecs.CreateEntity();
        entity.IsA(superEntity);

        Assert.That(entity.Owns(component));
        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(1));

        ecs.GetMutable<int>(entity, component) = 2;
        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(2));
        Assert.That(ecs.Get<int>(superSuperEntity, component), Is.EqualTo(1));

        ecs.Set<int>(entity, component, 3);

        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(3));
        Assert.That(ecs.Get<int>(superSuperEntity, component), Is.EqualTo(1));

        ecs.Remove(entity, component);

        Assert.That(entity.Has(component), Is.False);
    }

    [Test]
    public void DirectAccessCopyDefaultBehavior()
    {
        var ecs = World.Create();

        var component = ecs.CreateComponent<int>();

        var superSuperEntity = ecs.CreateEntity();
        ecs.Set(superSuperEntity, component, 1);

        var superEntity = ecs.CreateEntity();
        superEntity.IsA(superSuperEntity);

        var entity = ecs.CreateEntity();
        entity.IsA(superEntity);

        Assert.That(entity.Owns(component));
        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(1));

        ecs.GetMutable<int>(entity, component) = 2;
        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(2));
        Assert.That(ecs.Get<int>(superSuperEntity, component), Is.EqualTo(1));

        ecs.Set<int>(entity, component, 3);

        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(3));
        Assert.That(ecs.Get<int>(superSuperEntity, component), Is.EqualTo(1));

        ecs.Remove(entity, component);

        Assert.That(entity.Has(component), Is.False);
    }

    [Test]
    public void DirectAccessIgnore()
    {
        var ecs = World.Create();

        var component = ecs.CreateComponent<int>().OnExtendIgnore();

        var superSuperEntity = ecs.CreateEntity();
        ecs.Set(superSuperEntity, component, 1);

        var superEntity = ecs.CreateEntity();
        superEntity.IsA(superSuperEntity);

        var entity = ecs.CreateEntity();
        entity.IsA(superEntity);

        Assert.That(entity.Has(component), Is.False);

        ecs.Set<int>(entity, component, 3);

        Assert.That(ecs.Get<int>(entity, component), Is.EqualTo(3));
        Assert.That(ecs.Get<int>(superSuperEntity, component), Is.EqualTo(1));

        ecs.Remove(entity, component);

        Assert.That(entity.Has(component), Is.False);
    }
}
