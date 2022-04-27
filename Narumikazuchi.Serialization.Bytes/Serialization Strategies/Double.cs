namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Double"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Double : ISerializationDeserializationStrategy<System.Byte[], System.Double>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Double Reference =>
            ref s_Reference;
    }

    partial struct Double
    {
        private static Double s_Reference = new();
    }

    partial struct Double : IDeserializationStrategy<System.Byte[], System.Double>
    {
        System.Double IDeserializationStrategy<System.Byte[], System.Double>.Deserialize(System.Byte[] input) =>
            BitConverter.ToDouble(input);
    }

    partial struct Double : ISerializationStrategy<System.Byte[], System.Double>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Double>.Serialize(System.Double input) =>
            BitConverter.GetBytes(input);
    }
}