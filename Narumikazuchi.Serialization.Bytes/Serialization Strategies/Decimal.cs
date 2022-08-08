namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="DoubleStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class DecimalStrategy : ISerializationDeserializationStrategy<Byte[], Decimal>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class DecimalStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Decimal);
}

partial class DecimalStrategy : IDeserializationStrategy<Byte[], Decimal>
{
    Decimal IDeserializationStrategy<Byte[], Decimal>.Deserialize(Byte[] input)
    {
        Span<Byte> data = input.AsSpan();

        return new(new Int32[]
        {
            BitConverter.ToInt32(data[..4]),
            BitConverter.ToInt32(data[4..8]),
            BitConverter.ToInt32(data[8..12]),
            BitConverter.ToInt32(data[12..])
        });
    }
}

partial class DecimalStrategy : ISerializationStrategy<Byte[], Decimal>
{
    Byte[] ISerializationStrategy<Byte[], Decimal>.Serialize(Decimal input) =>
        Decimal.GetBits(input)
                        .Select(BitConverter.GetBytes)
                        .SelectMany(x => x)
                        .ToArray();
}