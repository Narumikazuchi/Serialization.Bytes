namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="IntPtr"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct IntPtr : ISerializationDeserializationStrategy<System.Byte[], System.IntPtr>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref IntPtr Reference =>
            ref s_Reference;
    }

    partial struct IntPtr
    {
        private static IntPtr s_Reference = new();
    }

    partial struct IntPtr : IDeserializationStrategy<System.Byte[], System.IntPtr>
    {
        System.IntPtr IDeserializationStrategy<System.Byte[], System.IntPtr>.Deserialize(System.Byte[] input) =>
            new(value: BitConverter.ToInt64(input));
    }

    partial struct IntPtr : ISerializationStrategy<System.Byte[], System.IntPtr>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.IntPtr>.Serialize(System.IntPtr input) =>
            BitConverter.GetBytes(input.ToInt64());
    }
}