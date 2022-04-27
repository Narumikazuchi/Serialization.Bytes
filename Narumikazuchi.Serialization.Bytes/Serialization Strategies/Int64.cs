namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Int64"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Int64 : ISerializationDeserializationStrategy<System.Byte[], System.Int64>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Int64 Reference =>
            ref s_Reference;
    }

    partial struct Int64
    {
        private static Int64 s_Reference = new();
    }

    partial struct Int64 : IDeserializationStrategy<System.Byte[], System.Int64>
    {
        System.Int64 IDeserializationStrategy<System.Byte[], System.Int64>.Deserialize(System.Byte[] input) =>
            BitConverter.ToInt64(input);
    }

    partial struct Int64 : ISerializationStrategy<System.Byte[], System.Int64>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Int64>.Serialize(System.Int64 input) =>
            BitConverter.GetBytes(input);
    }
}