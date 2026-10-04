namespace Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

internal interface IContravariantComponentCollection<in T> : IComponentCollection
{
    /// <summary>
    /// Sets the value at the given <paramref name="index"/>
    /// </summary>
    T? this[int index] { set; }
}
