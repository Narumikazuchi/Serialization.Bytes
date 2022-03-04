namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures the <see cref="IByteSerializer{TSerializable}"/> for a specific type.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface ISerializationForeignTypeConfigurator<TSerializable>
{
    /// <summary>
    /// Builds the serializer for deserialization.
    /// </summary>
    public IByteDeserializerDefaultStrategyAppender<TSerializable> ForDeserialization([DisallowNull] Func<ISerializationInfoGetter, TSerializable> constructFromSerializationData);
    /// <summary>
    /// Builds the serializer for deserialization.
    /// </summary>
    public IByteDeserializerDefaultStrategyAppender<TSerializable> ForDeserialization([DisallowNull] IDeserializationStrategy<Byte[], TSerializable> strategy);

    /// <summary>
    /// Builds the serializer for serialization.
    /// </summary>
    public IByteSerializerDefaultStrategyAppender<TSerializable> ForSerialization([DisallowNull] Action<TSerializable?, ISerializationInfoAdder> getSerializationData);
    /// <summary>
    /// Builds the serializer for serialization.
    /// </summary>
    public IByteSerializerDefaultStrategyAppender<TSerializable> ForSerialization([DisallowNull] ISerializationStrategy<Byte[], TSerializable> strategy);

    /// <summary>
    /// Builds the serializer for deserialization and serialization.
    /// </summary>
    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ForSerializationAndDeserialization([DisallowNull] Action<TSerializable, ISerializationInfoAdder> getSerializationData,
                                                                                                                [DisallowNull] Func<ISerializationInfoGetter, TSerializable> constructFromSerializationData);
    /// <summary>
    /// Builds the serializer for deserialization and serialization.
    /// </summary>
    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ForSerializationAndDeserialization([DisallowNull] ISerializationDeserializationStrategy<Byte[], TSerializable> strategy);
}
