namespace Tourmi.EntityComponentSystem.Queries.Parameters.Overrides;

internal class IdentifierOverride : IQueryParamOverride<Identifier>
{
    static Identifier IQueryParamOverride<Identifier>.CreateFrom(QueryParamEntityInfo entry)
        => new(entry.Archetype.Entities[entry.EntityIndex]);
}
