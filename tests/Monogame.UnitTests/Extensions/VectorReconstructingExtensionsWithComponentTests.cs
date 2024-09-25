using System.Reflection;

namespace Tourmi.Monogame.Extensions;

[TestFixture(TestOf = typeof(VectorReconstructingExtensions))]
internal class VectorReconstructingExtensionsWithComponentTests
{
    private static readonly float[] ConstantValue = [100f];

    private static IEnumerable<TestCaseData> TestCases
    {
        get
        {
            for (var entryComponentCount = 1; entryComponentCount <= 4; entryComponentCount++)
            {
                var entryValue = GetInitialValues(entryComponentCount);
                var combinations = GetDistinctComponentCombinations().ToArray();
                foreach (var combination in combinations)
                {
                    var paramGroups = combination.ToArray();
                    var max = paramGroups.Max(g => g.Max());
                    var containsY = paramGroups.Any(g => g.Contains(1));
                    var containsZ = paramGroups.Any(g => g.Contains(2));
                    var shouldContinue = paramGroups.All(g => g.All(i =>
                        {
                            if (i <= entryComponentCount)
                            {
                                return true;
                            }

                            if (entryComponentCount <= 1 && !containsY && max > 1)
                            {
                                return false;
                            }

                            if (entryComponentCount <= 2 && !containsZ && max > 2)
                            {
                                return false;
                            }

                            return true;
                        }));

                    if (!shouldContinue || paramGroups.Length == 0 || paramGroups.Any(g => g.Length is 0))
                    {
                        continue;
                    }

                    var differentValuesParams = paramGroups.Select(g => (g, g.Select(i => (i + 1) * -2f).ToArray())).ToArray();
                    yield return new WithComponentTestCase(entryValue, differentValuesParams);
                    if (paramGroups.Length > 1)
                    {
                        continue;
                    }
                    var sameValueParams = (paramGroups[0], ConstantValue);
                    yield return new WithComponentTestCase(entryValue, [sameValueParams]);
                }
            }
        }
    }

    private static IEnumerable<int[][]> GetDistinctComponentCombinations()
        => GetComponentCombinations().DistinctBy(c => $"({string.Join(null, c.Select(g => $"({string.Join(null, g)})"))})");

    private static IEnumerable<int[][]> GetComponentCombinations(int startIndex = 0)
    {
        if (startIndex >= 4)
        {
            yield break;
        }

        yield return [[startIndex]];

        foreach (var subcombination in GetComponentCombinations(startIndex + 1))
        {
            yield return subcombination;
            for (var i = 0; i < 4 - startIndex; i++)
            {
                yield return [[startIndex, .. subcombination.Take(i).SelectMany(c => c)], .. subcombination.Skip(i)];
            }
        }
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void MethodExists(WithComponentTestCase testCase)
    {
        Assert.That(testCase.MethodExists());
    }

    [Test]
    [TestCaseSource(nameof(TestCases))]
    public void MethodReturnsExpectedValue(WithComponentTestCase testCase)
    {
        Assert.That(testCase.Actual(), Is.EqualTo(testCase.Expected()));
    }

    [Test]
    public void AreAllMethodsCovered()
    {
        var coveredFunctions = TestCases.Select(tc => (WithComponentTestCase)tc.Arguments[0]!).Select(t => t.GetMethodOrDefault()).Distinct().ToHashSet();
        var actualMethods = typeof(VectorReconstructingExtensions).GetMethods().Where(m => m.Name.StartsWith("With", StringComparison.InvariantCulture)).ToHashSet();

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

    internal class WithComponentTestCase(
        float[] entryValue,
        (int[] Indexes, float[] Values)[] overwrittenValues)
    {
        private readonly float[] _entryValue = entryValue;
        private readonly (int[] Indexes, float[] Values)[] _overwrittenValues = overwrittenValues;

        public static implicit operator TestCaseData(WithComponentTestCase testCase)
        {
            return new TestCaseData(testCase).SetArgDisplayNames($"{ToVector(testCase._entryValue)}" +
                $".{testCase.GetMethodName()}" +
                $"({string.Join(", ", testCase.GetMethodParameters().Skip(1).Select(p => p.ToString()).ToArray())})");
        }

        public MethodInfo? GetMethodOrDefault()
        {
            var methodName = GetMethodName();
            var returnType = GetMethodReturnType();
            var parameterTypes = GetMethodParameterTypes().ToArray();
            return typeof(VectorReconstructingExtensions)
                .GetMethods()
                .Where(m => m.Name == methodName)
                .Where(m => m.ReturnType == returnType)
                .Where(m => m.GetParameters().Select(p => p.ParameterType).SequenceEqual(parameterTypes))
                .SingleOrDefault();
        }

        public bool MethodExists() => GetMethodOrDefault() != null;

        public object Actual()
        {
            var method = GetMethodOrDefault() ?? throw new InvalidOperationException();

            return method.Invoke(null, GetMethodParameters().ToArray())!;
        }

        public object Expected()
        {
            var returnType = GetMethodReturnType();

            if (returnType == typeof(float))
            {
                return GetValueAt(0);
            }

            if (returnType == typeof(Vector2))
            {
                return new Vector2(GetValueAt(0), GetValueAt(1));
            }

            if (returnType == typeof(Vector3))
            {
                return new Vector3(GetValueAt(0), GetValueAt(1), GetValueAt(2));
            }

            if (returnType == typeof(Vector4))
            {
                return new Vector4(GetValueAt(0), GetValueAt(1), GetValueAt(2), GetValueAt(3));
            }

            throw new InvalidOperationException();
        }

        private IEnumerable<object> GetMethodParameters()
        {
            yield return ToVector(_entryValue);

            if (_overwrittenValues.Any(v => v.Values.Length == 1 && v.Indexes.Length > 1))
            {
                yield return _overwrittenValues[0].Values[0];
                yield break;
            }

            foreach (var (indexes, values) in _overwrittenValues)
            {
                yield return ToVector(values);
            }
        }

        private string GetMethodName()
        {
            if (_overwrittenValues.Any(v => v.Values.Length == 1 && v.Indexes.Length > 1))
            {
                return $"With{string.Join(null, _overwrittenValues.First().Indexes.Select(GetComponentNameFromIndex))}";
            }

            var baseName = "With";
            foreach (var (indexes, values) in _overwrittenValues)
            {
                foreach (var index in indexes)
                {
                    baseName += GetComponentNameFromIndex(index);
                }
            }

            return baseName;
        }

        private Type GetMethodReturnType()
        {
            var componentCount = _overwrittenValues.Max(v => Math.Max(v.Indexes.Max() + 1, _entryValue.Length));

            return GetTypeFromComponentCount(componentCount);
        }

        private IEnumerable<Type> GetMethodParameterTypes() => GetMethodParameters().Select(p => p.GetType());

        private float GetValueAt(int componentIndex)
        {
            if (_overwrittenValues.Any(v => v.Indexes.Contains(componentIndex)))
            {
                if (_overwrittenValues.Any(v => v.Values.Length == 1 && v.Indexes.Length > 1))
                {
                    return _overwrittenValues[0].Values[0];
                }

                var (indexes, values) = _overwrittenValues.Where(v => v.Indexes!.Contains(componentIndex)).Single();
                var index = Array.IndexOf(indexes!, componentIndex);
                return values[index];
            }

            return _entryValue[componentIndex];
        }
    }
}
