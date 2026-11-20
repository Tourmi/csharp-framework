namespace Tourmi.EntityComponentSystem.Ids;

/// <summary>
/// Extension methods relation to <see cref="IdentifierTypes"/>
/// </summary>
public static class IdentifierTypesExtensions
{
    extension(Identifier id)
    {
        /// <summary>
        /// Sets the given flags to 0 for the id.
        /// </summary>
        public Identifier EraseFlags(IdentifierTypes identifierType) => id & (~identifierType);
    }
}
