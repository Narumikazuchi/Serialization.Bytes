namespace Narumikazuchi.Serialization.Bytes;

partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Handles serialization of <see cref="String"/> values from and into <see cref="Byte"/>[].
    /// </summary>
    public readonly partial struct String : ISerializationDeserializationStrategy<System.Byte[], System.String>
    {
        /// <summary>
        /// The statically allocated reference of this struct.
        /// </summary>
        public static ref String Reference =>
            ref s_Reference;
    }

    partial struct String
    {
        private static String s_Reference = new();
    }

    partial struct String : IDeserializationStrategy<System.Byte[], System.String>
    {
        System.String IDeserializationStrategy<System.Byte[], System.String>.Deserialize(System.Byte[] input)
        {
            System.Int32 size = BitConverter.ToInt32(input);
            if (size == 0)
            {
                return System.String
                             .Empty;
            }
            return Encoding.UTF8
                           .GetString(bytes: input,
                                      index: 4,
                                      count: size);
        }
    }

    partial struct String : ISerializationStrategy<System.Byte[], System.String>
    {
        System.Byte[] ISerializationStrategy<System.Byte[], System.String>.Serialize(System.String input)
        {
            if (input is null)
            {
                return new System.Byte[] { 0x00, 0x00, 0x00, 0x00 };
            }

            System.Byte[] data = Encoding.UTF8
                                         .GetBytes(s: input);
            System.Byte[] size = BitConverter.GetBytes(data.Length);
            return size.Concat(data)
                       .ToArray();
        }
    }
}