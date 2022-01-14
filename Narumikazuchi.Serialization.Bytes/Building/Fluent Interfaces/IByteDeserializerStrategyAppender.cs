namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteDeserializer{TSerializable}"/> to use specified strategies for serialization.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteDeserializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Configures the <see cref="IByteDeserializer{TSerializable}"/> to use the specified strategy for the specified <typeparamref name="TFrom"/>.
    /// </summary>
    /// <param name="strategy">The strategy to use for the deserialization process of the type <typeparamref name="TFrom"/>.</param>
    public IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> UseStrategyForType<TFrom>([DisallowNull] IDeserializationStrategy<Byte[], TFrom> strategy);

    /// <summary>
    /// Configures the <see cref="IByteDeserializer{TSerializable}"/> to use the specified strategies for the deserialization of the associated types.
    /// </summary>
    /// <param name="strategies">The collections of strategies to use for deserialization.</param>
    public IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> UseStrategies([DisallowNull] IEnumerable<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>> strategies);
}
