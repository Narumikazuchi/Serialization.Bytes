namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures the <see cref="IByteSerializer{TSerializable}"/> for a specific type.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface ISerializationOwnedTypeConfigurator<TSerializable>
{
    /// <summary>
    /// Builds the serializer for deserialization.
    /// </summary>
    public IByteDeserializerDefaultStrategyAppender<TSerializable> ForDeserialization();

    /// <summary>
    /// Builds the serializer for serialization.
    /// </summary>
    public IByteSerializerDefaultStrategyAppender<TSerializable> ForSerialization();

    /// <summary>
    /// Builds the serializer for deserialization and serialization.
    /// </summary>
    public IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ForSerializationAndDeserialization();
}
