namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="BooleanStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class BooleanStrategy : ISerializationDeserializationStrategy<Byte[], Boolean>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class BooleanStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Boolean);
}

partial class BooleanStrategy : IDeserializationStrategy<Byte[], Boolean>
{
    Boolean IDeserializationStrategy<Byte[], Boolean>.Deserialize(Byte[] input) =>
        BitConverter.ToBoolean(input);
}

partial class BooleanStrategy : ISerializationStrategy<Byte[], Boolean>
{
    Byte[] ISerializationStrategy<Byte[], Boolean>.Serialize(Boolean input) =>
        BitConverter.GetBytes(input);
}