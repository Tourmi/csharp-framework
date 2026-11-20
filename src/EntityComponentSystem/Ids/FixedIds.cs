namespace Tourmi.EntityComponentSystem.Ids;

/// <summary>
/// Contains all entity Ids that are guaranteed to never change during the lifetime of an ecs.
/// </summary>
public static class FixedIds
{
    /// <summary>
    /// Region of reserved Ids for potential future use.
    /// </summary>
    public static class Reserved
    {
        /// <summary>
        /// Start of the Id region.
        /// </summary>
        public const uint RegionStart = 0x1;

        /// <summary>
        /// Size of the Id region.
        /// </summary>
        public const uint RegionSize = 0xFF;

        /// <summary>
        /// End of the id region, exclusive bound.
        /// </summary>
        public const uint RegionEnd = RegionStart + RegionSize;
    }

    /// <summary>
    /// Ids that have a special meaning
    /// </summary>
    public static class Special
    {
        /// <summary>
        /// Start of the Id region.
        /// </summary>
        public const uint RegionStart = Reserved.RegionEnd;

        /// <summary>
        /// Size of the Id region.
        /// </summary>
        public const uint RegionSize = 0x100;

        /// <summary>
        /// End of the id region, exclusive bound.
        /// </summary>
        public const uint RegionEnd = RegionStart + RegionSize;

        /// <summary>
        /// Special Id that acts as a placeholder for a query parameter determined at runtime.
        /// </summary>
        public static readonly Identifier Parameter1 = RegionStart + 0x01;

        /// <summary>
        /// Special Id that acts as a placeholder for a query parameter determined at runtime.
        /// </summary>
        public static readonly Identifier Parameter2 = RegionStart + 0x02;

        /// <summary>
        /// Special Id that acts as a placeholder for a query parameter determined at runtime.
        /// </summary>
        public static readonly Identifier Parameter3 = RegionStart + 0x03;

        /// <summary>
        /// Special Id that acts as a placeholder for a query parameter determined at runtime.
        /// </summary>
        public static readonly Identifier Parameter4 = RegionStart + 0x04;

        /// <summary>
        /// Special Id that acts as a placeholder for a query parameter determined at runtime.
        /// </summary>
        public static readonly Identifier Parameter5 = RegionStart + 0x05;

        /// <summary>
        /// Special Id that acts as a placeholder for a query parameter determined at runtime.
        /// </summary>
        public static readonly Identifier Parameter6 = RegionStart + 0x06;

        /// <summary>
        /// Special Id that acts as a placeholder for a query parameter determined at runtime.
        /// </summary>
        public static readonly Identifier Parameter7 = RegionStart + 0x07;

        /// <summary>
        /// Special Id that acts as a placeholder for a query parameter determined at runtime.
        /// </summary>
        public static readonly Identifier Parameter8 = RegionStart + 0x08;
    }

    /// <summary>
    /// Entity Ids of relation definitions.
    /// </summary>
    public static class Relations
    {
        /// <summary>
        /// Start of the Id region.
        /// </summary>
        public const uint RegionStart = Special.RegionEnd;

        /// <summary>
        /// Size of the Id region.
        /// </summary>
        public const uint RegionSize = 0x100;

        /// <summary>
        /// End of the id region, exclusive bound.
        /// </summary>
        public const uint RegionEnd = RegionStart + RegionSize;

        /// <inheritdoc cref="BuiltInRelationType.Undefined"/>
        public static readonly Identifier Undefined = RegionStart + (byte)BuiltInRelationType.Undefined;

        /// <inheritdoc cref="BuiltInRelationType.ChildOf"/>
        public static readonly Identifier ChildOf = RegionStart + (byte)BuiltInRelationType.ChildOf;

        /// <inheritdoc cref="BuiltInRelationType.IsA"/>
        public static readonly Identifier IsA = RegionStart + (byte)BuiltInRelationType.IsA;

        /// <inheritdoc cref="BuiltInRelationType.InstanceOf"/>
        public static readonly Identifier InstanceOf = RegionStart + (byte)BuiltInRelationType.InstanceOf;

