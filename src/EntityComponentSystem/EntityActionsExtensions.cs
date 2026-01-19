namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Extension methods for <see cref="World"/>
/// </summary>
public static class EntityActionsExtensions
{
    extension(IEntityActions actions)
    {
        /// <summary>
        /// Creates a new entity with the given <paramref name="name"/>.
        /// </summary>
        public Entity CreateEntity(string name)
        {
            var entity = actions.ThrowIfNull().CreateEntity();
            entity.Set(new Name() { Value = name });
            return entity;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the entity is alive and contains the component of type <typeparamref name="T"/>, <see langword="false"/> otherwise.
        /// </summary>
        public bool Has<T>(Identifier entity) => actions.ThrowIfNull().Has(entity, actions.GetComponentForType<T>());

        /// <summary>
        /// Returns the component of type <typeparamref name="T"/> for the <paramref name="entity"/>.
        /// </summary>
        /// <returns>The component for the <paramref name="entity"/>, or <see langword="default"/> if the component is missing.</returns>
        public T? Get<T>(Identifier entity) => actions.ThrowIfNull().Get<T>(entity, actions.GetComponentForType<T>());

        /// <summary>
        /// Returns a mutable reference of the component of type <typeparamref name="T"/> for the <paramref name="entity"/>.
        /// </summary>
        public ref T? GetMutable<T>(Identifier entity) => ref actions.ThrowIfNull().GetMutable<T>(entity, actions.GetComponentForType<T>());

        /// <summary>
        /// Adds the component of type <typeparamref name="T"/> to the <paramref name="entity"/>
        /// </summary>
        public void Add<T>(Identifier entity) => actions.ThrowIfNull().Add(entity, actions.GetComponentForType<T>());

        /// <summary>
        /// Sets the component's value for the <paramref name="entity"/> to the given <paramref name="value"/>
        /// </summary>
        public void Set<T>(Identifier entity, T value) => actions.ThrowIfNull().Set(entity, actions.GetComponentForType<T>(), value);

        /// <summary>
        /// Removes the component of type <typeparamref name="T"/> from the <paramref name="entity"/>
        /// </summary>
        public void Remove<T>(Identifier entity) => actions.ThrowIfNull().Remove(entity, actions.GetComponentForType<T>());

        /// <summary>
        /// Returns the entity mapped to the given <typeparamref name="TComponent"/> type.
        /// </summary>
        public Entity GetComponentForType<TComponent>() => actions.ThrowIfNull().World.GetComponentForType(typeof(TComponent));
    }
}
