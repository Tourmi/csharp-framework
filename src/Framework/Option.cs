using System.Diagnostics.CodeAnalysis;

namespace Tourmi.Framework;

/// <summary>
/// An optional value.
/// </summary>
[SuppressMessage("Naming", "CA1716:Identifiers should not match keywords", Justification = "Option type")]
public readonly partial struct Option<T>() : IEquatable<Option<T>>, IEnumerable<T>
{
    private readonly T _value = default!;

    /// <summary>
    /// Option with a value.
    /// </summary>
    public Option(T value) : this()
    {
        _value = value;
        HasValue = true;
    }

    /// <summary>
    /// Implicit conversion to an option.
    /// </summary>
    public static implicit operator Option<T>(T value) => new(value);

    /// <summary>
    /// Get the option's value, throwing a <see cref="InvalidCastException"/> if the option does not have a value.
    /// </summary>
    public static explicit operator T(Option<T> option) => option.HasValue ? option._value : throw new InvalidCastException("Maybe did not have a value.");

    /// <inheritdoc/>
    public static bool operator ==(Option<T> left, Option<T> right) => left.Equals(right);

    /// <inheritdoc/>
    public static bool operator !=(Option<T> left, Option<T> right) => !(left == right);

    /// <summary>
    /// True if the option has a value.
    /// </summary>
    public bool HasValue { get; }

    /// <summary>
    /// Unwraps the value from the option, throwing if it does not have a value.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the option does not have a value.</exception>
    public T Get()
    {
        if (!HasValue)
        {
            throw new InvalidOperationException("Cannot get value of an empty option.");
        }

        return _value;
    }

    /// <summary>
    /// Returns the option's value, or the given <paramref name="defaultValue"/> otherwise.
    /// </summary>
    [return: NotNullIfNotNull(nameof(defaultValue))]
    public T? GetOrDefault(T? defaultValue = default) => HasValue ? _value : defaultValue;

    /// <inheritdoc/>
    public bool Equals(Option<T> other) => HasValue && other.HasValue && EqualityComparer<T>.Default.Equals(_value, other._value) || HasValue == other.HasValue;

    /// <inheritdoc/>
    public override bool Equals([NotNullWhen(true)] object? obj) => obj is Option<T> option && Equals(option);

    /// <inheritdoc/>
    public override int GetHashCode() => HasValue && _value is not null ? _value.GetHashCode() : 0;

    /// <inheritdoc cref="IEnumerable{T}.GetEnumerator"/>
    public Enumerator GetEnumerator() => new(this);

    /// <inheritdoc/>
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => GetEnumerator();

    /// <inheritdoc/>
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