        /// <inheritdoc cref="BuiltInRelationType.Requires"/>
        public static readonly Identifier Requires = RegionStart + (byte)BuiltInRelationType.Requires;

        /// <inheritdoc cref="BuiltInRelationType.DependsOn"/>
        public static readonly Identifier DependsOn = RegionStart + (byte)BuiltInRelationType.DependsOn;

        /// <inheritdoc cref="BuiltInRelationType.SubscribedTo"/>
        public static readonly Identifier SubscribedTo = RegionStart + (byte)BuiltInRelationType.SubscribedTo;

        /// <inheritdoc cref="BuiltInRelationType.Wildcard"/>
        public static readonly Identifier Wildcard = RegionStart + (byte)BuiltInRelationType.Wildcard;
    }

    /// <summary>
    /// Ids for special entities.
    /// </summary>
    public static class Entities
    {
        /// <summary>
        /// Start of the Id region.
        /// </summary>
        public const uint RegionStart = Relations.RegionEnd;

        /// <summary>
        /// Size of the Id region.
        /// </summary>
        public const uint RegionSize = 0x100;

        /// <summary>
        /// End of the id region, exclusive bound.
        /// </summary>
        public const uint RegionEnd = RegionStart + RegionSize;

        /// <summary>
        /// Singleton entity that stores the <see cref="EntityComponentSystem.World"/>
        /// </summary>
        public static readonly Identifier World = RegionStart + 0x00;
    }

    /// <summary>
    /// Entity Ids of built-in components.
    /// </summary>
    public static class Components
    {
        /// <summary>
        /// Start of the Id region.
        /// </summary>
        public const uint RegionStart = Entities.RegionEnd;

        /// <summary>
        /// Size of the Id region.
        /// </summary>
        public const uint RegionSize = 0x100;

        /// <summary>
        /// End of the id region, exclusive bound.
        /// </summary>
        public const uint RegionEnd = RegionStart + RegionSize;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.Component"/>
        /// </summary>
        public static readonly Identifier Component = RegionStart + 0x00;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.DataComponent"/>
        /// </summary>
        public static readonly Identifier DataComponent = RegionStart + 0x01;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Name"/>
        /// </summary>
        public static readonly Identifier Name = RegionStart + 0x02;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.RelationDefinition"/>
        /// </summary>
        public static readonly Identifier RelationDefinition = RegionStart + 0x03;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.Singleton"/>
        /// </summary>
        public static readonly Identifier Singleton = RegionStart + 0x04;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Tags.Prefab"/>
        /// </summary>
        public static readonly Identifier Prefab = RegionStart + 0x05;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Schedule"/>
        /// </summary>
        public static readonly Identifier Schedule = RegionStart + 0x06;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.SystemComponent"/>
        /// </summary>
        public static readonly Identifier SystemComponent = RegionStart + 0x07;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.Unique"/>
        /// </summary>
        public static readonly Identifier Unique = RegionStart + 0x08;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.EventActionSystem"/>
        /// </summary>
        public static readonly Identifier EventActionSystem = RegionStart + 0x09;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.Symmetric"/>
        /// </summary>
        public static readonly Identifier Symmetric = RegionStart + 0x10;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.Transitive"/>
        /// </summary>
        public static readonly Identifier Transitive = RegionStart + 0x11;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.Reflective"/>
        /// </summary>
        public static readonly Identifier Reflective = RegionStart + 0x12;
    }

    /// <summary>
    /// Entity Ids of built-in events.
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Start of the Id region.
        /// </summary>
        public const uint RegionStart = Components.RegionEnd;

        /// <summary>
        /// Size of the Id region.
        /// </summary>
        public const uint RegionSize = 0x100;

        /// <summary>
        /// End of the id region, exclusive bound.
        /// </summary>
        public const uint RegionEnd = RegionStart + RegionSize;

        /// <summary>
        /// Event that is raised right before a <see cref="Tick"/> event.
        /// </summary>
        public static readonly Identifier PreTick = RegionStart + 0x00;

        /// <summary>
        /// The default Tick event, which new systems are usually subscribed to.
        /// </summary>
        public static readonly Identifier Tick = RegionStart + 0x01;

