namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="UInt16"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct UInt16 : ISerializationDeserializationStrategy<System.Byte[], System.UInt16>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref UInt16 Reference =>
            ref s_Reference;

    }

    partial struct UInt16
    {
        private static UInt16 s_Reference = new();
        private static IDeserializationStrategy<System.Byte[], System.UInt16> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.UInt16> s_SerializationStrategy = s_Reference;
    }

    partial struct UInt16 : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct UInt16 : IDeserializationStrategy<System.Byte[], System.UInt16>
    {
        System.UInt16 IDeserializationStrategy<System.Byte[], System.UInt16>.Deserialize(System.Byte[] input) =>
            BitConverter.ToUInt16(input);
    }

    partial struct UInt16 : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not UInt16 value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct UInt16 : ISerializationStrategy<System.Byte[], System.UInt16>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.UInt16>.Serialize(System.UInt16 input) =>
            BitConverter.GetBytes(input);
    }
}