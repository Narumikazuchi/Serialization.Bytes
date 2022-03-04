namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="UIntPtr"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct UIntPtr : ISerializationDeserializationStrategy<System.Byte[], System.UIntPtr>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref UIntPtr Reference =>
            ref s_Reference;

    }

    partial struct UIntPtr
    {
        private static UIntPtr s_Reference = new();
        private static IDeserializationStrategy<System.Byte[], System.UIntPtr> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.UIntPtr> s_SerializationStrategy = s_Reference;
    }

    partial struct UIntPtr : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct UIntPtr : IDeserializationStrategy<System.Byte[], System.UIntPtr>
    {
        System.UIntPtr IDeserializationStrategy<System.Byte[], System.UIntPtr>.Deserialize(System.Byte[] input) =>
            new(value: BitConverter.ToUInt64(input));
    }

    partial struct UIntPtr : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not UIntPtr value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct UIntPtr : ISerializationStrategy<System.Byte[], System.UIntPtr>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.UIntPtr>.Serialize(System.UIntPtr input) =>
            BitConverter.GetBytes(input.ToUInt64());
    }
}