        /// <summary>
        /// Event that is raised after a <see cref="Tick"/> event.
        /// </summary>
        public static readonly Identifier PostTick = RegionStart + 0x02;

        /// <summary>
        /// Entity event raised upon adding a component (or removing a component when using a Not filter)
        /// </summary>
        public static readonly Identifier OnAdd = RegionStart + 0x03;

        /// <summary>
        /// Entity event raised upon any modifications to its filtered components.
        /// </summary>
        public static readonly Identifier OnModified = RegionStart + 0x04;

        /// <summary>
        /// Entity event raised upon removing a component (or adding a component when using a Not filter)
        /// </summary>
        public static readonly Identifier OnRemove = RegionStart + 0x05;

        /// <summary>
        /// Event raised when an entity is killed.
        /// </summary>
        public static readonly Identifier OnKill = RegionStart + 0x06;

        /// <summary>
        /// Event raised when the target of a relation is killed.
        /// </summary>
        public static readonly Identifier OnTargetKill = RegionStart + 0x07;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Relations.OnExtend"/>
        /// </summary>
        public static readonly Identifier OnExtend = RegionStart + 0x08;

        /// <summary>
        /// Shorthand for a Relation(OnExtend, Copy) id.
        /// </summary>
        public static readonly RelationComponentIdentifier OnExtendCopy = Actions.Copy.ToRelationId().WithRelationType(BuiltInRelationType.OnExtend);

        /// <summary>
        /// Shorthand for a Relation(OnExtend, Inherit) id.
        /// </summary>
        public static readonly RelationComponentIdentifier OnExtendInherit = Actions.Inherit.ToRelationId().WithRelationType(BuiltInRelationType.OnExtend);

        /// <summary>
        /// Shorthand for a Relation(OnExtend, Ignore) id.
        /// </summary>
        public static readonly RelationComponentIdentifier OnExtendIgnore = Actions.Ignore.ToRelationId().WithRelationType(BuiltInRelationType.OnExtend);
    }

    /// <summary>
    /// Entity Ids of built-in actions.
    /// </summary>
    public static class Actions
    {
        /// <summary>
        /// Start of the Id region.
        /// </summary>
        public const uint RegionStart = Events.RegionEnd;

        /// <summary>
        /// Size of the Id region.
        /// </summary>
        public const uint RegionSize = 0x100;

        /// <summary>
        /// End of the id region, exclusive bound.
        /// </summary>
        public const uint RegionEnd = RegionStart + RegionSize;

        /// <summary>
        /// Action that removes the component upon an event such as OnKill or OnTargetKill
        /// </summary>
        public static readonly Identifier Remove = RegionStart + 0x00;

        /// <summary>
        /// Action that kills the entity with the component upon an event such as OnKill or OnTargetKill
        /// </summary>
        public static readonly Identifier Kill = RegionStart + 0x01;

        /// <summary>
        /// Action that crashes the application upon an event such as OnKill or OnTargetKill
        /// </summary>
        public static readonly Identifier Panic = RegionStart + 0x02;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.Copy"/>
        /// </summary>
        public static readonly Identifier Copy = RegionStart + 0x03;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.Inherit"/>
        /// </summary>
        public static readonly Identifier Inherit = RegionStart + 0x04;

        /// <summary>
        /// <see cref="EntityComponentSystem.Components.Metacomponents.Ignore"/>
        /// </summary>
        public static readonly Identifier Ignore = RegionStart + 0x05;
    }

    /// <summary>
    /// Start of the region of Core Ids.
    /// </summary>
    public const uint CoreRegionStart = Reserved.RegionStart;

    /// <summary>
    /// End of the region of Core Ids, exclusive. New entities may be created starting from this Id.
    /// </summary>
    public const uint CoreRegionEnd = Actions.RegionEnd;

    /// <summary>
    /// Size of the region of Core Ids.
    /// </summary>
    public const uint CoreRegionSize = CoreRegionEnd - CoreRegionStart;

    /// <summary>
    /// Reserved region of core ids.
    /// </summary>
    public static readonly IdentifierRegion CoreRegion = new() { Offset = CoreRegionStart, Amount = CoreRegionSize, Name = "Core" };
}
