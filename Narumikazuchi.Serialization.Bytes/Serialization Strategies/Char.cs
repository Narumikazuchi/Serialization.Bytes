namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Char"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Char : ISerializationDeserializationStrategy<System.Byte[], System.Char>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Char Reference =>
            ref s_Reference;

    }

    partial struct Char
    {
        private static Char s_Reference = new();
        private static IDeserializationStrategy<System.Byte[], System.Char> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.Char> s_SerializationStrategy = s_Reference;
    }

    partial struct Char : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct Char : IDeserializationStrategy<System.Byte[], System.Char>
    {
        System.Char IDeserializationStrategy<System.Byte[], System.Char>.Deserialize(System.Byte[] input) =>
            BitConverter.ToChar(input);
    }

    partial struct Char : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not Char value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct Char : ISerializationStrategy<System.Byte[], System.Char>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Char>.Serialize(System.Char input) =>
            BitConverter.GetBytes(input);
    }
}