namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Int32"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Int32 : ISerializationDeserializationStrategy<System.Byte[], System.Int32>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Int32 Reference =>
            ref s_Reference;

    }

    partial struct Int32
    {
        private static Int32 s_Reference = new();
        private static IDeserializationStrategy<System.Byte[], System.Int32> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.Int32> s_SerializationStrategy = s_Reference;
    }

    partial struct Int32 : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct Int32 : IDeserializationStrategy<System.Byte[], System.Int32>
    {
        System.Int32 IDeserializationStrategy<System.Byte[], System.Int32>.Deserialize(System.Byte[] input) =>
            BitConverter.ToInt32(input);
    }

    partial struct Int32 : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not Int32 value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct Int32 : ISerializationStrategy<System.Byte[], System.Int32>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Int32>.Serialize(System.Int32 input) =>
            BitConverter.GetBytes(input);
    }
}