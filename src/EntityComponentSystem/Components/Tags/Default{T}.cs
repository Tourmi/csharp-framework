using System.Diagnostics.CodeAnalysis;

namespace Tourmi.EntityComponentSystem.Components.Tags;

/// <summary>
/// Marks an entity as the default value for <typeparamref name="T"/>.
/// </summary>
[TagComponent]
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Needed for terse queries.")]
public readonly record struct Default<T>;
