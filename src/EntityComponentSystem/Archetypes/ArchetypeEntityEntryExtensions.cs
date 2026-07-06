using MovedEntity = (Tourmi.EntityComponentSystem.Ids.Identifier Id, int NewIndex);
namespace Tourmi.EntityComponentSystem.Archetypes;

/// <summary>
/// Extension methods for <see cref="ArchetypeEntityEntry"/>
/// </summary>
internal static class ArchetypeEntityEntryExtensions
{
    extension(ref ArchetypeEntityEntry entry)
    {
        /// <inheritdoc cref="Archetype.RemoveEntity(int)"/>
        public Option<MovedEntity> Remove()
        {
            var movedEntity = entry.Archetype.RemoveEntity(entry.Index);
            entry = default;
            return movedEntity;
        }

        /// <inheritdoc cref="Archetype.AddComponent(int, Identifier, Type?)"/>
        public Option<MovedEntity> AddComponent(Identifier componentIdentifier, Type? componentDataType)
        {
            (entry, var movedEntity) = entry.Archetype.AddComponent(entry.Index, componentIdentifier, componentDataType);
            return movedEntity;
        }

        /// <inheritdoc cref="Archetype.RemoveComponent(int, Identifier)"/>
        public Option<MovedEntity> RemoveComponent(Identifier componentIdentifier)
        {
            (entry, var movedEntity) = entry.Archetype.RemoveComponent(entry.Index, componentIdentifier);
            return movedEntity;
        }
    }
}
