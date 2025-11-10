namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Extension methods for <see cref="World"/>
/// </summary>
public static class WorldExtensions
{
    /// <summary>
    /// Creates a new entity with the given <paramref name="name"/>.
    /// </summary>
    public static Entity CreateEntity(this World world, string name)
    {
        var entity = world.ThrowIfNull().CreateEntity();
        entity.Set(new Name() { Value = name });
        return entity;
    }

    /// <summary>
    /// Creates and returns a new prefab
    /// </summary>
    public static Entity CreatePrefab(this World world)
    {
        var entity = world.ThrowIfNull().CreateEntity();
        entity.Add<Prefab>();

        return entity;
    }
}
