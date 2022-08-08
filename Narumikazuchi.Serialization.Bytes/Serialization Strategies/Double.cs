namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="DoubleStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class DoubleStrategy : ISerializationDeserializationStrategy<Byte[], Double>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class DoubleStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Double);
}

partial class DoubleStrategy : IDeserializationStrategy<Byte[], Double>
{
    Double IDeserializationStrategy<Byte[], Double>.Deserialize(Byte[] input) =>
        BitConverter.ToDouble(input);
}

partial class DoubleStrategy : ISerializationStrategy<Byte[], Double>
{
    Byte[] ISerializationStrategy<Byte[], Double>.Serialize(Double input) =>
        BitConverter.GetBytes(input);
}