namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="UInt32Strategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class UInt32Strategy : ISerializationDeserializationStrategy<Byte[], UInt32>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class UInt32Strategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(UInt32);
}

partial class UInt32Strategy : IDeserializationStrategy<Byte[], UInt32>
{
    UInt32 IDeserializationStrategy<Byte[], UInt32>.Deserialize(Byte[] input) =>
        BitConverter.ToUInt32(input);
}

partial class UInt32Strategy : ISerializationStrategy<Byte[], UInt32>
{
    Byte[] ISerializationStrategy<Byte[], UInt32>.Serialize(UInt32 input) =>
        BitConverter.GetBytes(input);
}