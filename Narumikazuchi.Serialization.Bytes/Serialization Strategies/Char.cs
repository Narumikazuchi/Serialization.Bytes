namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Char"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Char : ISerializationDeserializationStrategy<System.Byte[], System.Char>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Char Reference =>
            ref s_Reference;
    }

    partial struct Char
    {
        private static Char s_Reference = new();
    }

    partial struct Char : IDeserializationStrategy<System.Byte[], System.Char>
    {
        System.Char IDeserializationStrategy<System.Byte[], System.Char>.Deserialize(System.Byte[] input) =>
            BitConverter.ToChar(input);
    }

    partial struct Char : ISerializationStrategy<System.Byte[], System.Char>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Char>.Serialize(System.Char input) =>
            BitConverter.GetBytes(input);
    }
}