namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="TimeSpanStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class TimeSpanStrategy : ISerializationDeserializationStrategy<Byte[], TimeSpan>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class TimeSpanStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(TimeSpan);
}

partial class TimeSpanStrategy : IDeserializationStrategy<Byte[], TimeSpan>
{
    TimeSpan IDeserializationStrategy<Byte[], TimeSpan>.Deserialize(Byte[] input) =>
        new(ticks: BitConverter.ToInt64(input));
}

partial class TimeSpanStrategy : ISerializationStrategy<Byte[], TimeSpan>
{
    Byte[] ISerializationStrategy<Byte[], TimeSpan>.Serialize(TimeSpan input) =>
        BitConverter.GetBytes(input.Ticks);
}