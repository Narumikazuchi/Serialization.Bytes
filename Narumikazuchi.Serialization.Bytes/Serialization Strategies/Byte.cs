namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="ByteStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class ByteStrategy : ISerializationDeserializationStrategy<Byte[], Byte>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class ByteStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Byte);
}

partial class ByteStrategy : IDeserializationStrategy<Byte[], Byte>
{
    Byte IDeserializationStrategy<Byte[], Byte>.Deserialize(Byte[] input) =>
        input[0];
}

partial class ByteStrategy : ISerializationStrategy<Byte[], Byte>
{
    Byte[] ISerializationStrategy<Byte[], Byte>.Serialize(Byte input) =>
        new Byte[] { input };
}