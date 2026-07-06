namespace Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

internal interface ICovariantComponentCollection<out T> : IComponentCollection
{
    /// <summary>
    /// Gets the value at the given <paramref name="index"/>
    /// </summary>
    T? this[int index] { get; }
}
