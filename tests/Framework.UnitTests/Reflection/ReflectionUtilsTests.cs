#pragma warning disable CA1812 // Avoid uninstantiated internal classes

namespace Tourmi.Framework.Reflection;

[TestFixture(TestOf = typeof(ReflectionUtils))]
internal class ReflectionUtilsTests
{
    [Test]
    public void GetImplementingTypesReturnsPublicFullyImplementedTypes()
    {
        Assert.That(ReflectionUtils.GetImplementingTypes<ITestInterface>(), Is.EquivalentTo(new Type[] { typeof(Implementation) }));
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Needs to be public for test")]
    public class Implementation : AbstractImplementation;

    private class PrivateImplementation : AbstractImplementation;

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Needs to be public for test")]
    public abstract class AbstractImplementation : ITestInterface
    {
        public int SomeProperty { get; set; }
    }

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Maintainability", "CA1515:Consider making public types internal", Justification = "Needs to be public for test")]
    public interface ITestInterface
    {
        public int SomeProperty { get; set; }
    }
}
