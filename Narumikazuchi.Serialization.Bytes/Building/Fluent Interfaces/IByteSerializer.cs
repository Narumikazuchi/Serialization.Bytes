namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Represents the functionality of an <see cref="ISerializer{TSerializable}"/>,
/// which will serialize the given objects into an array of <see cref="Byte"/>.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteSerializer<TSerializable> : 
    ISerializer<TSerializable>
{ }