namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="UInt16Strategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class UInt16Strategy : ISerializationDeserializationStrategy<Byte[], UInt16>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class UInt16Strategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(UInt16);
}

partial class UInt16Strategy : IDeserializationStrategy<Byte[], UInt16>
{
    UInt16 IDeserializationStrategy<Byte[], UInt16>.Deserialize(Byte[] input) =>
        BitConverter.ToUInt16(input);
}

partial class UInt16Strategy : ISerializationStrategy<Byte[], UInt16>
{
    Byte[] ISerializationStrategy<Byte[], UInt16>.Serialize(UInt16 input) =>
        BitConverter.GetBytes(input);
}