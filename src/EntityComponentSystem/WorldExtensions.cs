namespace Tourmi.EntityComponentSystem;

/// <summary>
/// Extension methods for <see cref="World"/>
/// </summary>
public static class WorldExtensions
{
    extension(World world)
    {
        /// <summary>
        /// Attaches an existing entity to a type, 
        /// such as when the type <typeparamref name="T"/> is requested, 
        /// the given <paramref name="entity"/> will be filled in.
        /// </summary>
        public void AttachEntityToType<T>(Identifier entity) => world.AttachEntityToType(typeof(T), entity);
    }
}
