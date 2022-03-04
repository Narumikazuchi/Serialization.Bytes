namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Byte"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Byte : ISerializationDeserializationStrategy<System.Byte[], System.Byte>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Byte Reference =>
            ref s_Reference;

    }

    partial struct Byte
    {
        private static Byte s_Reference = new();
        private static IDeserializationStrategy<System.Byte[], System.Byte> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.Byte> s_SerializationStrategy = s_Reference;
    }

    partial struct Byte : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct Byte : IDeserializationStrategy<System.Byte[], System.Byte>
    {
        System.Byte IDeserializationStrategy<System.Byte[], System.Byte>.Deserialize(System.Byte[] input) =>
            input[0];
    }

    partial struct Byte : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not Byte value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct Byte : ISerializationStrategy<System.Byte[], System.Byte>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Byte>.Serialize(System.Byte input) =>
            new System.Byte[] { input };
    }
}