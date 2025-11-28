namespace Tourmi.EntityComponentSystem.Archetypes;

/// <summary>
/// An entity entry into an <see cref="Archetypes.Archetype"/>.
/// </summary>
/// <param name="Archetype">The <see cref="Archetypes.Archetype"/> this entry is for</param>
/// <param name="Index">The index that points to the entity within the <see cref="Archetypes.Archetype"/></param>
internal readonly record struct ArchetypeEntityEntry(Archetype Archetype, int Index)
{
    /// <inheritdoc cref="Archetype.SetValue"/>
    public void SetValue<T>(Identifier componentId, T value) => Archetype.SetValue(Index, componentId, value);

    /// <inheritdoc cref="Archetype.GetValue"/>
    public T? GetValue<T>(Identifier componentId) => Archetype.GetValue<T>(Index, componentId);

    /// <inheritdoc cref="Archetype.GetValueRef"/>
    public ref T? GetValueRef<T>(Identifier componentId) => ref Archetype.GetValueRef<T>(Index, componentId);

    /// <inheritdoc cref="Archetype.GetDebugValue"/>
    internal object? GetDebugValue(Identifier componentId) => Archetype.GetDebugValue(Index, componentId);
}
