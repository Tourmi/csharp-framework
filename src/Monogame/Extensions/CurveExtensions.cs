using Tourmi.Monogame.Helpers;

namespace Tourmi.Monogame.Extensions;

/// <summary>
/// Extension methods for <see cref="Curve"/>
/// </summary>
public static class CurveExtensions
{
    /// <summary>
    /// Appends all of the given curves together, dividing the length of each by the amount of curves given. 
    /// Assumes that every curve starts from position 0, but they can be of any length.
    /// 
    /// Preserves <paramref name="c1"/>'s PreLoop and PostLoop values
    /// </summary>
    public static Curve Append(this Curve c1, params Curve[] otherCurves)
    {
        _ = c1.ThrowIfNull();
        _ = otherCurves.ThrowIfNull();

        var keys = new List<CurveKey>();
        var curves = new Curve[otherCurves.Length + 1];
        curves[0] = c1;
        Array.Copy(otherCurves, 0, curves, 1, otherCurves.Length);

        var totalLength = curves.Sum(c => c.LastKey().Position);

        float currLength = 0;
        for (var i = 0; i < curves.Length; i++)
        {
            var isNotFirstCurve = i > 0;
            var isNotLastCurve = i < curves.Length - 1;

            var j = 0;
            foreach (var curveKey in curves[i].Keys.OrderBy(k => k.Position))
            {
                var isFirstKey = j == 0;
                var isLastKey = j == curves[i].Keys.Count - 1;
                if (isLastKey && isNotLastCurve)
                {
                    continue;
                }

                var newCurveKey = new CurveKey((curveKey.Position + currLength) / totalLength, curveKey.Value, curveKey.TangentIn, curveKey.TangentOut);
                keys.Add(newCurveKey);

                j++;
            }

            currLength += curves[i].LastKey().Position;
        }

        return Curves.BuildCurve([.. keys], c1.PreLoop, c1.PostLoop);
    }

    /// <summary>
    /// Negates this curve. (so a value of 0 becomes 1, and vice-versa). Also takes care of updating tangents
    /// </summary>
    /// <param name="c1"></param>
    /// <returns></returns>
    public static Curve Negate(this Curve c1) => c1.ThrowIfNull().UpdateCurveKeys(k => new CurveKey(k.Position, 1 - k.Value, -k.TangentIn, -k.TangentOut));

    /// <summary>
    /// Scales the total length of this curve
    /// </summary>
    /// <param name="c1"></param>
    /// <param name="scaleFactor"></param>
    /// <returns></returns>
    public static Curve Scale(this Curve c1, float scaleFactor) => c1.ThrowIfNull().UpdateCurveKeys(k => new CurveKey(k.Position * scaleFactor, k.Value, k.TangentIn, k.TangentOut));

    /// <summary>
    /// Returns the first CurveKey of the curve
    /// </summary>
    public static CurveKey FirstKey(this Curve c1) => c1.ThrowIfNull().Keys.MinBy(k => k.Position)!;
    /// <summary>
    /// Returns the last CurveKey of the curve
    /// </summary>
    public static CurveKey LastKey(this Curve c1) => c1.ThrowIfNull().Keys.MaxBy(k => k.Position)!;

    /// <summary>
    /// Updates all of the curve keys in the given <see cref="Curve"/> with the given <paramref name="func"/>
    /// </summary>
    /// <returns>The updated curve</returns>
    public static Curve UpdateCurveKeys(this Curve c1, Func<CurveKey, CurveKey> func) => Curves.BuildCurve(c1.ThrowIfNull().Keys.Select(func.ThrowIfNull()).ToArray());
}
