using System.Runtime.CompilerServices;
using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Extension methods allowing to execute functions from a query.
/// </summary>
public static class QueryForEachExtensions
{
    extension(Query query)
    {
        /// <summary>
        /// Executes the given <paramref name="action"/> for all entities returned by the query.
        /// </summary>
        public void ForEach(Action action)
        {
            foreach (var archetype in query.GetArchetypes())
            {
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action();
                }
            }
        }

        /// <summary>
        /// Returns the first value of type <typeparamref name="T"/>.
        /// </summary>
        public T First<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T>()
            where T : allows ref struct
        {
            var result = default(T);
            var callbacks = ParamCallbacks.For<T>();
            var globalCache = callbacks.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache = callbacks.GetArchetypeCache(query.World, archetype, globalCache);

                if (archetype.EntityCount == 0)
                {
                    callbacks.FreeArchetypeCache(archetypeCache, globalCache, query.World, archetype);
                    continue;
                }

                result = callbacks.CreateFrom(new(query.World, archetype, 0, globalCache, archetypeCache));
                callbacks.FreeArchetypeCache(archetypeCache, globalCache, query.World, archetype);
                break;
            }

            callbacks.FreeGlobalCache(globalCache, query.World);

            return result!;
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        public void ForEach<T>(QueryParamAction<T> action)
            where T : IQueryParam<T>, allows ref struct
        {
            var globalCache = T.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache = T.GetArchetypeCache(query.World, archetype, globalCache);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(T.CreateFrom(new(query.World, archetype, i, globalCache, archetypeCache)));
                }

