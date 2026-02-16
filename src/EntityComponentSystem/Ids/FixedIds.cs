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
        /// Special Id which when used as the target of a Relation Id, will match any target.
        /// </summary>
        public static readonly Identifier Wildcard = new Identifier(RegionStart + 0x00) | IdentifierTypes.Relation;
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
    public static class ComponentIds
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
        /// <see cref="Components.Metacomponents.Component"/>
        /// </summary>
        public static readonly Identifier Component = RegionStart + 0x00;

        /// <summary>
        /// <see cref="Components.Metacomponents.DataComponent"/>
        /// </summary>
        public static readonly Identifier DataComponent = RegionStart + 0x01;

        /// <summary>
        /// <see cref="Components.Name"/>
        /// </summary>
        public static readonly Identifier Name = RegionStart + 0x02;

        /// <summary>
        /// <see cref="Components.Metacomponents.RelationDefinition"/>
        /// </summary>
        public static readonly Identifier RelationDefinition = RegionStart + 0x03;

        /// <summary>
        /// <see cref="Components.Metacomponents.Singleton"/>
        /// </summary>
        public static readonly Identifier Singleton = RegionStart + 0x04;

        /// <summary>
        /// <see cref="Components.Tags.Prefab"/>
        /// </summary>
        public static readonly Identifier Prefab = RegionStart + 0x05;

        /// <summary>
        /// <see cref="Components.Schedule"/>
        /// </summary>
        public static readonly Identifier Schedule = RegionStart + 0x06;

        /// <summary>
        /// <see cref="Components.SystemComponent"/>
        /// </summary>
        public static readonly Identifier SystemComponent = RegionStart + 0x07;
    }

    /// <summary>
    /// Entity Ids of built-in events.
    /// </summary>
    public static class Events
    {
        /// <summary>
        /// Start of the Id region.
        /// </summary>
        public const uint RegionStart = ComponentIds.RegionEnd;

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
    }

    /// <summary>
    /// Start of the region of Core Ids.
    /// </summary>
    public const uint CoreRegionStart = Reserved.RegionStart;

    /// <summary>
    /// End of the region of Core Ids, exclusive. New entities may be created starting from this Id.
    /// </summary>
    public const uint CoreRegionEnd = Events.RegionEnd;

    /// <summary>
    /// Size of the region of Core Ids.
    /// </summary>
    public const uint CoreRegionSize = CoreRegionEnd - CoreRegionStart;

    /// <summary>
    /// Reserved region of core ids.
    /// </summary>
    public static readonly IdentifierRegion CoreRegion = new() { Offset = CoreRegionStart, Amount = CoreRegionSize, Name = "Core" };
}
