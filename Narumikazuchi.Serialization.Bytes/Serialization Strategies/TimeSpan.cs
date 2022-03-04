namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="TimeSpan"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct TimeSpan : ISerializationDeserializationStrategy<System.Byte[], System.TimeSpan>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref TimeSpan Reference =>
            ref s_Reference;

    }

    partial struct TimeSpan
    {
        private static TimeSpan s_Reference = new();
        private static IDeserializationStrategy<System.Byte[], System.TimeSpan> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.TimeSpan> s_SerializationStrategy = s_Reference;
    }

    partial struct TimeSpan : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct TimeSpan : IDeserializationStrategy<System.Byte[], System.TimeSpan>
    {
        System.TimeSpan IDeserializationStrategy<System.Byte[], System.TimeSpan>.Deserialize(System.Byte[] input) =>
            new(ticks: BitConverter.ToInt64(input));
    }

    partial struct TimeSpan : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not TimeSpan value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct TimeSpan : ISerializationStrategy<System.Byte[], System.TimeSpan>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.TimeSpan>.Serialize(System.TimeSpan input) =>
            BitConverter.GetBytes(input.Ticks);
    }
}