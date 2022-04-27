namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="UInt32"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct UInt32 : ISerializationDeserializationStrategy<System.Byte[], System.UInt32>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref UInt32 Reference =>
            ref s_Reference;
    }

    partial struct UInt32
    {
        private static UInt32 s_Reference = new();
    }

    partial struct UInt32 : IDeserializationStrategy<System.Byte[], System.UInt32>
    {
        System.UInt32 IDeserializationStrategy<System.Byte[], System.UInt32>.Deserialize(System.Byte[] input) =>
            BitConverter.ToUInt32(input);
    }

    partial struct UInt32 : ISerializationStrategy<System.Byte[], System.UInt32>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.UInt32>.Serialize(System.UInt32 input) =>
            BitConverter.GetBytes(input);
    }
}