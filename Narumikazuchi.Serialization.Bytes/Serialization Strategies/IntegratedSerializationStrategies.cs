namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Contains all predefined strategies that are available for as default strategies for the three serializer classes
/// <see cref="ByteDeserializer{TSerializable}"/>, <see cref="ByteSerializer{TSerializable}"/> and <see cref="ByteSerializerDeserializer{TSerializable}"/>.
/// </summary>
public static partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Contains all predefined strategies in an <see cref="ReadOnlyDictionary{TKey, TValue, TEqualityComparer}"/>.
    /// </summary>
    public static ReadOnlyDictionary<Type, ISerializationDeserializationStrategy<Byte[]>, EqualityComparer<Type>> All { get; } = 
        ReadOnlyDictionary<Type, ISerializationDeserializationStrategy<Byte[]>, EqualityComparer<Type>>.CreateFrom(
            items: new Dictionary<Type, ISerializationDeserializationStrategy<Byte[]>>()
            {
                { typeof(Boolean),   BooleanStrategy.Instance },
                { typeof(Byte),      ByteStrategy.Instance },
                { typeof(Char),      CharStrategy.Instance },
                { typeof(Double),    DoubleStrategy.Instance },
                { typeof(Decimal),   DecimalStrategy.Instance },
                { typeof(Int16),     Int16Strategy.Instance },
                { typeof(Int32),     Int32Strategy.Instance },
                { typeof(Int64),     Int64Strategy.Instance },
                { typeof(IntPtr),    IntPtrStrategy.Instance },
                { typeof(SByte),     SByteStrategy.Instance },
                { typeof(Single),    SingleStrategy.Instance },
                { typeof(UInt16),    UInt16Strategy.Instance },
                { typeof(UInt32),    UInt32Strategy.Instance },
                { typeof(UInt64),    UInt64Strategy.Instance },
                { typeof(UIntPtr),   UIntPtrStrategy.Instance },
                { typeof(DateTime),  DateTimeStrategy.Instance },
                { typeof(DateOnly),  DateOnlyStrategy.Instance },
                { typeof(TimeSpan),  TimeSpanStrategy.Instance },
                { typeof(TimeOnly),  TimeOnlyStrategy.Instance },
                { typeof(Guid),      GuidStrategy.Instance },
                { typeof(Half),      HalfStrategy.Instance },
                { typeof(String),    StringStrategy.Instance }
            },
            equalityComparer: EqualityComparer<Type>.Default
        );

    // Caching them in case of repeated use
    internal static ReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>, EqualityComparer<Type>> AllSerialization { get; } =
        ReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>, EqualityComparer<Type>>.CreateFrom(
            items: All.Select(x => new KeyValuePair<Type, ISerializationStrategy<Byte[]>>(key: x.Key,
                                                                                                 value: x.Value)),
            equalityComparer: EqualityComparer<Type>.Default
        );

    // Caching them in case of repeated use
    internal static ReadOnlyDictionary<Type, IDeserializationStrategy<Byte[]>, EqualityComparer<Type>> AllDeserialization { get; } =
        ReadOnlyDictionary<Type, IDeserializationStrategy<Byte[]>, EqualityComparer<Type>>.CreateFrom(
            items: All.Select(x => new KeyValuePair<Type, IDeserializationStrategy<Byte[]>>(key: x.Key,
                                                                                                   value: x.Value)),
            equalityComparer: EqualityComparer<Type>.Default
        );
}