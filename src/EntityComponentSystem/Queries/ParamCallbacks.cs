using System.Diagnostics.CodeAnalysis;

using static System.Diagnostics.CodeAnalysis.DynamicallyAccessedMemberTypes;

namespace Tourmi.EntityComponentSystem.Queries;

/// <inheritdoc cref="ParamCallbacks{T}"/>
internal static class ParamCallbacks
{
    /// <summary>
    /// Returns the callbacks for parameter <typeparamref name="T"/>
    /// </summary>
    public static ParamCallbacks<T> For<[DynamicallyAccessedMembers(Interfaces | PublicMethods | NonPublicMethods)] T>()
        where T : allows ref struct
        => ParamCallbacks<T>.Instance;
}
