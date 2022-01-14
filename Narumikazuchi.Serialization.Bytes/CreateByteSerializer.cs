namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// A builder class to configure a serializer and/or deserializer for you needs.
/// </summary>
public static class CreateByteSerializer
{
    /// <summary>
    /// Builds an <see cref="IByteDeserializer{TSerializable}"/>.
    /// </summary>
    public static IByteDeserializerTypeConfigurator ForDeserialization() =>
        new __ConfigurationInformation();

    /// <summary>
    /// Builds an <see cref="IByteSerializer{TSerializable}"/>.
    /// </summary>
    public static IByteSerializerTypeConfigurator ForSerialization() =>
        new __ConfigurationInformation();

    /// <summary>
    /// Builds an <see cref="IByteSerializerDeserializer{TSerializable}"/>.
    /// </summary>
    public static IByteSerializerDeserializerTypeConfigurator ForSerializationAndDeserialization() =>
        new __ConfigurationInformation();
}
