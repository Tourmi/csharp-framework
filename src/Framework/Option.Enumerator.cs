namespace Tourmi.Framework;

public readonly partial struct Option<T>
{
    /// <summary>
    /// Enumerator for this Option
    /// </summary>
    public struct Enumerator() : IEnumerator<T>
    {
        private readonly T _value = default!;
        private readonly bool _hasValue;
        private bool _isAtValue;

        /// <summary>
        /// Constructs the enumerator with the given option.
        /// </summary>
        public Enumerator(Option<T> option) : this()
        {
            _hasValue = option.HasValue;
            if (_hasValue)
            {
                _value = option._value;
            }
        }

        /// <inheritdoc/>
        public readonly T Current => _isAtValue ? _value : throw new InvalidOperationException("Enumerator was in an invalid state.");

        /// <inheritdoc/>
        readonly object? IEnumerator.Current => Current;

        /// <inheritdoc/>
        public readonly void Dispose()
        {
            // Nothing to dispose.
        }

        /// <inheritdoc/>
        public bool MoveNext()
        {
            if (!_hasValue || _isAtValue)
            {
                return false;
            }

            _isAtValue = true;
            return true;
        }

        /// <inheritdoc/>
        public void Reset() => _isAtValue = false;
    }
}
