using System.ComponentModel;
using Tourmi.EntityComponentSystem.Components;

namespace Tourmi.EntityComponentSystem;

[InProcess]
[HideColumns("StdDev", "RatioSD")]
[MemoryDiagnoser]
public class Systems
{
    public record struct Position(float X, float Y);
    public record struct Speed(float X, float Y);

    private protected World? _ecs;

    public int AdditionalComponentCount { get; set; } = 10;

    [Params(100, 100_000, Priority = 1)]
    public int EntityCount { get; set; } = 1;

    [Params(10, 100)]
    public int SystemCount { get; set; } = 1;

    [GlobalSetup]
    public void GlobalSetup()
    {
        _ecs = World.Create();
        var random = new Random(0);

        var additionalComponents = new Identifier[AdditionalComponentCount];
        for (var i = 0; i < AdditionalComponentCount; i++)
        {
            var entity = _ecs.CreateEntity();
            entity.Add<Component>();
            entity.Set<Name>(new($"Additional Component {i}"));
            additionalComponents[i] = entity;
        }

        for (var i = 0; i < EntityCount; i++)
        {
            var entity = _ecs.CreateEntity();

            // Make sure not all entities have the same archetype by randomly giving them components.
            while (random.NextSingle() > 0.5)
            {
                var componentId = additionalComponents[random.Next(AdditionalComponentCount)];
                if (!_ecs.Has(entity, componentId))
                {
                    _ecs.Add(entity, componentId);
                }
            }

            // Half of entities have a position
            if (random.NextSingle() > 0.5)
            {
                entity.Set<Position>(new(random.Next(-1000, 1000), random.Next(-1000, 1000)));
            }

            // Half of entities have a speed
            if (random.NextSingle() > 0.5)
            {
                entity.Set<Speed>(new(random.Next(-1000, 1000), random.Next(-1000, 1000)));
            }
        }

        for (var i = 0; i < SystemCount; i++)
        {
            _ecs.AddSystem<Ref<Position>, RefReadonly<Speed>>(SimpleSystem);
        }
    }

    [Benchmark]
    public void RunSystems()
    {
        _ecs!.RunSystems();
    }

    private protected static void SimpleSystem(Ref<Position> positionRef, RefReadonly<Speed> speedRef)
    {
        ref var position = ref positionRef.Reference;
        ref readonly var speed = ref speedRef.Reference;

        position.X += speed.X;
        position.Y += speed.Y;
    }
}
