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
    }

    partial struct Byte : IDeserializationStrategy<System.Byte[], System.Byte>
    {
        System.Byte IDeserializationStrategy<System.Byte[], System.Byte>.Deserialize(System.Byte[] input) =>
            input[0];
    }

    partial struct Byte : ISerializationStrategy<System.Byte[], System.Byte>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Byte>.Serialize(System.Byte input) =>
            new System.Byte[] { input };
    }
}