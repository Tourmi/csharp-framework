namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Extension methods for <see cref="Entity"/> that allow adding relations.
/// </summary>
public static class EntityRelationExtensions
{
    extension(Entity entity)
    {
        /// <summary>
        /// Adds the given <typeparamref name="TRelation"/> with the <typeparamref name="TTarget"/> to the entity.
        /// </summary>
        public void AddRelation<TRelation, TTarget>() => entity.Actions?.Add<Relation<TRelation, TTarget>>(entity.Id);

        /// <summary>
        /// Adds the given <paramref name="relationType"/> with the given <paramref name="targetEntity"/> to the entity.
        /// </summary>
        public void AddRelation(BuiltInRelationType relationType, Identifier targetEntity) => entity.Actions?.Add(entity.Id, new RelationComponentIdentifier(targetEntity.ShortId, relationType));

        /// <summary>
        /// The entity is a <typeparamref name="TTarget"/>.
        /// </summary>
        public void IsA<TTarget>() => entity.AddRelation<IsA, TTarget>();

        /// <summary>
        /// The entity is a <paramref name="baseEntity"/>.
        /// </summary>
        public void IsA(Identifier baseEntity) => entity.AddRelation(BuiltInRelationType.IsA, baseEntity);

        /// <summary>
        /// The entity depends on <typeparamref name="TTarget"/>
        /// </summary>
        public void DependsOn<TTarget>() => entity.AddRelation<DependsOn, TTarget>();

        /// <summary>
        /// The entity depends on the given <paramref name="dependency"/>.
        /// </summary>
        public void DependsOn(Identifier dependency) => entity.AddRelation(BuiltInRelationType.DependsOn, dependency);

        /// <summary>
        /// The entity is a child of <typeparamref name="TTarget"/>
        /// </summary>
        public void ChildOf<TTarget>() => entity.AddRelation<ChildOf, TTarget>();

        /// <summary>
        /// The entity is a child of <paramref name="parent"/>
        /// </summary>
        public void ChildOf(Identifier parent) => entity.AddRelation(BuiltInRelationType.ChildOf, parent);

        /// <summary>
        /// The entity is an instance of <typeparamref name="TTarget"/>
        /// </summary>
        public void InstanceOf<TTarget>() => entity.AddRelation<InstanceOf, TTarget>();

        /// <summary>
        /// The entity is an instance of the given <paramref name="baseEntity"/>
        /// </summary>
        public void InstanceOf(Identifier baseEntity) => entity.AddRelation(BuiltInRelationType.InstanceOf, baseEntity);

        /// <summary>
        /// The entity requires <typeparamref name="TTarget"/>
        /// </summary>
        public void Requires<TTarget>(
            Requires.DependencyMissingBehavior MissingBehavior = default,
            Requires.DependencyRemovedBehavior RemovedBehavior = default)
            => entity.Actions?.Set(
                entity,
                new RelationComponentIdentifier(entity.Actions.GetEntityForType<TTarget>().Id.ShortId, BuiltInRelationType.Requires),
                new Requires(MissingBehavior, RemovedBehavior));

        /// <summary>
        /// The entity requires the <paramref name="target"/>
        /// </summary>
        public void Requires(
            Identifier target,
            Requires.DependencyMissingBehavior MissingBehavior = default,
            Requires.DependencyRemovedBehavior RemovedBehavior = default)
            => entity.Actions?.Set(
                entity,
                new RelationComponentIdentifier(target.ShortId, BuiltInRelationType.Requires),
                new Requires(MissingBehavior, RemovedBehavior));

        /// <summary>
        /// The entity is subscribed to <typeparamref name="TTarget"/>
        /// </summary>
        public void SubscribedTo<TTarget>() => entity.AddRelation<SubscribedTo, TTarget>();

        /// <summary>
        /// The entity is subscribed to the <paramref name="target"/>
        /// </summary>
        public void SubscribedTo(Identifier target) => entity.AddRelation(BuiltInRelationType.SubscribedTo, target);
    }
}
