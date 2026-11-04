namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Extension methods for <see cref="ComponentEntity"/>
/// </summary>
public static class ComponentEntityExtensions
{
    /// <param name="entity">This component</param>
    extension(ComponentEntity entity)
    {
        /// <summary>
        /// Returns the component entity as an entity.
        /// </summary>
        public Entity AsEntity() => entity;

        /// <summary>
        /// Adds the on-add <paramref name="action"/> to the component, creating a new entity containing the given <paramref name="action"/> as their <see cref="EventActionSystem"/>.
        /// </summary>
        public ComponentEntity OnAdd(EventActionSystem.EventAction action)
        {
            if (entity.Actions is null)
            {
                throw new InvalidOperationException("Cannot add on-add action to an invalid entity.");
            }

            var actionSystem = entity.Actions.CreateEntity();
            actionSystem.Set<EventActionSystem>(new(action.ThrowIfNull()));

            entity.AsEntity().AddRelation(BuiltInRelationType.OnAdd, actionSystem);

            return entity;
        }

        /// <summary>
        /// Adds the on-modified <paramref name="action"/> to the component, creating a new entity containing the given <paramref name="action"/> as their <see cref="EventActionSystem"/>.
        /// </summary>
        public ComponentEntity OnModified(EventActionSystem.EventAction action)
        {
            if (entity.Actions is null)
            {
                throw new InvalidOperationException("Cannot add on-modified action to an invalid entity.");
            }

            var actionSystem = entity.Actions.CreateEntity();
            actionSystem.Set<EventActionSystem>(new(action.ThrowIfNull()));

            entity.AsEntity().AddRelation(BuiltInRelationType.OnModified, actionSystem);

            return entity;
        }

        /// <summary>
        /// Adds the on-remove <paramref name="action"/> to the component, creating a new entity containing the given <paramref name="action"/> as their <see cref="EventActionSystem"/>.
        /// </summary>
        public ComponentEntity OnRemove(EventActionSystem.EventAction action)
        {
            if (entity.Actions is null)
            {
                throw new InvalidOperationException("Cannot add on-remove action to an invalid entity.");
            }

            var actionSystem = entity.Actions.CreateEntity();
            actionSystem.Set<EventActionSystem>(new(action.ThrowIfNull()));

            entity.AsEntity().AddRelation(BuiltInRelationType.OnRemove, actionSystem);

            return entity;
        }
    }
}
