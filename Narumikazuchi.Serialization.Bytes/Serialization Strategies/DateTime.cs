namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="DateTimeStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class DateTimeStrategy : ISerializationDeserializationStrategy<Byte[], DateTime>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class DateTimeStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(DateTime);
}

partial class DateTimeStrategy : IDeserializationStrategy<Byte[], DateTime>
{
    DateTime IDeserializationStrategy<Byte[], DateTime>.Deserialize(Byte[] input)
    {
        Int64 ticks = BitConverter.ToInt64(input);
        return new(ticks: ticks);
    }
}

partial class DateTimeStrategy : ISerializationStrategy<Byte[], DateTime>
{
    Byte[] ISerializationStrategy<Byte[], DateTime>.Serialize(DateTime input) =>
        BitConverter.GetBytes(input.Ticks);
}