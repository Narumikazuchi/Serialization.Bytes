namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Int32"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Int32 : ISerializationDeserializationStrategy<System.Byte[], System.Int32>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Int32 Reference =>
            ref s_Reference;
    }

    partial struct Int32
    {
        private static Int32 s_Reference = new();
    }

    partial struct Int32 : IDeserializationStrategy<System.Byte[], System.Int32>
    {
        System.Int32 IDeserializationStrategy<System.Byte[], System.Int32>.Deserialize(System.Byte[] input) =>
            BitConverter.ToInt32(input);
    }

    partial struct Int32 : ISerializationStrategy<System.Byte[], System.Int32>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Int32>.Serialize(System.Int32 input) =>
            BitConverter.GetBytes(input);
    }
}