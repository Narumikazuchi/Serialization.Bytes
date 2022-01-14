namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Configures a <see cref="IByteSerializerDeserializer{TSerializable}"/> to use specified strategies for serialization.
/// </summary>
[Browsable(false)]
[EditorBrowsable(EditorBrowsableState.Never)]
public interface IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> :
    IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    /// <summary>
    /// Configures the <see cref="IByteSerializerDeserializer{TSerializable}"/> to use the integrated default strategies for primitive types.
    /// </summary>
    /// <remarks>
    /// Default strategies will not override any prior custom strategies that have been configured. Furthermore every strategy targeting one
    /// of the types that the default strategies target will overwrite the default strategy with the custom one.<para/>
    /// The default strategies include strategies for the following types: <see cref="Boolean"/>, <see cref="Byte"/>, <see cref="Char"/>,
    /// <see cref="Double"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="IntPtr"/>, <see cref="SByte"/>,
    /// <see cref="Single"/>, <see cref="UInt16"/>, <see cref="UInt32"/>, <see cref="UInt64"/>, <see cref="UIntPtr"/>, <see cref="DateTime"/>,
    /// <see cref="DateOnly"/>, <see cref="TimeOnly"/>, <see cref="TimeSpan"/>, <see cref="Guid"/>, <see cref="Half"/>, <see cref="String"/>.
    /// </remarks>
    public IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> UseDefaultStrategies();
}
