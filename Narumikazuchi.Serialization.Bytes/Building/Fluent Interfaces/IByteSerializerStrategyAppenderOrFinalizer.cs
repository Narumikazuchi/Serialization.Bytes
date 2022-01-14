namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteSerializer{TSerializable}"/> to use specified strategies for serialization or finalizes the configuration.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteSerializerStrategyAppenderOrFinalizer<TSerializable> :
    IByteSerializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Finalizes the configuration and returns the configured <see cref="IByteSerializer{TSerializable}"/>.
    /// </summary>
    /// <returns>The configured <see cref="IByteSerializer{TSerializable}"/></returns>
    public IByteSerializer<TSerializable> Construct();
}
