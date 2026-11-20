namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Extension methods for <see cref="World"/>, relating to creating Systems.
/// </summary>
public static class WorldComponentExtensions
{
    extension(World world)
    {
        /// <summary>
        /// Creates and returns a new Tag, a component with no data.
        /// </summary>
        public ComponentEntity CreateTag(string? name = null)
        {
            var entity = world.ThrowIfNull().CreateEntity();
            entity.Add<Component>();

            if (name is not null && name.Length > 0)
            {
                entity.Set<Name>(new(name));
            }

            return new ComponentEntity(entity);
        }

        /// <summary>
        /// Creates and returns a new component with data of type <typeparamref name="T"/>.
        /// </summary>
        public ComponentEntity CreateComponent<T>(string? name = null)
        {
            var entity = world.ThrowIfNull().CreateEntity();
            entity.Add<Component>();
            entity.Set<DataComponent>(new(typeof(T)));

            if (name is not null && name.Length > 0)
            {
                entity.Set<Name>(new(name));
            }

            return new ComponentEntity(entity);
        }

        /// <summary>
        /// Returns the component represented by the given <see langword="type"/> <typeparamref name="T"/>, or an invalid entity if it is invalid.
        /// </summary>
        public ComponentEntity GetComponent<T>()
        {
            var entity = world.ThrowIfNull().GetEntityForType<T>();
            if (entity.Id.IsRelationId)
            {
                return default;
            }

            return new ComponentEntity(entity);
        }
    }
}
