namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="SByteStrategy"/> values from and into <see cref="ByteStrategy"/>[].
/// </summary>
[Singleton]
public partial class SByteStrategy : ISerializationDeserializationStrategy<Byte[], SByte>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class SByteStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(SByte);
}

partial class SByteStrategy : IDeserializationStrategy<Byte[], SByte>
{
    SByte IDeserializationStrategy<Byte[], SByte>.Deserialize(Byte[] input) =>
        unchecked((SByte)input[0]);
}

partial class SByteStrategy : ISerializationStrategy<Byte[], SByte>
{
    Byte[] ISerializationStrategy<Byte[], SByte>.Serialize(SByte input) =>
        new Byte[] { unchecked((Byte)input) };
}