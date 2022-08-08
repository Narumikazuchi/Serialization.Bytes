namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="SingleStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class SingleStrategy : ISerializationDeserializationStrategy<Byte[], Single>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class SingleStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Single);
}

partial class SingleStrategy : IDeserializationStrategy<Byte[], Single>
{
    Single IDeserializationStrategy<Byte[], Single>.Deserialize(Byte[] input) =>
        BitConverter.ToSingle(input);
}

partial class SingleStrategy : ISerializationStrategy<Byte[], Single>
{
    Byte[] ISerializationStrategy<Byte[], Single>.Serialize(Single input) =>
        BitConverter.GetBytes(input);
}