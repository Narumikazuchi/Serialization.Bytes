namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="UIntPtrStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class UIntPtrStrategy : ISerializationDeserializationStrategy<Byte[], UIntPtr>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class UIntPtrStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(UIntPtr);
}

partial class UIntPtrStrategy : IDeserializationStrategy<Byte[], UIntPtr>
{
    UIntPtr IDeserializationStrategy<Byte[], UIntPtr>.Deserialize(Byte[] input) =>
        new(value: BitConverter.ToUInt64(input));
}

partial class UIntPtrStrategy : ISerializationStrategy<Byte[], UIntPtr>
{
    Byte[] ISerializationStrategy<Byte[], UIntPtr>.Serialize(UIntPtr input) =>
        BitConverter.GetBytes(input.ToUInt64());
}