namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Represents the functionality of both an <see cref="ISerializer{TSerializable}"/> and <see cref="IDeserializer{TSerializable}"/>,
/// which will serialize the given objects into an array of <see cref="Byte"/>.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteSerializerDeserializer<TSerializable> :
    IByteSerializer<TSerializable>,
    IByteDeserializer<TSerializable>
{ }