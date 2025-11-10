using Tourmi.EntityComponentSystem.Components.Tags;

namespace Tourmi.EntityComponentSystem.Relations;

/// <summary>
/// Built-in relation types that are known by the ECS and have custom logic surrounding them.
/// </summary>
public enum BuiltInRelationType : byte
{
    /// <summary>
    /// The relation does not map to any built-in types.
    /// </summary>
    None = 0,

    /// <summary>
    /// Marks X as a child of Y. Useful for hierarchies.
    /// <para />ie: an Apple entity is the child of a Tree entity, a Wheel entity is a child of a Car entity.
    /// </summary>
    ChildOf = 1,

    /// <summary>
    /// Defines that X is a Y. When adding X to an entity, Y will also be added to the entity.
    /// <para />ie: Apple is a Fruit, Car is a Vehicle.
    /// </summary>
    IsA = 2,

    /// <summary>
    /// Defines that X is an instance of Y. If a component does not exist in the entity, Y will be queried for the component.
    /// Usually used for instances of <see cref="Prefab"/>
    /// <para />ie: PlayerCar entity is an instance of CarPrefab
    /// </summary>
    InstanceOf = 3,

    /// <summary>
    /// Defines that X depends on Y. If Y is missing from the entity, an error will be thrown.
    /// <para />ie: Speed component depends on Position component.
    /// </summary>
    DependsOn = 4,
}
