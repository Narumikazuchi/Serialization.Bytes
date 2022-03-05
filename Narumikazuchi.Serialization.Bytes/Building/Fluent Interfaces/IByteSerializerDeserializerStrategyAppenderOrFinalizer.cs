namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteSerializerDeserializer{TSerializable}"/> to use specified strategies for serialization or finalizes the configuration.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> :
    IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Finalizes the configuration and returns the configured <see cref="IByteSerializerDeserializer{TSerializable}"/>.
    /// </summary>
    /// <returns>The configured <see cref="IByteSerializerDeserializer{TSerializable}"/></returns>
    public IByteSerializerDeserializer<TSerializable> Create();
}
