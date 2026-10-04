namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Extension methods for <see cref="World"/>
/// </summary>
public static class WorldExtensions
{
    extension(World world)
    {
        /// <summary>
        /// Raises the <typeparamref name="TEvent"/> event, updating all systems or schedules subscribed to it.
        /// </summary>
        [SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Used externally, not a C# event")]
        public void Raise<TEvent>() => world.Raise(world.GetEntityForType<TEvent>());

        /// <summary>
        /// Raises the event represented by the given <paramref name="eventId"/>, updating all systems or schedules subscribed to it.
        /// </summary>
        [SuppressMessage("Design", "CA1030:Use events where appropriate", Justification = "Used externally, not a C# event")]
        public void Raise(Identifier eventId) => world
            .ThrowIfNull()
            .GetCachedQueryFor<ParamGroup<Relation<SubscribedTo, Parameter1>>>(eventId)
            .ForEach((Identifier id, Optional<SystemComponent> system, OptionalRef<Schedule> scheduleRef) =>
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

                        schedule.CurrentTick = 0;
                        world.Raise(id);
                    }
                }
                else if (system.HasValue)
                {
                    system.Value.Execute();
                }

                world.RunQueuedActions();
            });

        /// <summary>
        /// Sends out the <see cref="Events.Tick"/> event, along with its <see cref="Events.PreTick"/> and <see cref="Events.PostTick"/> events, updating schedules and systems subscribed to them.
        /// </summary>
        public void Tick()
        {
            world.Raise<Events.PreTick>();
            world.Raise<Events.Tick>();
            world.Raise<Events.PostTick>();
        }

        /// <summary>
        /// Attaches an existing entity to a type, 
        /// such as when the type <typeparamref name="T"/> is requested, 
        /// the given <paramref name="entity"/> will be filled in.
        /// </summary>
        public void AttachEntityToType<T>(Identifier entity) => world.AttachEntityToType(typeof(T), entity);
    }
}
