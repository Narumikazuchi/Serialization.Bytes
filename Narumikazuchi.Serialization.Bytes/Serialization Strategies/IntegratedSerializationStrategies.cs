namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Contains all predefined strategies that are utilized by the <see cref="IByteDeserializerDefaultStrategyAppender{TSerializable}.UseDefaultStrategies"/>, <see cref="IByteSerializerDefaultStrategyAppender{TSerializable}.UseDefaultStrategies"/> and
/// <see cref="IByteSerializerDeserializerDefaultStrategyAppender{TSerializable}.UseDefaultStrategies"/> builder methods.
/// </summary>
public static partial class IntegratedSerializationStrategies
{
    /// <summary>
    /// Contains all predefined strategies in an <see cref="IReadOnlyDictionary{TKey, TValue}"/>.
    /// </summary>
    public static IReadOnlyDictionary<Type, ISerializationDeserializationStrategy<System.Byte[]>> All { get; } = new Dictionary<Type, ISerializationDeserializationStrategy<System.Byte[]>>()
    {
        { typeof(System.Boolean),   Boolean.Reference },
        { typeof(System.Byte),      Byte.Reference },
        { typeof(System.Char),      Char.Reference },
        { typeof(System.Double),    Double.Reference },
        { typeof(System.Decimal),   Decimal.Reference },
        { typeof(System.Int16),     Int16.Reference },
        { typeof(System.Int32),     Int32.Reference },
        { typeof(System.Int64),     Int64.Reference },
        { typeof(System.IntPtr),    IntPtr.Reference },
        { typeof(System.SByte),     SByte.Reference },
        { typeof(System.Single),    Single.Reference },
        { typeof(System.UInt16),    UInt16.Reference },
        { typeof(System.UInt32),    UInt32.Reference },
        { typeof(System.UInt64),    UInt64.Reference },
        { typeof(System.UIntPtr),   UIntPtr.Reference },
        { typeof(System.DateTime),  DateTime.Reference },
        { typeof(System.DateOnly),  DateOnly.Reference },
        { typeof(System.TimeSpan),  TimeSpan.Reference },
        { typeof(System.TimeOnly),  TimeOnly.Reference },
        { typeof(System.Guid),      Guid.Reference },
        { typeof(System.Half),      Half.Reference },
        { typeof(System.String),    String.Reference }
    };
}