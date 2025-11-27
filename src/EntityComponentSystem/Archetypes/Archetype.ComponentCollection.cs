using System.Runtime.InteropServices;

namespace Tourmi.EntityComponentSystem.Archetypes;

internal partial class Archetype
{
    private abstract class ComponentCollection
    {
        public abstract void AddEntry();
        public abstract void RemoveEntry(int index);
        public abstract void TakeEntryFrom(ComponentCollection originalCollection, int index);
    }

    private class ComponentCollection<T> : ComponentCollection
    {
        private readonly List<T?> _values = [];

        public T? this[int index]
        {
            get => _values[index];
            set => _values[index] = value;
        }

        public override void AddEntry() => _values.Add(default);

        public override void TakeEntryFrom(ComponentCollection originalCollection, int index)
        {
            if (originalCollection is not ComponentCollection<T> collection)
            {
                throw new ArgumentOutOfRangeException(nameof(originalCollection));
            }

            _values.Add(collection._values[index]);
            collection.RemoveEntry(index);
        }

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

        public ref T? GetRef(int index) => ref GetValues()[index];
    }
}
