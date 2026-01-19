using Tourmi.EntityComponentSystem.Exceptions;

namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Extension methods for <see cref="Entity"/>
/// </summary>
public static class EntityExtensions
{
    /// <param name="entity">The entity that will be acted on</param>
    extension(Entity entity)
    {
        /// <summary>
        /// Returns the name of the entity, if it has one.
        /// </summary>
        public string? Name => entity.Get<Name>().Value;

        /// <summary>
        /// Returns the display name of the entity, it being the entity's <see cref="Name"/> if it has one, 
        /// otherwise returns its <see cref="Entity.Id"/>.
        /// </summary>
        public string DisplayName => entity.Name ?? entity.Id.ToString();

        /// <summary>
        /// Adds the component of type <typeparamref name="TComponent"/> to the entity, instantiating it with the parameterless constructor.
        /// Does nothing if the entity already has the component.
        /// </summary>
        public void Add<TComponent>() => entity.World?.Add<TComponent>(entity);

        /// <summary>
        /// Adds the component with <paramref name="componentId"/> to the entity.
        /// Does nothing if the entity already has the component.
        /// </summary>
        /// <remarks>
        /// This overload should be used if the component does not have a value. ie: for flags or states
        /// </remarks>
        public void Add(Identifier componentId) => entity.World?.Add(entity, componentId);

        /// <summary>
        /// Removes the <typeparamref name="TComponent"/> from the entity.
        /// </summary>
        public void Remove<TComponent>() => entity.World?.Remove<TComponent>(entity);

        /// <summary>
        /// Removes the component with <paramref name="componentId"/> from the entity.
        /// </summary>
        public void Remove(Identifier componentId) => entity.World?.Remove(entity, componentId);

        /// <summary>
        /// Returns a readonly copy of the <typeparamref name="TComponent"/> from the entity.
        /// Will throw if the entity does not contain the <typeparamref name="TComponent"/>.
        /// </summary>
        public TComponent? Get<TComponent>() => entity.World is null ? default : entity.World.Get<TComponent>(entity);

        /// <summary>
        /// Returns a readonly copy of the <typeparamref name="TComponent"/> with <paramref name="componentId"/> from the <paramref name="entity"/>.
        /// Will throw if the entity does not contain a component with the given <paramref name="componentId"/>.
        /// </summary>
        public TComponent? Get<TComponent>(Identifier componentId) => entity.World is null ? default : entity.World.Get<TComponent>(entity, componentId);

        /// <summary>
        /// Returns the <typeparamref name="TComponent"/> from the entity.
        /// Will throw if the entity does not contain the <typeparamref name="TComponent"/>.
        /// </summary>
        public ref TComponent? GetMutable<TComponent>()
        {
            if (entity.World is null)
            {
                EntityInvalidException.ThrowEntityUninitialized(entity);
            }

            return ref entity.World.GetMutable<TComponent>(entity);
        }

        /// <summary>
        /// Returns the <typeparamref name="TComponent"/> with <paramref name="componentId"/> from the <paramref name="entity"/>.
        /// Will throw if the entity does not contain a component with the given <paramref name="componentId"/>.
        /// </summary>
        public ref TComponent? GetMutable<TComponent>(Identifier componentId)
        {
            if (entity.World is null)
            {
                EntityInvalidException.ThrowEntityUninitialized(entity);
            }

            return ref entity.World.GetMutable<TComponent>(entity, componentId);
        }

        /// <summary>
        /// Adds the <typeparamref name="TComponent"/> to the entity if it doesn't contain it,
        /// then returns the value of the component.
        /// </summary>
        public TComponent? Ensure<TComponent>() => entity.World is null ? default : entity.World.Ensure<TComponent>(entity);

        /// <summary>
        /// Adds the <typeparamref name="TComponent"/> with <paramref name="componentId"/> to the entity if it doesn't contain it,
        /// then returns the value of the component.
        /// </summary>
        public TComponent? Ensure<TComponent>(Identifier componentId) => entity.World is null ? default : entity.World.Ensure<TComponent>(entity, componentId);

        /// <summary>
        /// Adds the <typeparamref name="TComponent"/> to the entity if it doesn't contain it,
        /// then returns the component.
        /// </summary>
        public ref TComponent? EnsureMutable<TComponent>()
        {
            if (entity.World is null)
            {
                EntityInvalidException.ThrowEntityUninitialized(entity);
            }

            return ref entity.World.EnsureMutable<TComponent>(entity);
        }

        /// <summary>
        /// Adds the <typeparamref name="TComponent"/> with <paramref name="componentId"/> to the entity if it doesn't contain it,
        /// then returns the component.
        /// </summary>
        public ref TComponent? EnsureMutable<TComponent>(Identifier componentId)
        {
            if (entity.World is null)
            {
                EntityInvalidException.ThrowEntityUninitialized(entity);
            }

            return ref entity.World.EnsureMutable<TComponent>(entity, componentId);
        }

        /// <summary>
        /// Returns <see langword="true"/> if the entity contains the given component.
        /// </summary>
        public bool Has<TComponent>() => entity.World?.Has<TComponent>(entity) ?? false;

        /// <summary>
        /// Returns <see langword="true"/> if the entity contains the given component with <paramref name="componentId"/>.
        /// </summary>
        public bool Has(Identifier componentId) => entity.World?.Has(entity, componentId) ?? false;

        /// <summary>
        /// Sets the component for the entity, adding it if the entity did not contain it.
        /// </summary>
        public void Set<TComponent>(TComponent component) => entity.World?.Set(entity, component);

        /// <summary>
        /// Sets the component with <paramref name="componentId"/> for the entity, adding it if the entity did not contain it.
        /// </summary>
        public void Set<TComponent>(Identifier componentId, TComponent component) => entity.World?.Set(entity, componentId, component);

        /// <summary>
        /// Returns <see langword="true"/> if the entity is alive.
        /// </summary>
        public bool IsAlive() => entity.World?.IsAlive(entity) ?? false;

        /// <summary>
        /// Kills the entity, deleting its components.
        /// </summary>
        public void Kill() => entity.World?.Kill(entity);
    }
}
