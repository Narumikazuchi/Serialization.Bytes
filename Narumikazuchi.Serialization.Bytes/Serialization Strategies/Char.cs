namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="CharStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class CharStrategy : ISerializationDeserializationStrategy<Byte[], Char>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class CharStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(Char);
}

partial class CharStrategy : IDeserializationStrategy<Byte[], Char>
{
    Char IDeserializationStrategy<Byte[], Char>.Deserialize(Byte[] input) =>
        BitConverter.ToChar(input);
}

partial class CharStrategy : ISerializationStrategy<Byte[], Char>
{
    Byte[] ISerializationStrategy<Byte[], Char>.Serialize(Char input) =>
        BitConverter.GetBytes(input);
}