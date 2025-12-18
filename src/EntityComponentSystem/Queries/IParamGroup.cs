namespace Tourmi.EntityComponentSystem.Queries;

/// <summary>
/// Interface marking a type as a <see cref="ParamGroup"/>
/// </summary>
public interface IParamGroup
{
    /// <summary>
    /// Amount of parameters the group contains.
    /// </summary>
    int Count { get; }
}
