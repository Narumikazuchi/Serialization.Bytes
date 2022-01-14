namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures the <see cref="IByteDeserializer{TSerializable}"/> for a specific type.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteDeserializerTypeConfigurator
{
    /// <summary>
    /// Configure the <see cref="IByteDeserializer{TSerializable}"/> for a type that you do not own or that does not implement <see cref="IDeserializable{TSelf}"/>.
    /// </summary>
    /// <param name="constructFromSerializationData">The delegate which constructs the object from the data that has been deserialized.</param>
    public IByteDeserializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] Func<ISerializationInfoGetter, TSerializable?> constructFromSerializationData);
    /// <summary>
    /// Configure the <see cref="IByteDeserializer{TSerializable}"/> for a type that you do not own or that does not implement <see cref="IDeserializable{TSelf}"/>.
    /// </summary>
    /// <param name="strategy">The strategy to use when serializing this type.</param>
    public IByteDeserializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] IDeserializationStrategy<Byte[], TSerializable> strategy);

    /// <summary>
    /// Configure the <see cref="IByteDeserializer{TSerializable}"/> for a type that does implement <see cref="IDeserializable{TSelf}"/>.
    /// </summary>
    public IByteDeserializerDefaultStrategyAppender<TSerializable> ConfigureForOwnedType<TSerializable>()
        where TSerializable : IDeserializable<TSerializable>;
}
