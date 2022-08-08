namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="IntPtrStrategy"/> values from and into <see cref="ByteStrategy"/>[].
/// </summary>
[Singleton]
public partial class IntPtrStrategy : ISerializationDeserializationStrategy<Byte[], IntPtr>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class IntPtrStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(IntPtr);
}

partial class IntPtrStrategy : IDeserializationStrategy<Byte[], IntPtr>
{
    IntPtr IDeserializationStrategy<Byte[], IntPtr>.Deserialize(Byte[] input) =>
        new(value: BitConverter.ToInt64(input));
}

partial class IntPtrStrategy : ISerializationStrategy<Byte[], IntPtr>
{
    Byte[] ISerializationStrategy<Byte[], IntPtr>.Serialize(IntPtr input) =>
        BitConverter.GetBytes(input.ToInt64());
}