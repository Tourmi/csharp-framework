using System.Runtime.CompilerServices;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Extension methods for <see cref="World"/>, relating to creating Systems.
/// </summary>
public static class WorldSystemExtensions
{
    extension(World world)
    {
        /// <summary>
        /// Creates a new system entity that is activated on each Tick, with the given <paramref name="action"/>.
        /// </summary>
        public SystemEntity CreateTickSystem(Action action) => world.CreateEventSystem<Events.Tick>(action);

        /// <summary>
        /// Creates a new system that is subscribed to the <typeparamref name="TEvent"/> event.
        /// </summary>
        public SystemEntity CreateEventSystem<TEvent>(Action action)
        {
            var systemEntity = world.CreateEntity();
            systemEntity.Set<SystemComponent>(new(action));
            systemEntity.SubscribedTo<TEvent>();

            return (SystemEntity)systemEntity;
        }

        /// <summary>
        /// Creates a new system that is subscribed to the <paramref name="eventId"/>.
        /// </summary>
        public SystemEntity CreateEventSystem(Identifier eventId, Action action)
        {
            var systemEntity = world.CreateEntity();
            systemEntity.Set<SystemComponent>(new(action));
            systemEntity.Add(new RelationComponentIdentifier(eventId.ShortId, BuiltInRelationType.SubscribedTo));

            return (SystemEntity)systemEntity;
        }

        /// <summary>
        /// Adds a system to the default schedule.
        /// </summary>
        public SystemEntity AddSystem<T1>(QueryParamAction<T1> system)
            where T1 : IQueryParam<T1>, allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<T1>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2>(QueryParamAction<T1, T2> system)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3>(QueryParamAction<T1, T2, T3> system)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4>(QueryParamAction<T1, T2, T3, T4> system)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5>(QueryParamAction<T1, T2, T3, T4, T5> system)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
            where T5 : IQueryParam<T5>, allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4, T5>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6>(QueryParamAction<T1, T2, T3, T4, T5, T6> system)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
            where T5 : IQueryParam<T5>, allows ref struct
            where T6 : IQueryParam<T6>, allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4, T5, T6>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T7>(QueryParamAction<T1, T2, T3, T4, T5, T6, T7> system)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
            where T5 : IQueryParam<T5>, allows ref struct
            where T6 : IQueryParam<T6>, allows ref struct
            where T7 : IQueryParam<T7>, allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T7,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T8>(QueryParamAction<T1, T2, T3, T4, T5, T6, T7, T8> system)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
            where T5 : IQueryParam<T5>, allows ref struct
            where T6 : IQueryParam<T6>, allows ref struct
            where T7 : IQueryParam<T7>, allows ref struct
            where T8 : IQueryParam<T8>, allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        [OverloadResolutionPriority(-1)]
        public SystemEntity AddSystem<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1>(Action<T1> system)
            where T1 : allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }


        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        [OverloadResolutionPriority(-1)]
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2>(Action<T1, T2> system)
            where T1 : allows ref struct
            where T2 : allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        [OverloadResolutionPriority(-1)]
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3>(Action<T1, T2, T3> system)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        [OverloadResolutionPriority(-1)]
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4>(Action<T1, T2, T3, T4> system)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        [OverloadResolutionPriority(-1)]
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5>(Action<T1, T2, T3, T4, T5> system)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
            where T5 : allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4, T5>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        [OverloadResolutionPriority(-1)]
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6>(Action<T1, T2, T3, T4, T5, T6> system)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
            where T5 : allows ref struct
            where T6 : allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4, T5, T6>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        [OverloadResolutionPriority(-1)]
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T7>(Action<T1, T2, T3, T4, T5, T6, T7> system)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
            where T5 : allows ref struct
            where T6 : allows ref struct
            where T7 : allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4, T5, T6, T7>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }

        /// <inheritdoc cref="AddSystem{T1}(World, QueryParamAction{T1})"/>
        [OverloadResolutionPriority(-1)]
        public SystemEntity AddSystem<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T7,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> system)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
            where T5 : allows ref struct
            where T6 : allows ref struct
            where T7 : allows ref struct
            where T8 : allows ref struct
        {
            _ = system.ThrowIfNull();

            var query = world.GetCachedQueryFor<ParamGroup<T1, T2, T3, T4, T5, T6, T7, T8>>();

            return world.CreateTickSystem(() => query.ForEach(system));
        }
    }
}
