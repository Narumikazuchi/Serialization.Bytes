namespace Narumikazuchi.Serialization.Bytes;

internal static class __Cache
{
    public static IDictionary<Type, IList<Object>> CreatedDeserializers { get; } = new Dictionary<Type, IList<Object>>();
    public static IDictionary<Type, IList<Object>> CreatedSerializers { get; } = new Dictionary<Type, IList<Object>>();
    public static IDictionary<Type, IList<Object>> CreatedSerializersDeserializers { get; } = new Dictionary<Type, IList<Object>>();
    public static IDictionary<Type, IList<IByteSerializer<ISerializable>>> CreatedOwnedSerializers { get; } = new Dictionary<Type, IList<IByteSerializer<ISerializable>>>();
}
