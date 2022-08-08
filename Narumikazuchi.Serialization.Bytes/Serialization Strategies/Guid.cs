namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="GuidStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class GuidStrategy : ISerializationDeserializationStrategy<Byte[], Guid>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class GuidStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Guid);
}

partial class GuidStrategy : IDeserializationStrategy<Byte[], Guid>
{
    Guid IDeserializationStrategy<Byte[], Guid>.Deserialize(Byte[] input) =>
        new(b: input);
}

partial class GuidStrategy : ISerializationStrategy<Byte[], Guid>
{
    Byte[] ISerializationStrategy<Byte[], Guid>.Serialize(Guid input) =>
        input.ToByteArray();
}