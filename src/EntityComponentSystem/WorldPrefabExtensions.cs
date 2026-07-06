namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Extension methods for <see cref="World"/>, relating to creating Systems.
/// </summary>
public static class WorldPrefabExtensions
{
    extension(World world)
    {
        /// <summary>
        /// Creates and returns a new prefab
        /// </summary>
        public PrefabEntity CreatePrefab()
        {
            var entity = world.ThrowIfNull().CreateEntity();
            entity.Add<Prefab>();

            return new PrefabEntity(entity);
        }
    }
}
