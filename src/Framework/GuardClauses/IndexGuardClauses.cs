using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses for an <see cref="Index"/>
/// </summary>
public static class IndexGuardClauses
{
    extension(Index argument)
    {
        /// <summary>
        /// Throws if the given index is out of bounds of the <paramref name="collection"/>.
        /// </summary>
        public Index ThrowIfOutOfBounds<T>(T collection, [CallerArgumentExpression(nameof(argument))] string? argumentName = null, [CallerArgumentExpression(nameof(collection))] string? collectionName = null)
            where T : ICollection
#if NET10_0_OR_GREATER
            , allows ref struct
#endif
        => ThrowIfOutOfBounds(argument, collection.ThrowIfNull().Count, argumentName, collectionName);

        /// <summary>
        /// Throws if the given index is out of bounds of the <paramref name="collectionSize"/>.
        /// </summary>
        public Index ThrowIfOutOfBounds(int collectionSize, [CallerArgumentExpression(nameof(argument))] string? argumentName = null, [CallerArgumentExpression(nameof(collectionSize))] string? collectionName = null)
        {
            var offset = argument.GetOffset(collectionSize);
            if (offset < 0 || offset >= collectionSize)
            {
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} was out of bounds of the collection {collectionName}.");
            }

            return argument;
        }
    }
}
