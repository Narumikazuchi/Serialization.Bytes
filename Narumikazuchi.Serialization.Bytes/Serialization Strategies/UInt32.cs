namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="UInt32"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct UInt32 : ISerializationDeserializationStrategy<System.Byte[], System.UInt32>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref UInt32 Reference =>
            ref s_Reference;

    }

    partial struct UInt32
    {
        private static UInt32 s_Reference = new();
        private static IDeserializationStrategy<System.Byte[], System.UInt32> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.UInt32> s_SerializationStrategy = s_Reference;
    }

    partial struct UInt32 : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct UInt32 : IDeserializationStrategy<System.Byte[], System.UInt32>
    {
        System.UInt32 IDeserializationStrategy<System.Byte[], System.UInt32>.Deserialize(System.Byte[] input) =>
            BitConverter.ToUInt32(input);
    }

    partial struct UInt32 : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not UInt32 value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct UInt32 : ISerializationStrategy<System.Byte[], System.UInt32>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.UInt32>.Serialize(System.UInt32 input) =>
            BitConverter.GetBytes(input);
    }
}