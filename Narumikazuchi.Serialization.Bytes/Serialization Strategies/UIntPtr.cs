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
    }

    partial struct UIntPtr : IDeserializationStrategy<System.Byte[], System.UIntPtr>
    {
        System.UIntPtr IDeserializationStrategy<System.Byte[], System.UIntPtr>.Deserialize(System.Byte[] input) =>
            new(value: BitConverter.ToUInt64(input));
    }

    partial struct UIntPtr : ISerializationStrategy<System.Byte[], System.UIntPtr>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.UIntPtr>.Serialize(System.UIntPtr input) =>
            BitConverter.GetBytes(input.ToUInt64());
    }
}