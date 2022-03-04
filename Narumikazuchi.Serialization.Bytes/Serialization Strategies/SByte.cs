namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="SByte"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct SByte : ISerializationDeserializationStrategy<System.Byte[], System.SByte>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref SByte Reference =>
            ref s_Reference;

    }

    partial struct SByte
    {
        private static SByte s_Reference = new();
        private static IDeserializationStrategy<System.Byte[], System.SByte> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.SByte> s_SerializationStrategy = s_Reference;
    }

    partial struct SByte : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct SByte : IDeserializationStrategy<System.Byte[], System.SByte>
    {
        System.SByte IDeserializationStrategy<System.Byte[], System.SByte>.Deserialize(System.Byte[] input) =>
            unchecked((System.SByte)input[0]);
    }

    partial struct SByte : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not SByte value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct SByte : ISerializationStrategy<System.Byte[], System.SByte>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.SByte>.Serialize(System.SByte input) =>
            new System.Byte[] { unchecked((System.Byte)input) };
    }
}