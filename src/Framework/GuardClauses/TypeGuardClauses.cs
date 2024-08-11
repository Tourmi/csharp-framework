using System.Runtime.CompilerServices;

namespace Tourmi.Framework.GuardClauses;

/// <summary>
/// Guard clauses on a <see cref="Type"/>
/// </summary>
public static class TypeGuardClauses
{
    /// <summary>
    /// Throws an <see cref="ArgumentException"/> if the type is abstract
    /// </summary>
    /// <returns>The original type</returns>
    /// <exception cref="ArgumentException"></exception>
    public static Type ThrowIfAbstract(this Type type, [CallerArgumentExpression(nameof(type))] string? paramName = null)
        => type.ThrowIfNull().IsAbstract ? throw new ArgumentException($"Given type {paramName} '{type}' cannot be abstract") : type;
}
