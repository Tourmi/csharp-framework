namespace Tourmi.EntityComponentSystem.Archetypes.ComponentCollections;

internal interface IComponentCollection<T> :
    ICovariantComponentCollection<T>,
    IContravariantComponentCollection<T>,
    IComponentCollection
{
    /// <summary>
    /// Returns a reference to the entry at the <paramref name="index"/>
    /// </summary>
    new ref T? this[int index] { get; }

    /// <summary>
    /// Returns this collection as a <see cref="Span{T}"/>
    /// </summary>
    Span<T?> AsSpan();

    /// <inheritdoc cref="GetEnumerator"/>
    ComponentCollectionEnumerator<T> GetEnumerator();
}
