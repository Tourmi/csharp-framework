namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Interface marking a type as a <see cref="ParamGroup"/>
/// </summary>
internal interface IParamGroup
{
    /// <summary>
    /// Amount of parameters the group contains.
    /// </summary>
    int Count { get; }
}
