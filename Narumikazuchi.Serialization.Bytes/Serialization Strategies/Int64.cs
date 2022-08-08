namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="Int64Strategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class Int64Strategy : ISerializationDeserializationStrategy<Byte[], Int64>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class Int64Strategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Int64);
}

partial class Int64Strategy : IDeserializationStrategy<Byte[], Int64>
{
    Int64 IDeserializationStrategy<Byte[], Int64>.Deserialize(Byte[] input) =>
        BitConverter.ToInt64(input);
}

partial class Int64Strategy : ISerializationStrategy<Byte[], Int64>
{
    Byte[] ISerializationStrategy<Byte[], Int64>.Serialize(Int64 input) =>
        BitConverter.GetBytes(input);
}