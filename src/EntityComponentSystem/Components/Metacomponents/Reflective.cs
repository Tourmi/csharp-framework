namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// Marks a relation as Reflective.
/// ie: HasRelation(IsA, Car) will return <see langword="true"/> for the Car entity, 
/// even though it does not contain the Relation{IsA, Car} component.
/// </summary>
/// <remarks>
/// Queries will ignore reflective relations, due to technical constraints.
/// </remarks>
[TagComponent]
public readonly struct Reflective;
