namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="StringStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class StringStrategy : ISerializationDeserializationStrategy<Byte[], String>
{
    /// <inheritdoc/>
    public Int32 Priority { get; } = 1;
}

partial class StringStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(String);
}

partial class StringStrategy : IDeserializationStrategy<Byte[], String>
{
    String IDeserializationStrategy<Byte[], String>.Deserialize(Byte[] input)
    {
        Int32 size = BitConverter.ToInt32(input);
        if (size == 0)
        {
            return String.Empty;
        }
        return Encoding.UTF8.GetString(bytes: input,
                                        index: 4,
                                        count: size);
    }
}

partial class StringStrategy : ISerializationStrategy<Byte[], String>
{
    Byte[] ISerializationStrategy<Byte[], String>.Serialize(String input)
    {
        if (input is null)
        {
            return new Byte[] { 0x00, 0x00, 0x00, 0x00 };
        }

        Byte[] data = Encoding.UTF8.GetBytes(s: input);
        Byte[] size = BitConverter.GetBytes(data.Length);
        return size.Concat(data)
                    .ToArray();
    }
}