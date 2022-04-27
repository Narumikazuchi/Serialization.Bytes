namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Single"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Single : ISerializationDeserializationStrategy<System.Byte[], System.Single>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Single Reference =>
            ref s_Reference;
    }

    partial struct Single
    {
        private static Single s_Reference = new();
    }

    partial struct Single : IDeserializationStrategy<System.Byte[], System.Single>
    {
        System.Single IDeserializationStrategy<System.Byte[], System.Single>.Deserialize(System.Byte[] input) =>
            BitConverter.ToSingle(input);
    }

    partial struct Single : ISerializationStrategy<System.Byte[], System.Single>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Single>.Serialize(System.Single input) =>
            BitConverter.GetBytes(input);
    }
}