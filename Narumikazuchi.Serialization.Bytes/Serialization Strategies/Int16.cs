namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Int16"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Int16 : ISerializationDeserializationStrategy<System.Byte[], System.Int16>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Int16 Reference =>
            ref s_Reference;
    }

    partial struct Int16
    {
        private static Int16 s_Reference = new();
    }

    partial struct Int16 : IDeserializationStrategy<System.Byte[], System.Int16>
    {
        System.Int16 IDeserializationStrategy<System.Byte[], System.Int16>.Deserialize(System.Byte[] input) =>
            BitConverter.ToInt16(input);
    }

    partial struct Int16 : ISerializationStrategy<System.Byte[], System.Int16>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Int16>.Serialize(System.Int16 input) =>
            BitConverter.GetBytes(input);
    }
}