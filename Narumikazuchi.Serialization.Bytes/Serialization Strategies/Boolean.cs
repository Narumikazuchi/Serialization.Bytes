namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Boolean"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Boolean : ISerializationDeserializationStrategy<System.Byte[], System.Boolean>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Boolean Reference => 
            ref s_Reference;
    }

    partial struct Boolean
    {
        private static Boolean s_Reference = new();
    }

    partial struct Boolean : IDeserializationStrategy<System.Byte[], System.Boolean>
    {
        System.Boolean IDeserializationStrategy<System.Byte[], System.Boolean>.Deserialize(System.Byte[] input) =>
            BitConverter.ToBoolean(input);
    }

    partial struct Boolean : ISerializationStrategy<System.Byte[], System.Boolean>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Boolean>.Serialize(System.Boolean input) =>
            BitConverter.GetBytes(input);
    }
}