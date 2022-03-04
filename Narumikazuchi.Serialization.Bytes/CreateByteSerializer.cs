namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// A builder class to configure a serializer and/or deserializer for you needs.
/// </summary>
public static class CreateByteSerializer
{
    /// <summary>
    /// Configure the serializer for a type that you do not own or that does not implement <see cref="IDeserializable{TSelf}"/> or <see cref="ISerializable"/>, 
    /// depending on the later defined type of serialization.
    /// </summary>
    public static ISerializationForeignTypeConfigurator<TSerializable> ConfigureForForeignType<TSerializable>() =>
        new __ByteSerializerBuilder<TSerializable>();

    /// <summary>
    /// Configure the serializer for a type that does implement <see cref="IDeserializable{TSelf}"/> or <see cref="ISerializable"/>, 
    /// depending on the later defined type of serialization.
    /// </summary>
    public static ISerializationOwnedTypeConfigurator<TSerializable> ConfigureForOwnedType<TSerializable>() =>
        new __ByteSerializerBuilder<TSerializable>();
}
