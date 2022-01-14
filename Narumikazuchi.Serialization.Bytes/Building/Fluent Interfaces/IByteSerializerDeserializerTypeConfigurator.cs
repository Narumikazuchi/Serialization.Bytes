namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures the <see cref="IByteSerializerDeserializer{TSerializable}"/> for a specific type.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteSerializerDeserializerTypeConfigurator
{
    /// <summary>
    /// Configure the <see cref="IByteSerializerDeserializer{TSerializable}"/> for a type that you do not own or that does neither implement <see cref="ISerializable"/> nor <see cref="IDeserializable{TSelf}"/>.
    /// </summary>
    /// <param name="getSerializationData">The delegate which fetches the data to store.</param>
    /// <param name="constructFromSerializationData">The delegate which constructs the object from the data that has been deserialized.</param>
    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] Action<TSerializable?, ISerializationInfoAdder> getSerializationData,
                                                                                                                    [DisallowNull] Func<ISerializationInfoGetter, TSerializable?> constructFromSerializationData);
    /// <summary>
    /// Configure the <see cref="IByteSerializerDeserializer{TSerializable}"/> for a type that you do not own or that does neither implement <see cref="ISerializable"/> nor <see cref="IDeserializable{TSelf}"/>.
    /// </summary>
    /// <param name="strategy">The strategy to use when serializing and deserializing this type.</param>
    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] ISerializationDeserializationStrategy<Byte[], TSerializable> strategy);

    /// <summary>
    /// Configure the <see cref="IByteSerializerDeserializer{TSerializable}"/> for a type that does implement both <see cref="ISerializable"/> and <see cref="IDeserializable{TSelf}"/>.
    /// </summary>
    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ConfigureForOwnedType<TSerializable>()
        where TSerializable : IDeserializable<TSerializable>;
}