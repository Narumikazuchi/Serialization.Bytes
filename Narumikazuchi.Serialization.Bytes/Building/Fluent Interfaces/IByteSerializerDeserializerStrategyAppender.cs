namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteSerializerDeserializer{TSerializable}"/> to use specified strategies for serialization.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Configures the <see cref="IByteSerializerDeserializer{TSerializable}"/> to use the specified strategy for the specified <typeparamref name="TFrom"/>.
    /// </summary>
    /// <param name="strategy">The strategy to use for the serialization process of the type <typeparamref name="TFrom"/>.</param>
    public IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> UseStrategyForType<TFrom>([DisallowNull] ISerializationDeserializationStrategy<Byte[], TFrom> strategy);

    /// <summary>
    /// Configures the <see cref="IByteSerializerDeserializer{TSerializable}"/> to use the specified strategies for the serialization of the associated types.
    /// </summary>
    /// <param name="strategies">The collections of strategies to use for serialization.</param>
    public IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> UseStrategies([DisallowNull] IEnumerable<KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>>> strategies);
}
