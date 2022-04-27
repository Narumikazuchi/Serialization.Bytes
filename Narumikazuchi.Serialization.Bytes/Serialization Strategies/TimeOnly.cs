namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="TimeOnly"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct TimeOnly : ISerializationDeserializationStrategy<System.Byte[], System.TimeOnly>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref TimeOnly Reference =>
            ref s_Reference;
    }

    partial struct TimeOnly
    {
        private static TimeOnly s_Reference = new();
    }

    partial struct TimeOnly : IDeserializationStrategy<System.Byte[], System.TimeOnly>
    {
        System.TimeOnly IDeserializationStrategy<System.Byte[], System.TimeOnly>.Deserialize(System.Byte[] input) =>
            new(ticks: BitConverter.ToInt64(input));
    }

    partial struct TimeOnly : ISerializationStrategy<System.Byte[], System.TimeOnly>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.TimeOnly>.Serialize(System.TimeOnly input) =>
            BitConverter.GetBytes(input.Ticks);
    }
}