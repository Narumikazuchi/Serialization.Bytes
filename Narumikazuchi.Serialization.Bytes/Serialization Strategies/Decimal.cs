namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="Double"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct Decimal : ISerializationDeserializationStrategy<System.Byte[], System.Decimal>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref Decimal Reference =>
            ref s_Reference;
    }

    partial struct Decimal
    {
        private static Decimal s_Reference = new();
    }

    partial struct Decimal : IDeserializationStrategy<System.Byte[], System.Decimal>
    {
        System.Decimal IDeserializationStrategy<System.Byte[], System.Decimal>.Deserialize(System.Byte[] input) =>
            new(input.Select(x => (System.Int32)x)
                     .ToArray());
    }

    partial struct Decimal : ISerializationStrategy<System.Byte[], System.Decimal>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.Decimal>.Serialize(System.Decimal input) =>
            System.Decimal.GetBits(input)
                          .Select(x => (System.Byte)x)
                          .ToArray();
    }
}