                T.FreeArchetypeCache(archetypeCache, globalCache, query.World, archetype);
            }

            T.FreeGlobalCache(globalCache, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        public void ForEach<T1, T2>(QueryParamAction<T1, T2> action)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
        {
            var globalCache1 = T1.GetGlobalCache(query.World);
            var globalCache2 = T2.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = T1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = T2.GetArchetypeCache(query.World, archetype, globalCache2);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        T1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        T2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)));
                }

                T1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                T2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
            }

            T1.FreeGlobalCache(globalCache1, query.World);
            T2.FreeGlobalCache(globalCache2, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        public void ForEach<T1, T2, T3>(QueryParamAction<T1, T2, T3> action)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
        {
            var globalCache1 = T1.GetGlobalCache(query.World);
            var globalCache2 = T2.GetGlobalCache(query.World);
            var globalCache3 = T3.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = T1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = T2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = T3.GetArchetypeCache(query.World, archetype, globalCache3);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        T1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        T2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        T3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)));
                }

                T1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                T2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                T3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
            }

            T1.FreeGlobalCache(globalCache1, query.World);
            T2.FreeGlobalCache(globalCache2, query.World);
            T3.FreeGlobalCache(globalCache3, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        public void ForEach<T1, T2, T3, T4>(QueryParamAction<T1, T2, T3, T4> action)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
        {
            var globalCache1 = T1.GetGlobalCache(query.World);
            var globalCache2 = T2.GetGlobalCache(query.World);
            var globalCache3 = T3.GetGlobalCache(query.World);
            var globalCache4 = T4.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = T1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = T2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = T3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = T4.GetArchetypeCache(query.World, archetype, globalCache4);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        T1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        T2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        T3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        T4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)));
                }

                T1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                T2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                T3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                T4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
            }

            T1.FreeGlobalCache(globalCache1, query.World);
            T2.FreeGlobalCache(globalCache2, query.World);
            T3.FreeGlobalCache(globalCache3, query.World);
            T4.FreeGlobalCache(globalCache4, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        public void ForEach<T1, T2, T3, T4, T5>(QueryParamAction<T1, T2, T3, T4, T5> action)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
            where T5 : IQueryParam<T5>, allows ref struct
        {
            var globalCache1 = T1.GetGlobalCache(query.World);
            var globalCache2 = T2.GetGlobalCache(query.World);
            var globalCache3 = T3.GetGlobalCache(query.World);
            var globalCache4 = T4.GetGlobalCache(query.World);
            var globalCache5 = T5.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = T1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = T2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = T3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = T4.GetArchetypeCache(query.World, archetype, globalCache4);
                var archetypeCache5 = T5.GetArchetypeCache(query.World, archetype, globalCache5);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        T1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        T2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        T3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        T4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)),
                        T5.CreateFrom(new(query.World, archetype, i, globalCache5, archetypeCache5)));
                }

                T1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                T2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                T3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                T4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
                T5.FreeArchetypeCache(archetypeCache5, globalCache5, query.World, archetype);
            }

            T1.FreeGlobalCache(globalCache1, query.World);
            T2.FreeGlobalCache(globalCache2, query.World);
            T3.FreeGlobalCache(globalCache3, query.World);
            T4.FreeGlobalCache(globalCache4, query.World);
            T5.FreeGlobalCache(globalCache5, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        public void ForEach<T1, T2, T3, T4, T5, T6>(QueryParamAction<T1, T2, T3, T4, T5, T6> action)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
            where T5 : IQueryParam<T5>, allows ref struct
            where T6 : IQueryParam<T6>, allows ref struct
        {
            var globalCache1 = T1.GetGlobalCache(query.World);
            var globalCache2 = T2.GetGlobalCache(query.World);
            var globalCache3 = T3.GetGlobalCache(query.World);
            var globalCache4 = T4.GetGlobalCache(query.World);
            var globalCache5 = T5.GetGlobalCache(query.World);
            var globalCache6 = T6.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = T1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = T2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = T3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = T4.GetArchetypeCache(query.World, archetype, globalCache4);
                var archetypeCache5 = T5.GetArchetypeCache(query.World, archetype, globalCache5);
                var archetypeCache6 = T6.GetArchetypeCache(query.World, archetype, globalCache6);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        T1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        T2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        T3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        T4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)),
                        T5.CreateFrom(new(query.World, archetype, i, globalCache5, archetypeCache5)),
                        T6.CreateFrom(new(query.World, archetype, i, globalCache6, archetypeCache6)));
                }

                T1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                T2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                T3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                T4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
                T5.FreeArchetypeCache(archetypeCache5, globalCache5, query.World, archetype);
                T6.FreeArchetypeCache(archetypeCache6, globalCache6, query.World, archetype);
            }

            T1.FreeGlobalCache(globalCache1, query.World);
            T2.FreeGlobalCache(globalCache2, query.World);
            T3.FreeGlobalCache(globalCache3, query.World);
            T4.FreeGlobalCache(globalCache4, query.World);
            T5.FreeGlobalCache(globalCache5, query.World);
            T6.FreeGlobalCache(globalCache6, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        public void ForEach<T1, T2, T3, T4, T5, T6, T7>(QueryParamAction<T1, T2, T3, T4, T5, T6, T7> action)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
            where T5 : IQueryParam<T5>, allows ref struct
            where T6 : IQueryParam<T6>, allows ref struct
            where T7 : IQueryParam<T7>, allows ref struct
        {
            var globalCache1 = T1.GetGlobalCache(query.World);
            var globalCache2 = T2.GetGlobalCache(query.World);
            var globalCache3 = T3.GetGlobalCache(query.World);
            var globalCache4 = T4.GetGlobalCache(query.World);
            var globalCache5 = T5.GetGlobalCache(query.World);
            var globalCache6 = T6.GetGlobalCache(query.World);
            var globalCache7 = T7.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = T1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = T2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = T3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = T4.GetArchetypeCache(query.World, archetype, globalCache4);
                var archetypeCache5 = T5.GetArchetypeCache(query.World, archetype, globalCache5);
                var archetypeCache6 = T6.GetArchetypeCache(query.World, archetype, globalCache6);
                var archetypeCache7 = T7.GetArchetypeCache(query.World, archetype, globalCache7);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        T1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        T2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        T3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        T4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)),
                        T5.CreateFrom(new(query.World, archetype, i, globalCache5, archetypeCache5)),
                        T6.CreateFrom(new(query.World, archetype, i, globalCache6, archetypeCache6)),
                        T7.CreateFrom(new(query.World, archetype, i, globalCache7, archetypeCache7)));
                }

                T1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                T2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                T3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                T4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
                T5.FreeArchetypeCache(archetypeCache5, globalCache5, query.World, archetype);
                T6.FreeArchetypeCache(archetypeCache6, globalCache6, query.World, archetype);
                T7.FreeArchetypeCache(archetypeCache7, globalCache7, query.World, archetype);
            }

            T1.FreeGlobalCache(globalCache1, query.World);
            T2.FreeGlobalCache(globalCache2, query.World);
            T3.FreeGlobalCache(globalCache3, query.World);
            T4.FreeGlobalCache(globalCache4, query.World);
            T5.FreeGlobalCache(globalCache5, query.World);
            T6.FreeGlobalCache(globalCache6, query.World);
            T7.FreeGlobalCache(globalCache7, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        public void ForEach<T1, T2, T3, T4, T5, T6, T7, T8>(QueryParamAction<T1, T2, T3, T4, T5, T6, T7, T8> action)
            where T1 : IQueryParam<T1>, allows ref struct
            where T2 : IQueryParam<T2>, allows ref struct
            where T3 : IQueryParam<T3>, allows ref struct
            where T4 : IQueryParam<T4>, allows ref struct
            where T5 : IQueryParam<T5>, allows ref struct
            where T6 : IQueryParam<T6>, allows ref struct
            where T7 : IQueryParam<T7>, allows ref struct
            where T8 : IQueryParam<T8>, allows ref struct
        {
            var globalCache1 = T1.GetGlobalCache(query.World);
            var globalCache2 = T2.GetGlobalCache(query.World);
            var globalCache3 = T3.GetGlobalCache(query.World);
            var globalCache4 = T4.GetGlobalCache(query.World);
            var globalCache5 = T5.GetGlobalCache(query.World);
            var globalCache6 = T6.GetGlobalCache(query.World);
            var globalCache7 = T7.GetGlobalCache(query.World);
            var globalCache8 = T8.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = T1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = T2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = T3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = T4.GetArchetypeCache(query.World, archetype, globalCache4);
                var archetypeCache5 = T5.GetArchetypeCache(query.World, archetype, globalCache5);
                var archetypeCache6 = T6.GetArchetypeCache(query.World, archetype, globalCache6);
                var archetypeCache7 = T7.GetArchetypeCache(query.World, archetype, globalCache7);
                var archetypeCache8 = T8.GetArchetypeCache(query.World, archetype, globalCache8);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        T1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        T2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        T3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        T4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)),
                        T5.CreateFrom(new(query.World, archetype, i, globalCache5, archetypeCache5)),
                        T6.CreateFrom(new(query.World, archetype, i, globalCache6, archetypeCache6)),
                        T7.CreateFrom(new(query.World, archetype, i, globalCache7, archetypeCache7)),
                        T8.CreateFrom(new(query.World, archetype, i, globalCache8, archetypeCache8)));
                }

                T1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                T2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                T3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                T4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
                T5.FreeArchetypeCache(archetypeCache5, globalCache5, query.World, archetype);
                T6.FreeArchetypeCache(archetypeCache6, globalCache6, query.World, archetype);
                T7.FreeArchetypeCache(archetypeCache7, globalCache7, query.World, archetype);
                T8.FreeArchetypeCache(archetypeCache8, globalCache8, query.World, archetype);
            }

            T1.FreeGlobalCache(globalCache1, query.World);
            T2.FreeGlobalCache(globalCache2, query.World);
            T3.FreeGlobalCache(globalCache3, query.World);
            T4.FreeGlobalCache(globalCache4, query.World);
            T5.FreeGlobalCache(globalCache5, query.World);
            T6.FreeGlobalCache(globalCache6, query.World);
            T7.FreeGlobalCache(globalCache7, query.World);
            T8.FreeGlobalCache(globalCache8, query.World);
        }

        /// <summary>
        /// Executes the given <paramref name="action"/> for all entities returned by the query.
        /// </summary>
        [OverloadResolutionPriority(-1)]
        public void ForEach<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T>(Action<T> action)
            where T : allows ref struct
        {
            var callbacks = ParamCallbacks.For<T>();
            var globalCache = callbacks.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache = callbacks.GetArchetypeCache(query.World, archetype, globalCache);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(callbacks.CreateFrom(new(query.World, archetype, i, globalCache, archetypeCache)));
                }

                callbacks.FreeArchetypeCache(archetypeCache, globalCache, query.World, archetype);
            }

            callbacks.FreeGlobalCache(globalCache, query.World);
        }

        /// <summary>
        /// Executes the given <paramref name="action"/> for all entities returned by the query.
        /// </summary>
        [OverloadResolutionPriority(-1)]
        public void ForEach<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2>(Action<T1, T2> action)
            where T1 : allows ref struct
            where T2 : allows ref struct
        {
            var callbacks1 = ParamCallbacks.For<T1>();
            var callbacks2 = ParamCallbacks.For<T2>();
            var globalCache1 = callbacks1.GetGlobalCache(query.World);
            var globalCache2 = callbacks2.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = callbacks1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = callbacks2.GetArchetypeCache(query.World, archetype, globalCache2);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        callbacks1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        callbacks2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)));
                }

                callbacks1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                callbacks2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
            }

            callbacks1.FreeGlobalCache(globalCache1, query.World);
            callbacks2.FreeGlobalCache(globalCache2, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        [OverloadResolutionPriority(-1)]
        public void ForEach<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3>(Action<T1, T2, T3> action)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
        {
            var callbacks1 = ParamCallbacks.For<T1>();
            var callbacks2 = ParamCallbacks.For<T2>();
            var callbacks3 = ParamCallbacks.For<T3>();
            var globalCache1 = callbacks1.GetGlobalCache(query.World);
            var globalCache2 = callbacks2.GetGlobalCache(query.World);
            var globalCache3 = callbacks3.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = callbacks1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = callbacks2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = callbacks3.GetArchetypeCache(query.World, archetype, globalCache3);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        callbacks1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        callbacks2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        callbacks3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)));
                }

                callbacks1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                callbacks2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                callbacks3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
            }

            callbacks1.FreeGlobalCache(globalCache1, query.World);
            callbacks2.FreeGlobalCache(globalCache2, query.World);
            callbacks3.FreeGlobalCache(globalCache3, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        [OverloadResolutionPriority(-1)]
        public void ForEach<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4>(Action<T1, T2, T3, T4> action)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
        {
            var callbacks1 = ParamCallbacks.For<T1>();
            var callbacks2 = ParamCallbacks.For<T2>();
            var callbacks3 = ParamCallbacks.For<T3>();
            var callbacks4 = ParamCallbacks.For<T4>();
            var globalCache1 = callbacks1.GetGlobalCache(query.World);
            var globalCache2 = callbacks2.GetGlobalCache(query.World);
            var globalCache3 = callbacks3.GetGlobalCache(query.World);
            var globalCache4 = callbacks4.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = callbacks1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = callbacks2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = callbacks3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = callbacks4.GetArchetypeCache(query.World, archetype, globalCache4);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        callbacks1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        callbacks2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        callbacks3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        callbacks4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)));
                }

                callbacks1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                callbacks2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                callbacks3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                callbacks4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
            }

            callbacks1.FreeGlobalCache(globalCache1, query.World);
            callbacks2.FreeGlobalCache(globalCache2, query.World);
            callbacks3.FreeGlobalCache(globalCache3, query.World);
            callbacks4.FreeGlobalCache(globalCache4, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        [OverloadResolutionPriority(-1)]
        public void ForEach<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5>(Action<T1, T2, T3, T4, T5> action)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
            where T5 : allows ref struct
        {
            var callbacks1 = ParamCallbacks.For<T1>();
            var callbacks2 = ParamCallbacks.For<T2>();
            var callbacks3 = ParamCallbacks.For<T3>();
            var callbacks4 = ParamCallbacks.For<T4>();
            var callbacks5 = ParamCallbacks.For<T5>();
            var globalCache1 = callbacks1.GetGlobalCache(query.World);
            var globalCache2 = callbacks2.GetGlobalCache(query.World);
            var globalCache3 = callbacks3.GetGlobalCache(query.World);
            var globalCache4 = callbacks4.GetGlobalCache(query.World);
            var globalCache5 = callbacks5.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = callbacks1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = callbacks2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = callbacks3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = callbacks4.GetArchetypeCache(query.World, archetype, globalCache4);
                var archetypeCache5 = callbacks5.GetArchetypeCache(query.World, archetype, globalCache5);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        callbacks1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        callbacks2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        callbacks3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        callbacks4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)),
                        callbacks5.CreateFrom(new(query.World, archetype, i, globalCache5, archetypeCache5)));
                }

                callbacks1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                callbacks2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                callbacks3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                callbacks4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
                callbacks5.FreeArchetypeCache(archetypeCache5, globalCache5, query.World, archetype);
            }

            callbacks1.FreeGlobalCache(globalCache1, query.World);
            callbacks2.FreeGlobalCache(globalCache2, query.World);
            callbacks3.FreeGlobalCache(globalCache3, query.World);
            callbacks4.FreeGlobalCache(globalCache4, query.World);
            callbacks5.FreeGlobalCache(globalCache5, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        [OverloadResolutionPriority(-1)]
        public void ForEach<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6>(Action<T1, T2, T3, T4, T5, T6> action)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
            where T5 : allows ref struct
            where T6 : allows ref struct
        {
            var callbacks1 = ParamCallbacks.For<T1>();
            var callbacks2 = ParamCallbacks.For<T2>();
            var callbacks3 = ParamCallbacks.For<T3>();
            var callbacks4 = ParamCallbacks.For<T4>();
            var callbacks5 = ParamCallbacks.For<T5>();
            var callbacks6 = ParamCallbacks.For<T6>();
            var globalCache1 = callbacks1.GetGlobalCache(query.World);
            var globalCache2 = callbacks2.GetGlobalCache(query.World);
            var globalCache3 = callbacks3.GetGlobalCache(query.World);
            var globalCache4 = callbacks4.GetGlobalCache(query.World);
            var globalCache5 = callbacks5.GetGlobalCache(query.World);
            var globalCache6 = callbacks6.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = callbacks1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = callbacks2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = callbacks3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = callbacks4.GetArchetypeCache(query.World, archetype, globalCache4);
                var archetypeCache5 = callbacks5.GetArchetypeCache(query.World, archetype, globalCache5);
                var archetypeCache6 = callbacks6.GetArchetypeCache(query.World, archetype, globalCache6);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        callbacks1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        callbacks2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        callbacks3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        callbacks4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)),
                        callbacks5.CreateFrom(new(query.World, archetype, i, globalCache5, archetypeCache5)),
                        callbacks6.CreateFrom(new(query.World, archetype, i, globalCache6, archetypeCache6)));
                }

                callbacks1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                callbacks2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                callbacks3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                callbacks4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
                callbacks5.FreeArchetypeCache(archetypeCache5, globalCache5, query.World, archetype);
                callbacks6.FreeArchetypeCache(archetypeCache6, globalCache6, query.World, archetype);
            }

            callbacks1.FreeGlobalCache(globalCache1, query.World);
            callbacks2.FreeGlobalCache(globalCache2, query.World);
            callbacks3.FreeGlobalCache(globalCache3, query.World);
            callbacks4.FreeGlobalCache(globalCache4, query.World);
            callbacks5.FreeGlobalCache(globalCache5, query.World);
            callbacks6.FreeGlobalCache(globalCache6, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        [OverloadResolutionPriority(-1)]
        public void ForEach<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T7>(Action<T1, T2, T3, T4, T5, T6, T7> action)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
            where T5 : allows ref struct
            where T6 : allows ref struct
            where T7 : allows ref struct
        {
            var callbacks1 = ParamCallbacks.For<T1>();
            var callbacks2 = ParamCallbacks.For<T2>();
            var callbacks3 = ParamCallbacks.For<T3>();
            var callbacks4 = ParamCallbacks.For<T4>();
            var callbacks5 = ParamCallbacks.For<T5>();
            var callbacks6 = ParamCallbacks.For<T6>();
            var callbacks7 = ParamCallbacks.For<T7>();
            var globalCache1 = callbacks1.GetGlobalCache(query.World);
            var globalCache2 = callbacks2.GetGlobalCache(query.World);
            var globalCache3 = callbacks3.GetGlobalCache(query.World);
            var globalCache4 = callbacks4.GetGlobalCache(query.World);
            var globalCache5 = callbacks5.GetGlobalCache(query.World);
            var globalCache6 = callbacks6.GetGlobalCache(query.World);
            var globalCache7 = callbacks7.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = callbacks1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = callbacks2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = callbacks3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = callbacks4.GetArchetypeCache(query.World, archetype, globalCache4);
                var archetypeCache5 = callbacks5.GetArchetypeCache(query.World, archetype, globalCache5);
                var archetypeCache6 = callbacks6.GetArchetypeCache(query.World, archetype, globalCache6);
                var archetypeCache7 = callbacks7.GetArchetypeCache(query.World, archetype, globalCache7);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        callbacks1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        callbacks2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        callbacks3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        callbacks4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)),
                        callbacks5.CreateFrom(new(query.World, archetype, i, globalCache5, archetypeCache5)),
                        callbacks6.CreateFrom(new(query.World, archetype, i, globalCache6, archetypeCache6)),
                        callbacks7.CreateFrom(new(query.World, archetype, i, globalCache7, archetypeCache7)));
                }

                callbacks1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                callbacks2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                callbacks3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                callbacks4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
                callbacks5.FreeArchetypeCache(archetypeCache5, globalCache5, query.World, archetype);
                callbacks6.FreeArchetypeCache(archetypeCache6, globalCache6, query.World, archetype);
                callbacks7.FreeArchetypeCache(archetypeCache7, globalCache7, query.World, archetype);
            }

            callbacks1.FreeGlobalCache(globalCache1, query.World);
            callbacks2.FreeGlobalCache(globalCache2, query.World);
            callbacks3.FreeGlobalCache(globalCache3, query.World);
            callbacks4.FreeGlobalCache(globalCache4, query.World);
            callbacks5.FreeGlobalCache(globalCache5, query.World);
            callbacks6.FreeGlobalCache(globalCache6, query.World);
            callbacks7.FreeGlobalCache(globalCache7, query.World);
        }

        /// <inheritdoc cref="ForEach(Query, Action)"/>
        [OverloadResolutionPriority(-1)]
        public void ForEach<
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T1,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T2,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T3,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T4,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T5,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T6,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T7,
            [DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> action)
            where T1 : allows ref struct
            where T2 : allows ref struct
            where T3 : allows ref struct
            where T4 : allows ref struct
            where T5 : allows ref struct
            where T6 : allows ref struct
            where T7 : allows ref struct
            where T8 : allows ref struct
        {
            var callbacks1 = ParamCallbacks.For<T1>();
            var callbacks2 = ParamCallbacks.For<T2>();
            var callbacks3 = ParamCallbacks.For<T3>();
            var callbacks4 = ParamCallbacks.For<T4>();
            var callbacks5 = ParamCallbacks.For<T5>();
            var callbacks6 = ParamCallbacks.For<T6>();
            var callbacks7 = ParamCallbacks.For<T7>();
            var callbacks8 = ParamCallbacks.For<T8>();
            var globalCache1 = callbacks1.GetGlobalCache(query.World);
            var globalCache2 = callbacks2.GetGlobalCache(query.World);
            var globalCache3 = callbacks3.GetGlobalCache(query.World);
            var globalCache4 = callbacks4.GetGlobalCache(query.World);
            var globalCache5 = callbacks5.GetGlobalCache(query.World);
            var globalCache6 = callbacks6.GetGlobalCache(query.World);
            var globalCache7 = callbacks7.GetGlobalCache(query.World);
            var globalCache8 = callbacks8.GetGlobalCache(query.World);

            foreach (var archetype in query.GetArchetypes())
            {
                var archetypeCache1 = callbacks1.GetArchetypeCache(query.World, archetype, globalCache1);
                var archetypeCache2 = callbacks2.GetArchetypeCache(query.World, archetype, globalCache2);
                var archetypeCache3 = callbacks3.GetArchetypeCache(query.World, archetype, globalCache3);
                var archetypeCache4 = callbacks4.GetArchetypeCache(query.World, archetype, globalCache4);
                var archetypeCache5 = callbacks5.GetArchetypeCache(query.World, archetype, globalCache5);
                var archetypeCache6 = callbacks6.GetArchetypeCache(query.World, archetype, globalCache6);
                var archetypeCache7 = callbacks7.GetArchetypeCache(query.World, archetype, globalCache7);
                var archetypeCache8 = callbacks8.GetArchetypeCache(query.World, archetype, globalCache8);
                for (var i = 0; i < archetype.EntityCount; i++)
                {
                    action(
                        callbacks1.CreateFrom(new(query.World, archetype, i, globalCache1, archetypeCache1)),
                        callbacks2.CreateFrom(new(query.World, archetype, i, globalCache2, archetypeCache2)),
                        callbacks3.CreateFrom(new(query.World, archetype, i, globalCache3, archetypeCache3)),
                        callbacks4.CreateFrom(new(query.World, archetype, i, globalCache4, archetypeCache4)),
                        callbacks5.CreateFrom(new(query.World, archetype, i, globalCache5, archetypeCache5)),
                        callbacks6.CreateFrom(new(query.World, archetype, i, globalCache6, archetypeCache6)),
                        callbacks7.CreateFrom(new(query.World, archetype, i, globalCache7, archetypeCache7)),
                        callbacks8.CreateFrom(new(query.World, archetype, i, globalCache8, archetypeCache8)));
                }

                callbacks1.FreeArchetypeCache(archetypeCache1, globalCache1, query.World, archetype);
                callbacks2.FreeArchetypeCache(archetypeCache2, globalCache2, query.World, archetype);
                callbacks3.FreeArchetypeCache(archetypeCache3, globalCache2, query.World, archetype);
                callbacks4.FreeArchetypeCache(archetypeCache4, globalCache4, query.World, archetype);
                callbacks5.FreeArchetypeCache(archetypeCache5, globalCache5, query.World, archetype);
                callbacks6.FreeArchetypeCache(archetypeCache6, globalCache6, query.World, archetype);
                callbacks7.FreeArchetypeCache(archetypeCache7, globalCache7, query.World, archetype);
                callbacks8.FreeArchetypeCache(archetypeCache8, globalCache8, query.World, archetype);
            }

            callbacks1.FreeGlobalCache(globalCache1, query.World);
            callbacks2.FreeGlobalCache(globalCache2, query.World);
            callbacks3.FreeGlobalCache(globalCache3, query.World);
            callbacks4.FreeGlobalCache(globalCache4, query.World);
            callbacks5.FreeGlobalCache(globalCache5, query.World);
            callbacks6.FreeGlobalCache(globalCache6, query.World);
            callbacks7.FreeGlobalCache(globalCache7, query.World);
            callbacks8.FreeGlobalCache(globalCache8, query.World);
        }
    }
}
