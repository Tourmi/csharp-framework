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

    public class Implementation : AbstractImplementation;

    private class PrivateImplementation : AbstractImplementation;

    public abstract class AbstractImplementation : ITestInterface
    {
        public int SomeProperty { get; set; }
    }

    public interface ITestInterface
    {
        public int SomeProperty { get; set; }
    }
}
