namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures the <see cref="IByteSerializer{TSerializable}"/> for a specific type.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteSerializerTypeConfigurator
{
    /// <summary>
    /// Configure the <see cref="IByteSerializer{TSerializable}"/> for a type that you do not own or that does not implement <see cref="ISerializable"/>.
    /// </summary>
    /// <param name="getSerializationData">The delegate which fetches the data to store.</param>
    public IByteSerializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] Action<TSerializable?, ISerializationInfoAdder> getSerializationData);
    /// <summary>
    /// Configure the <see cref="IByteSerializer{TSerializable}"/> for a type that you do not own or that does not implement <see cref="ISerializable"/>.
    /// </summary>
    /// <param name="strategy">The strategy to use when serializing this type.</param>
    public IByteSerializerDefaultStrategyAppender<TSerializable> ConfigureForForeignType<TSerializable>([DisallowNull] ISerializationStrategy<Byte[], TSerializable> strategy);

    /// <summary>
    /// Configure the <see cref="IByteSerializer{TSerializable}"/> for a type that does implement <see cref="ISerializable"/>.
    /// </summary>
    public IByteSerializerDefaultStrategyAppender<TSerializable> ConfigureForOwnedType<TSerializable>()
        where TSerializable : ISerializable;
}
