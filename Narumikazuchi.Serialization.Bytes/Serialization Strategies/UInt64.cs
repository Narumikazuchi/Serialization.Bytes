namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="UInt64Strategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class UInt64Strategy : ISerializationDeserializationStrategy<Byte[], UInt64>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class UInt64Strategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(UInt64);
}

partial class UInt64Strategy : IDeserializationStrategy<Byte[], UInt64>
{
    UInt64 IDeserializationStrategy<Byte[], UInt64>.Deserialize(Byte[] input) =>
        BitConverter.ToUInt64(input);
}

partial class UInt64Strategy : ISerializationStrategy<Byte[], UInt64>
{
    Byte[] ISerializationStrategy<Byte[], UInt64>.Serialize(UInt64 input) =>
        BitConverter.GetBytes(input);
}