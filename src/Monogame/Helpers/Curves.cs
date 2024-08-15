using Tourmi.Monogame.Extensions;

namespace Tourmi.Monogame.Helpers;

/// <summary>
/// Helper methods for <see cref="Curve"/> and <see cref="CurveKey"/>
/// </summary>
public static class Curves
{
    /// <summary>
    /// Builds a curve based on the given curve keys, using <see cref="CurveLoopType.Constant"/> for both Pre and PostLoop options
    /// </summary>
    public static Curve BuildCurve(params CurveKey[] curveKeys) => BuildCurve(curveKeys, CurveLoopType.Constant, CurveLoopType.Constant);

    /// <summary>
    /// Builds a curve based on the given curve keys, preloop and postloop options
    /// </summary>
    public static Curve BuildCurve(
        IEnumerable<CurveKey> curveKeys,
        CurveLoopType preLoop = CurveLoopType.Constant,
        CurveLoopType postLoop = CurveLoopType.Constant)
    {
        _ = curveKeys.ThrowIfNull();

        var res = new Curve()
        {
            PreLoop = CurveLoopType.Constant,
            PostLoop = CurveLoopType.Constant
        };

        foreach (var key in curveKeys)
        {
            res.Keys.Add(key);
        }

        return res;
    }

    /// <summary>
    /// A linear curve
    /// </summary>
    public static Curve Linear() => BuildCurve(
        new CurveKey(0, 0, -1, 1),
        new CurveKey(1, 1, 1, -1));

    /// <summary>
    /// Ease in and out
    /// </summary>
    public static Curve EaseInOut() => BuildCurve(
        new CurveKey(0, 0, 0, 0),
        new CurveKey(0.5f, 0.5f, 1, 1),
        new CurveKey(1, 1, 0, 0));

    /// <summary>
    /// Ease in curve
    /// </summary>
    public static Curve EaseIn() => BuildCurve(
        new CurveKey(0, 0, 0, 0),
        new CurveKey(1, 1, 1, 1));

    /// <summary>
    /// Ease out curve
    /// </summary>
    public static Curve EaseOut() => BuildCurve(
        new CurveKey(0, 0, 1, 1),
        new CurveKey(1, 1, 0, 0));

    /// <summary>
    /// A looping ease int and out curve
    /// </summary>
    public static Curve LoopEaseInOut() => EaseInOut().Append(EaseInOut().Negate());
    /// <summary>
    /// A looping linear curve
    /// </summary>
    public static Curve LoopLinear() => Linear().Append(Linear().Negate());
}
