namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteDeserializer{TSerializable}"/> to use specified strategies for deserialization or finalizes the configuration.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> :
    IByteDeserializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Finalizes the configuration and returns the configured <see cref="IByteDeserializer{TSerializable}"/>.
    /// </summary>
    /// <returns>The configured <see cref="IByteDeserializer{TSerializable}"/></returns>
    public IByteDeserializer<TSerializable> Construct();
}
