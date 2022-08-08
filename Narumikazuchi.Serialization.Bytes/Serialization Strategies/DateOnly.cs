namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Handles serialization of <see cref="DateOnlyStrategy"/> values from and into <see cref="Byte"/>[].
/// </summary>
[Singleton]
public partial class DateOnlyStrategy : ISerializationDeserializationStrategy<Byte[], DateOnly>
{
    /// <inheritdoc/>
    public Int32 Priority { get; }
}

partial class DateOnlyStrategy : ITypeAppliedStrategy
{
    /// <inheritdoc/>
    public Boolean CanBeAppliedTo(Type type) =>
        type == typeof(DateOnly);
}

partial class DateOnlyStrategy : IDeserializationStrategy<Byte[], DateOnly>
{
    DateOnly IDeserializationStrategy<Byte[], DateOnly>.Deserialize(Byte[] input)
    {
        Int32 year = BitConverter.ToInt32(input, 0);
        Int32 month = BitConverter.ToInt32(input, 4);
        Int32 day = BitConverter.ToInt32(input, 8);
        return new(year: year,
                    month: month,
                    day: day);
    }
}

partial class DateOnlyStrategy : ISerializationStrategy<Byte[], DateOnly>
{
    Byte[] ISerializationStrategy<Byte[], DateOnly>.Serialize(DateOnly input) =>
        BitConverter.GetBytes(input.Year)
                    .Concat(BitConverter.GetBytes(input.Month))
                    .Concat(BitConverter.GetBytes(input.Day))
                    .ToArray();
}