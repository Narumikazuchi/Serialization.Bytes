namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Boolean"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Boolean : ISerializationDeserializationStrategy<System.Byte[], System.Boolean>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Boolean Reference => 
            ref s_Reference;

    }

    partial struct Boolean
    {
        private static Boolean s_Reference = new();
        private static IDeserializationStrategy<System.Byte[], System.Boolean> s_DeserializationStrategy = s_Reference;
        private static ISerializationStrategy<System.Byte[], System.Boolean> s_SerializationStrategy = s_Reference;
    }

    partial struct Boolean : IDeserializationStrategy<System.Byte[]>
    {
        Object? IDeserializationStrategy<System.Byte[]>.Deserialize(System.Byte[] input) =>
            s_DeserializationStrategy.Deserialize(input);
    }

    partial struct Boolean : IDeserializationStrategy<System.Byte[], System.Boolean>
    {
        System.Boolean IDeserializationStrategy<System.Byte[], System.Boolean>.Deserialize(System.Byte[] input) =>
            BitConverter.ToBoolean(input);
    }

    partial struct Boolean : ISerializationStrategy<System.Byte[]>
    {
        System.Byte[] ISerializationStrategy<System.Byte[]>.Serialize(Object? input)
        {
            if (input is not Boolean value)
            {
                throw new InvalidCastException();
            }
            return s_SerializationStrategy.Serialize(value);
        }
    }

    partial struct Boolean : ISerializationStrategy<System.Byte[], System.Boolean>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Boolean>.Serialize(System.Boolean input) =>
            BitConverter.GetBytes(input);
    }
}