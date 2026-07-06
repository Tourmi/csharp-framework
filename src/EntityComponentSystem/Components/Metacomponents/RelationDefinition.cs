namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// Meta-component that marks an entity as a relation definition. (ie: IsA, ChildOf, etc)
/// </summary>
/// <param name="RelationType">The relation type of the definition.</param>
public readonly record struct RelationDefinition(ushort RelationType);
