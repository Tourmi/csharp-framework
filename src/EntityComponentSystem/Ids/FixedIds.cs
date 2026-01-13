namespace Tourmi.EntityComponentSystem.Ids;

/// <summary>
/// Contains all entity Ids that are guaranteed to never change during the lifetime of an ecs.
/// </summary>
public static class FixedIds
{
    /// <summary>
    /// Start of the reserved region for Ids. May be used in the future.
    /// </summary>
    public const uint ReservedRegionStart = 0x1;

    /// <summary>
    /// Size of the reserved region of Ids. May be used in the future.
    /// </summary>
    public const ushort ReservedRegionSize = 0xFF;

    /// <summary>
    /// Start of the Id range containing Core entities needed for the ECS to work properly.
    /// </summary>
    public const uint CoreRegionStart = Relations.RegionStart + Relations.RegionSize;

    /// <summary>
    /// Size of the Id range of Core entities.
    /// </summary>
    public const ushort CoreRegionSize = 0x400;

    /// <summary>
    /// Singleton entity that stores the <see cref="EntityComponentSystem.World"/>
    /// </summary>
    public static readonly Identifier World = CoreRegionStart + 0;

    /// <summary>
    /// Entity Ids of relation definitions.
    /// </summary>
    public static class Relations
    {
        /// <summary>
        /// Start of the region for relation definitions.
        /// </summary>
        public const uint RegionStart = ReservedRegionStart + ReservedRegionSize;

        /// <summary>
        /// Size of the region for relation definitions.
        /// </summary>
        public const ushort RegionSize = 0x100;

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
    }

    /// <summary>
    /// Entity Ids of components.
    /// </summary>
    public static class ComponentIds
    {
        /// <summary>
        /// <see cref="Components.Metacomponents.Component"/>
        /// </summary>
        public static readonly Identifier Component = CoreRegionStart + 1;

        /// <summary>
        /// <see cref="Components.Metacomponents.DataComponent"/>
        /// </summary>
        public static readonly Identifier DataComponent = CoreRegionStart + 2;

        /// <summary>
        /// <see cref="Components.Name"/>
        /// </summary>
        public static readonly Identifier Name = CoreRegionStart + 3;

        /// <summary>
        /// <see cref="Components.Metacomponents.RelationDefinition"/>
        /// </summary>
        public static readonly Identifier RelationDefinition = CoreRegionStart + 4;

        /// <summary>
        /// <see cref="Components.Metacomponents.Singleton"/>
        /// </summary>
        public static readonly Identifier Singleton = CoreRegionStart + 5;

        /// <summary>
        /// <see cref="Components.Tags.Prefab"/>
        /// </summary>
        public static readonly Identifier Prefab = CoreRegionStart + 6;

        /// <summary>
        /// <see cref="Components.Schedule"/>
        /// </summary>
        public static readonly Identifier Schedule = CoreRegionStart + 7;

        /// <summary>
        /// <see cref="Components.SystemComponent"/>
        /// </summary>
        public static readonly Identifier SystemComponent = CoreRegionStart + 8;
    }
}
