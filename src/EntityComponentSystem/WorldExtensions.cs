namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Extension methods for <see cref="World"/>
/// </summary>
public static class WorldExtensions
{
    extension(World world)
    {
        /// <summary>
        /// Sends out the Tick event, allowing schedules and systems subscribed to it to run.
        /// </summary>
        public void Tick() => world.GetCachedQueryFor<ParamGroup<Relation<SubscribedTo, Events.Tick>>>().ForEach((Identifier id, Optional<SystemComponent> system, OptionalRef<Schedule> scheduleRef) =>
        {
            // If entity has
            if (scheduleRef.HasValue)
            {
                ref var schedule = ref scheduleRef.Reference;
                // TODO: Check DeltaTime, and compare with current schedule time.

                schedule.CurrentTick++;
                if (schedule.CurrentTick >= schedule.TickRate)
                {
                    if (system.HasValue)
                    {
                        system.Value.Execute();
                    }

                    // TODO: Tick all subscribers.

                    schedule.CurrentTick = 0;
                }
            }
            else if (system.HasValue)
            {
                system.Value.Execute();
            }
        });

        /// <summary>
        /// Creates a new entity with the given <paramref name="name"/>.
        /// </summary>
        public Entity CreateEntity(string name)
        {
            var entity = world.ThrowIfNull().CreateEntity();
            entity.Set(new Name() { Value = name });
            return entity;
        }

        /// <summary>
        /// Returns <see langword="true"/> if the entity is alive and contains the component of type <typeparamref name="T"/>, <see langword="false"/> otherwise.
        /// </summary>
        public bool Has<T>(Identifier entity) => world.ThrowIfNull().Has(entity, world.GetComponentForType<T>());

        /// <summary>
        /// Returns the component of type <typeparamref name="T"/> for the <paramref name="entity"/>.
        /// </summary>
        /// <returns>The component for the <paramref name="entity"/>, or <see langword="default"/> if the component is missing.</returns>
        public T? Get<T>(Identifier entity) => world.ThrowIfNull().Get<T>(entity, world.GetComponentForType<T>());

        /// <summary>
        /// Returns a mutable reference of the component of type <typeparamref name="T"/> for the <paramref name="entity"/>.
        /// </summary>
        public ref T? GetMutable<T>(Identifier entity) => ref world.ThrowIfNull().GetMutable<T>(entity, world.GetComponentForType<T>());

        /// <summary>
        /// Returns the component of type <typeparamref name="T"/> for the <paramref name="entity"/>.
        /// </summary>
        /// <returns>The component for the <paramref name="entity"/>, or <see langword="null"/> if the component is missing.</returns>
        public T? Ensure<T>(Identifier entity) => world.ThrowIfNull().Ensure<T>(entity, world.GetComponentForType<T>());

        /// <summary>
        /// Returns a mutable reference of the component of type <typeparamref name="T"/> for the <paramref name="entity"/>.
        /// </summary>
        public ref T? EnsureMutable<T>(Identifier entity) => ref world.ThrowIfNull().EnsureMutable<T>(entity, world.GetComponentForType<T>());

        /// <summary>
        /// Adds the component of type <typeparamref name="T"/> to the <paramref name="entity"/>
        /// </summary>
        public void Add<T>(Identifier entity) => world.ThrowIfNull().Add(entity, world.GetComponentForType<T>());

        /// <summary>
        /// Sets the component's value for the <paramref name="entity"/> to the given <paramref name="value"/>
        /// </summary>
        public void Set<T>(Identifier entity, T value) => world.ThrowIfNull().Set(entity, world.GetComponentForType<T>(), value);

        /// <summary>
        /// Removes the component of type <typeparamref name="T"/> from the <paramref name="entity"/>
        /// </summary>
        public void Remove<T>(Identifier entity) => world.ThrowIfNull().Remove(entity, world.GetComponentForType<T>());
    }
}
