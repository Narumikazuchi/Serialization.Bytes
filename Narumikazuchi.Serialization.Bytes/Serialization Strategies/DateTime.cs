namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="DateTime"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct DateTime : ISerializationDeserializationStrategy<System.Byte[], System.DateTime>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref DateTime Reference =>
            ref s_Reference;
    }

    partial struct DateTime
    {
        private static DateTime s_Reference = new();
    }

    partial struct DateTime : IDeserializationStrategy<System.Byte[], System.DateTime>
    {
        System.DateTime IDeserializationStrategy<System.Byte[], System.DateTime>.Deserialize(System.Byte[] input)
        {
            System.Int64 ticks = BitConverter.ToInt64(input);
            return new(ticks: ticks);
        }
    }

    partial struct DateTime : ISerializationStrategy<System.Byte[], System.DateTime>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.DateTime>.Serialize(System.DateTime input) =>
            BitConverter.GetBytes(input.Ticks);
    }
}