using System.Runtime.InteropServices;

namespace Tourmi.EntityComponentSystem.Archetypes;

internal partial class Archetype
{
    private abstract class ComponentCollection
    {
        public abstract void AddEntry();
        public abstract void RemoveEntry(int index);
    }

    private class ComponentCollection<T> : ComponentCollection
    {
        private readonly List<T?> _values = [];

        public override void AddEntry() => _values.Add(default);

        public override void RemoveEntry(int index)
        {
            if (index == _values.Count - 1)
            {
                _values.RemoveAt(index);
                return;
            }

            var replaceIndex = _values.Count - 1;
            var replaceValue = _values[replaceIndex];
            _values.RemoveAt(replaceIndex);
            _values[index] = replaceValue;
        }

        public Span<T?> GetValues()
        {
            var span = CollectionsMarshal.AsSpan(_values);
            return span;
        }
    }
}
