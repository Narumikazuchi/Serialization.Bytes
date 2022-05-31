namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="DateOnly"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct DateOnly : ISerializationDeserializationStrategy<System.Byte[], System.DateOnly>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref DateOnly Reference =>
            ref s_Reference;
    }

    partial struct DateOnly
    {
        private static DateOnly s_Reference = new();
    }

    partial struct DateOnly : IDeserializationStrategy<System.Byte[], System.DateOnly>
    {
        System.DateOnly IDeserializationStrategy<System.Byte[], System.DateOnly>.Deserialize(System.Byte[] input)
        {
            System.Int32 year = BitConverter.ToInt32(input, 0);
            System.Int32 month = BitConverter.ToInt32(input, 4);
            System.Int32 day = BitConverter.ToInt32(input, 8);
            return new(year: year,
                       month: month,
                       day: day);
        }
    }

    partial struct DateOnly : ISerializationStrategy<System.Byte[], System.DateOnly>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.DateOnly>.Serialize(System.DateOnly input) =>
            BitConverter.GetBytes(input.Year)
                        .Concat(BitConverter.GetBytes(input.Month))
                        .Concat(BitConverter.GetBytes(input.Day))
                        .ToArray();
    }
}