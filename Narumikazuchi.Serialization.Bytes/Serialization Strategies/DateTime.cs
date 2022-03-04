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
        private static IDeserializationStrategy<System.Byte[], System.DateTime> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.DateTime> s_SerializationStrategy = s_Reference;
    }

    partial struct DateTime : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct DateTime : IDeserializationStrategy<System.Byte[], System.DateTime>
    {
        System.DateTime IDeserializationStrategy<System.Byte[], System.DateTime>.Deserialize(System.Byte[] input)
        {
            System.Int64 ticks = BitConverter.ToInt64(input);
            return new(ticks: ticks);
        }
    }

    partial struct DateTime : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not DateTime value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct DateTime : ISerializationStrategy<System.Byte[], System.DateTime>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.DateTime>.Serialize(System.DateTime input) =>
            BitConverter.GetBytes(input.Ticks);
    }
}