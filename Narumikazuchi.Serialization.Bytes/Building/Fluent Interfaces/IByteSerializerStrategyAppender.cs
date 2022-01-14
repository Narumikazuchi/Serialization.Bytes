namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteSerializer{TSerializable}"/> to use specified strategies for serialization.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteSerializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Configures the <see cref="IByteSerializer{TSerializable}"/> to use the specified strategy for the specified <typeparamref name="TFrom"/>.
    /// </summary>
    /// <param name="strategy">The strategy to use for the serialization process of the type <typeparamref name="TFrom"/>.</param>
    public IByteSerializerStrategyAppenderOrFinalizer<TSerializable> UseStrategyForType<TFrom>([DisallowNull] ISerializationStrategy<Byte[], TFrom> strategy);

    /// <summary>
    /// Configures the <see cref="IByteSerializer{TSerializable}"/> to use the specified strategies for the serialization of the associated types.
    /// </summary>
    /// <param name="strategies">The collections of strategies to use for serialization.</param>
    public IByteSerializerStrategyAppenderOrFinalizer<TSerializable> UseStrategies([DisallowNull] IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies);
}
