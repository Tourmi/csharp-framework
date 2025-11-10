using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework;

/// <summary>
/// An optional value.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Option type")]
public readonly struct Option<T>() : IEquatable<Option<T>>, IEnumerable<T>
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
            _hasValue = option._hasValue;
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

    private readonly T _value = default!;
    private readonly bool _hasValue;

    /// <summary>
    /// Option with a value.
    /// </summary>
    public Option(T value) : this()
    {
        _value = value;
        _hasValue = true;
    }

    /// <summary>
    /// Implicit conversion to an option.
    /// </summary>
    public static implicit operator Option<T>(T value) => new(value);

    /// <summary>
    /// Get the option's value, throwing a <see cref="InvalidCastException"/> if the option does not have a value.
    /// </summary>
    public static explicit operator T(Option<T> option) => option._hasValue ? option._value : throw new InvalidCastException("Maybe did not have a value.");

    /// <inheritdoc/>
    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

    /// <summary>
    /// True if the option has a value.
    /// </summary>
    public bool HasValue => _hasValue;

    /// <summary>
    /// Unwraps the value from the option, throwing if it does not have a value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the option does not have a value.</exception>
    public T Get()
    {
        if (!_hasValue)
        {
            throw new InvalidOperationException("Cannot get value of an empty option.");
        }

        return _value;
    }

    /// <summary>
    /// Returns the option's value, or the given <paramref name="defaultValue"/> otherwise.
    /// </summary>
    [return: NotNullIfNotNull(nameof(defaultValue))]
    public T? GetOrDefault(T? defaultValue = default) => _hasValue ? _value : defaultValue;

    /// <inheritdoc/>
    public bool Equals(Option<T> other) => _hasValue && other._hasValue && EqualityComparer<T>.Default.Equals(_value, other._value) || _hasValue == other._hasValue;

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Option<T> option && Equals(option);

    /// <inheritdoc/>
    public override int GetHashCode() => _hasValue && _value is not null ? _value.GetHashCode() : 0;

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public Enumerator GetEnumerator() => new(this);

    /// <inheritdoc/>
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
