namespace Tourmi.EntityComponentSystem.Components.Metacomponents;

/// <summary>
/// Marks a relation as symmetric. 
/// ie: Adding (AtWarWith, X) to entity Y will also add (AtWarWith, Y) to entity X.
/// Removing the relation will remove it from both entities as well.
/// </summary>
[TagComponent]
public readonly struct Symmetric;
