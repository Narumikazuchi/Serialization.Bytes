namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="Int32Strategy"/> values from and into <see cref="ByteStrategy"/>[].
/// </summary>
[Singleton]
public partial class Int32Strategy : ISerializationDeserializationStrategy<Byte[], Int32>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class Int32Strategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Int32);
}

partial class Int32Strategy : IDeserializationStrategy<Byte[], Int32>
{
    Int32 IDeserializationStrategy<Byte[], Int32>.Deserialize(Byte[] input) =>
        BitConverter.ToInt32(input);
}

partial class Int32Strategy : ISerializationStrategy<Byte[], Int32>
{
    Byte[] ISerializationStrategy<Byte[], Int32>.Serialize(Int32 input) =>
        BitConverter.GetBytes(input);
}