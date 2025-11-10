namespace Tourmi.EntityComponentSystem.Entities;

/// <summary>
/// Extension methods for <see cref="Entity"/>
/// </summary>
public static class EntityExtensions
{
    /// <summary>
    /// Adds the component of type <typeparamref name="TComponent"/> to the entity, instantiating it with the parameterless constructor.
    /// Does nothing if the entity already has the component.
    /// </summary>
    public static void Add<TComponent>(in this Entity entity)
        where TComponent : new()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds the component with <paramref name="componentId"/> to the entity, instantiating it with the parameterless constructor.
    /// Does nothing if the entity already has the component.
    /// </summary>
    public static void Add<TComponent>(in this Entity entity, Identifier componentId)
        where TComponent : new()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds the component with <paramref name="componentId"/> to the entity.
    /// Does nothing if the entity already has the component.
    /// </summary>
    /// <remarks>
    /// This overload should be used if the component does not have a value. ie: for flags or states
    /// </remarks>
    public static void Add(in this Entity entity, Identifier componentId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes the <typeparamref name="TComponent"/> from the entity.
    /// </summary>
    public static void Remove<TComponent>(in this Entity entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes the component with <paramref name="componentId"/> from the entity.
    /// </summary>
    public static void Remove(in this Entity entity, Identifier componentId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a readonly copy of the <typeparamref name="TComponent"/> from the <paramref name="entity"/>.
    /// Will throw if the entity does not contain the <typeparamref name="TComponent"/>.
    /// </summary>
    public static TComponent Get<TComponent>(in this Entity entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns a readonly copy of the <typeparamref name="TComponent"/> with <paramref name="componentId"/> from the <paramref name="entity"/>.
    /// Will throw if the entity does not contain a component with the given <paramref name="componentId"/>.
    /// </summary>
    public static TComponent Get<TComponent>(in this Entity entity, Identifier componentId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <typeparamref name="TComponent"/> from the <paramref name="entity"/>.
    /// Will throw if the entity does not contain the <typeparamref name="TComponent"/>.
    /// </summary>
    public static ref TComponent GetMutable<TComponent>(in this Entity entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the <typeparamref name="TComponent"/> with <paramref name="componentId"/> from the <paramref name="entity"/>.
    /// Will throw if the entity does not contain a component with the given <paramref name="componentId"/>.
    /// </summary>
    public static ref TComponent GetMutable<TComponent>(in this Entity entity, Identifier componentId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds the <typeparamref name="TComponent"/> to the <paramref name="entity"/> if it doesn't contain it,
    /// then returns a readonly copy of the component.
    /// </summary>
    public static TComponent Ensure<TComponent>(in this Entity entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Adds the <typeparamref name="TComponent"/> to the <paramref name="entity"/> if it doesn't contain it,
    /// then returns the component.
    /// </summary>
    public static ref TComponent EnsureMutable<TComponent>(in this Entity entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns <see langword="true"/> if the entity contains the given component.
    /// </summary>
    public static bool Has<TComponent>(in this Entity entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns <see langword="true"/> if the entity contains the given component with <paramref name="componentId"/>.
    /// </summary>
    public static bool Has(in this Entity entity, Identifier componentId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sets the component for the entity, adding it if the entity did not contain it.
    /// </summary>
    public static void Set<TComponent>(in this Entity entity, TComponent component)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sets the component with <paramref name="componentId"/> for the entity, adding it if the entity did not contain it.
    /// </summary>
    public static void Set<TComponent>(in this Entity entity, Identifier componentId, TComponent component)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Marks the <typeparamref name="TComponent"/> as modified for the entity, triggering any OnChanged systems.
    /// </summary>
    public static void Modified<TComponent>(in this Entity entity)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Marks the component with <paramref name="componentId"/> as modified for the entity, triggering any OnChanged systems.
    /// </summary>
    public static void Modified(in this Entity entity, Identifier componentId)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns <see langword="true"/> if the entity is alive.
    /// </summary>
    public static bool IsAlive(in this Entity entity)
    {
        if (entity.World is null)
        {
            return false;
        }

        throw new NotImplementedException();
    }

    /// <summary>
    /// Kills the <paramref name="entity"/>, deleting its components.
    /// </summary>
    public static void Kill(in this Entity entity)
    {
        if (entity.World is null)
        {
            return;
        }

        throw new NotImplementedException();
    }
}
