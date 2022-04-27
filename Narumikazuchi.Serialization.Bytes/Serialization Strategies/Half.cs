namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Half"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Half : ISerializationDeserializationStrategy<System.Byte[], System.Half>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Half Reference =>
            ref s_Reference;
    }

    partial struct Half
    {
        private static Half s_Reference = new();
    }

    partial struct Half : IDeserializationStrategy<System.Byte[], System.Half>
    {
        System.Half IDeserializationStrategy<System.Byte[], System.Half>.Deserialize(System.Byte[] input) =>
            BitConverter.ToHalf(input);
    }

    partial struct Half : ISerializationStrategy<System.Byte[], System.Half>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Half>.Serialize(System.Half input) =>
            BitConverter.GetBytes(input);
    }
}