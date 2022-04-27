namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Guid"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Guid : ISerializationDeserializationStrategy<System.Byte[], System.Guid>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Guid Reference =>
            ref s_Reference;
    }

    partial struct Guid
    {
        private static Guid s_Reference = new();
    }

    partial struct Guid : IDeserializationStrategy<System.Byte[], System.Guid>
    {
        System.Guid IDeserializationStrategy<System.Byte[], System.Guid>.Deserialize(System.Byte[] input) =>
            new(b: input);
    }

    partial struct Guid : ISerializationStrategy<System.Byte[], System.Guid>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Guid>.Serialize(System.Guid input) =>
            input.ToByteArray();
    }
}