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
        /// Attaches an existing entity to a type, 
        /// such as when the type <typeparamref name="T"/> is requested, 
        /// the given <paramref name="entity"/> will be filled in.
        /// </summary>
        public void AttachEntityToType<T>(Identifier entity) => world.AttachEntityToType(typeof(T), entity);
    }
}
