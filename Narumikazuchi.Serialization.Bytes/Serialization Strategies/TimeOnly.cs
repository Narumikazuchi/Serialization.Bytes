namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="TimeOnlyStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class TimeOnlyStrategy : ISerializationDeserializationStrategy<Byte[], TimeOnly>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class TimeOnlyStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(TimeOnly);
}

partial class TimeOnlyStrategy : IDeserializationStrategy<Byte[], TimeOnly>
{
    TimeOnly IDeserializationStrategy<Byte[], TimeOnly>.Deserialize(Byte[] input) =>
        new(ticks: BitConverter.ToInt64(input));
}

partial class TimeOnlyStrategy : ISerializationStrategy<Byte[], TimeOnly>
{
    Byte[] ISerializationStrategy<Byte[], TimeOnly>.Serialize(TimeOnly input) =>
        BitConverter.GetBytes(input.Ticks);
}