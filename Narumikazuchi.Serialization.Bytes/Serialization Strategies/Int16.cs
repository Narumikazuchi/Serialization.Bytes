namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="Int16Strategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class Int16Strategy : ISerializationDeserializationStrategy<Byte[], Int16>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class Int16Strategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Int16);
}

partial class Int16Strategy : IDeserializationStrategy<Byte[], Int16>
{
    Int16 IDeserializationStrategy<Byte[], Int16>.Deserialize(Byte[] input) =>
        BitConverter.ToInt16(input);
}

partial class Int16Strategy : ISerializationStrategy<Byte[], Int16>
{
    Byte[] ISerializationStrategy<Byte[], Int16>.Serialize(Int16 input) =>
        BitConverter.GetBytes(input);
}