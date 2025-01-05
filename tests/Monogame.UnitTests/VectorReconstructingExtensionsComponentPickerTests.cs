using System.Reflection;
using System.Text.RegularExpressions;

namespace Tourmi.Monogame.Extensions;

[TestFixture(TestOf = typeof(VectorReconstructingExtensions))]
internal partial class VectorReconstructingExtensionsComponentPickerTests
{
    private static IEnumerable<TestCaseData> TestCases
    {
        get
        {
            foreach (var combination in GetComponentCombinations())
            {
                if (combination.Length == 0)
                {
                    continue;
                }

                for (var initialValueComponentCount = 1; initialValueComponentCount <= 4; initialValueComponentCount++)
                {
                    if (combination.Any(c => c >= initialValueComponentCount))
                    {
                        continue;
                    }

                    yield return new ComponentPickerTestCase(GetInitialValues(initialValueComponentCount), combination);
                }
            }
        }
    }

    private static IEnumerable<int[]> GetComponentCombinations(int countLeft = 4)
    {
        if (countLeft <= 0)
        {
            yield break;
        }

        for (var i = 0; i < 4; i++)
        {
            yield return [i];

            foreach (var subcombination in GetComponentCombinations(countLeft - 1))
            {
                yield return [i, .. subcombination];
            }
        }
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void MethodExists(ComponentPickerTestCase testCase)
    {
        Assert.That(testCase.MethodExists());
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void MethodReturnsExpectedValue(ComponentPickerTestCase testCase)
    {
        Assert.That(testCase.Actual(), Is.EqualTo(testCase.Expected()));
    }

    [Test]
    public void AreAllMethodsCovered()
    {
        var coveredFunctions = TestCases.Select(tc => (ComponentPickerTestCase)tc.Arguments[0]!).Select(t => t.GetMethodOrDefault()).ToHashSet();
        var actualMethods = typeof(VectorReconstructingExtensions).GetMethods().Where(m => ValidFunctionNameRegex().IsMatch(m.Name)).ToHashSet();

        Assert.That(coveredFunctions, Is.EquivalentTo(actualMethods));
    }

    private static Type GetTypeFromComponentCount(int componentCount) => componentCount switch
    {
        1 => typeof(float),
        2 => typeof(Vector2),
        3 => typeof(Vector3),
        4 => typeof(Vector4),
        _ => throw new ArgumentOutOfRangeException(nameof(componentCount)),
    };

    private static float[] GetInitialValues(int count)
        => Enumerable.Range(10, count).Select(value => (value % 2 == 0 ? value : -value) * 1.5f).ToArray();

    private static string GetComponentNameFromIndex(int index) => index switch
    {
        0 => "X",
        1 => "Y",
        2 => "Z",
        3 => "W",
        _ => throw new ArgumentOutOfRangeException(nameof(index)),
    };

    private static object ToVector(float[] values) => values.Length switch
    {
        1 => values[0],
        2 => new Vector2(values[0], values[1]),
        3 => new Vector3(values[0], values[1], values[2]),
        4 => new Vector4(values[0], values[1], values[2], values[3]),
        _ => throw new InvalidOperationException(),
    };

    internal class ComponentPickerTestCase(float[] componentValues, int[] pickedComponentIndexes)
    {
        private readonly float[] _componentValues = componentValues;
        private readonly int[] _pickedComponentIndexes = pickedComponentIndexes;

        public static implicit operator TestCaseData(ComponentPickerTestCase testCase) =>
            new TestCaseData(testCase).SetArgDisplayNames($"{ToVector(testCase._componentValues)}" + $".{testCase.GetMethodName()}()");

        public MethodInfo? GetMethodOrDefault()
        {
            var methodName = GetMethodName();
            var returnType = GetMethodReturnType();
            var parameterType = GetMethodParameterType();
            return typeof(VectorReconstructingExtensions)
                .GetMethods()
                .Where(m => m.Name == methodName)
                .Where(m => m.ReturnType == returnType)
                .Where(m => m.GetParameters().Select(p => p.ParameterType).Single().Equals(parameterType))
                .SingleOrDefault();
        }

        public bool MethodExists() => GetMethodOrDefault() != null;

        public object Actual()
        {
            var method = GetMethodOrDefault() ?? throw new InvalidOperationException();

            return method.Invoke(null, [GetMethodParameter()])!;
        }

        public object Expected()
        {
            var returnType = GetMethodReturnType();

            if (returnType == typeof(float))
            {
                return _componentValues[_pickedComponentIndexes[0]];
            }

            if (returnType == typeof(Vector2))
            {
                return new Vector2(_componentValues[_pickedComponentIndexes[0]], _componentValues[_pickedComponentIndexes[1]]);
            }

            if (returnType == typeof(Vector3))
            {
                return new Vector3(_componentValues[_pickedComponentIndexes[0]], _componentValues[_pickedComponentIndexes[1]], _componentValues[_pickedComponentIndexes[2]]);
            }

            if (returnType == typeof(Vector4))
            {
                return new Vector4(_componentValues[_pickedComponentIndexes[0]], _componentValues[_pickedComponentIndexes[1]], _componentValues[_pickedComponentIndexes[2]], _componentValues[_pickedComponentIndexes[3]]);
            }

            throw new InvalidOperationException();
        }

        private object GetMethodParameter() => ToVector(_componentValues);

        private string GetMethodName() => $"{string.Join(null, _pickedComponentIndexes.Select(GetComponentNameFromIndex))}";

        private Type GetMethodReturnType() => GetTypeFromComponentCount(_pickedComponentIndexes.Length);

        private Type GetMethodParameterType() => GetMethodParameter().GetType();
    }

    [GeneratedRegex("^[XYZW]{1,4}$")]
    private static partial Regex ValidFunctionNameRegex();
}
