namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="HalfStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class HalfStrategy : ISerializationDeserializationStrategy<Byte[], Half>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class HalfStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Half);
}

partial class HalfStrategy : IDeserializationStrategy<Byte[], Half>
{
    Half IDeserializationStrategy<Byte[], Half>.Deserialize(Byte[] input) =>
        BitConverter.ToHalf(input);
}

partial class HalfStrategy : ISerializationStrategy<Byte[], Half>
{
    Byte[] ISerializationStrategy<Byte[], Half>.Serialize(Half input) =>
        BitConverter.GetBytes(input);
}