using Narumikazuchi.Collections;

namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Represents the functionality to serialize an object of type <typeparamref name="TSerializable"/> into an array of <see cref="Byte"/>[].
/// Also represents the functionality to deserialize an an array of <see cref="Byte"/>[] back into an object of type <typeparamref name="TSerializable"/>.
/// </summary>
public sealed partial class ByteSerializerDeserializer<TSerializable> : ISerializerDeserializer<TSerializable>
{
    /// <summary>
    /// Creates a new <see cref="ByteSerializerDeserializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="ByteSerializerDeserializer{TSerializable}"/> this method creates by default does not use any of the integrated
    /// strategies while also requiring the type <typeparamref name="TSerializable"/> to implement the <see cref="ISerializable"/>
    /// interface. If you want to serialize a type that doesn't implement the <see cref="ISerializable"/> interface, use
    /// the <see cref="Create(Action{TSerializable, WriteableSerializationInfo}, Func{ReadOnlySerializationInfo, TSerializable?}, Action{MemberRegister})"/>
    /// method instead.
    /// </remarks>
    /// <returns>A new <see cref="ByteSerializerDeserializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.</returns>
    /// <exception cref="NotAllowed"/>
    public static ByteSerializerDeserializer<TSerializable> Create() =>
        Create(Array.Empty<KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>>>());
    /// <summary>
    /// Creates a new <see cref="ByteSerializerDeserializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="strategies">An <see cref="IEnumerable{T}"/> with additional strategies that will be used on top of the integrated ones.</param>
    /// <remarks>
    /// The <see cref="ByteSerializerDeserializer{TSerializable}"/> this method creates by default does not use any of the integrated
    /// strategies while also requiring the type <typeparamref name="TSerializable"/> to implement the <see cref="ISerializable"/>
    /// interface. If you want to serialize a type that doesn't implement the <see cref="ISerializable"/> interface, use
    /// the <see cref="Create(Action{TSerializable, WriteableSerializationInfo}, Func{ReadOnlySerializationInfo, TSerializable?}, Action{MemberRegister})"/>
    /// method instead.
    /// </remarks>
    /// <returns>A new <see cref="ByteSerializerDeserializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.</returns>
    /// <exception cref="NotAllowed"/>
    public static ByteSerializerDeserializer<TSerializable> Create<TStrategies>([DisallowNull] TStrategies strategies)
        where TStrategies : IEnumerable<KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>>>
    {
        ArgumentNullException.ThrowIfNull(strategies);

        return new(strategies: strategies);
    }
    public static ByteSerializerDeserializer<TSerializable> Create([DisallowNull] Action<TSerializable, WriteableSerializationInfo> dataGetter,
                                                                   [DisallowNull] Func<ReadOnlySerializationInfo, TSerializable?> dataSetter,
                                                                   [DisallowNull] Action<MemberRegister> memberRegistration)
    {
        ArgumentNullException.ThrowIfNull(dataGetter);
        ArgumentNullException.ThrowIfNull(dataSetter);

        return new(strategies: IntegratedSerializationStrategies.All,
                   dataGetter: dataGetter,
                   dataSetter: dataSetter,
                   memberRegistration: memberRegistration);
    }
    public static ByteSerializerDeserializer<TSerializable> Create<TStrategies>([DisallowNull] TStrategies strategies,
                                                                                [DisallowNull] Action<TSerializable, WriteableSerializationInfo> dataGetter,
                                                                                [DisallowNull] Func<ReadOnlySerializationInfo, TSerializable?> dataSetter,
                                                                                [DisallowNull] Action<MemberRegister> memberRegistration)
        where TStrategies : IEnumerable<KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>>>
    {
        ArgumentNullException.ThrowIfNull(strategies);
        ArgumentNullException.ThrowIfNull(dataGetter);
        ArgumentNullException.ThrowIfNull(dataSetter);

        return new(strategies: strategies,
                   dataGetter: dataGetter,
                   dataSetter: dataSetter,
                   memberRegistration: memberRegistration);
    }

    /// <inheritdoc/>
    public ReadOnlyList<Type> RegisteredStrategies =>
        m_Serializer.RegisteredStrategies;
}

partial class ByteSerializerDeserializer<TSerializable>
{
    internal ByteSerializerDeserializer(IEnumerable<KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>>> strategies)
    {
        IEnumerable<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>> deserialize = strategies.Select(x => new KeyValuePair<Type, IDeserializationStrategy<Byte[]>>(key: x.Key,
                                                                                                                                                                        value: x.Value));
        IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> serialize = strategies.Select(x => new KeyValuePair<Type, ISerializationStrategy<Byte[]>>(key: x.Key,
                                                                                                                                                                  value: x.Value));

        m_Deserializer = ByteDeserializer<TSerializable>.Create(deserialize.ToArray());
        m_Serializer = ByteSerializer<TSerializable>.Create(serialize.ToArray());
    }
    internal ByteSerializerDeserializer(IEnumerable<KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>>> strategies,
                                        Func<ReadOnlySerializationInfo, TSerializable?> dataSetter,
                                        Action<TSerializable, WriteableSerializationInfo> dataGetter,
                                        Action<MemberRegister> memberRegistration)
    {
        IEnumerable<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>> deserialize = strategies.Select(x => new KeyValuePair<Type, IDeserializationStrategy<Byte[]>>(key: x.Key,
                                                                                                                                                                        value: x.Value));
        IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> serialize = strategies.Select(x => new KeyValuePair<Type, ISerializationStrategy<Byte[]>>(key: x.Key,
                                                                                                                                                                  value: x.Value));

        m_Deserializer = ByteDeserializer<TSerializable>.Create(strategies: deserialize.ToArray(),
                                                                dataSetter: dataSetter,
                                                                memberRegistration: memberRegistration);
        m_Serializer = ByteSerializer<TSerializable>.Create(strategies: serialize.ToArray(),
                                                            dataGetter: dataGetter,
                                                            memberRegistration: memberRegistration);
    }

    private readonly ByteDeserializer<TSerializable> m_Deserializer;
    private readonly ByteSerializer<TSerializable> m_Serializer;
}

partial class ByteSerializerDeserializer<TSerializable> : IDeserializer<TSerializable>
{
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      Int64 offset,
                                      out UInt64 read,
                                      SerializationFinishAction actionAfter) =>
        m_Deserializer.Deserialize(stream: stream,
                                   offset: offset,
                                   actionAfter: actionAfter,
                                   read: out read);
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize<TStream>([DisallowNull] TStream stream,
                                               Int64 offset,
                                               out UInt64 read,
                                               SerializationFinishAction actionAfter)
        where TStream : IReadableStream =>
            m_Deserializer.Deserialize(stream: stream,
                                       offset: offset,
                                       actionAfter: actionAfter,
                                       read: out read);

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  Int64 offset,
                                  out UInt64 read,
                                  SerializationFinishAction actionAfter,
                                  [AllowNull] out TSerializable? result) =>
        m_Deserializer.TryDeserialize(stream: stream,
                                      offset: offset,
                                      actionAfter: actionAfter,
                                      read: out read,
                                      result: out result);
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public Boolean TryDeserialize<TStream>([DisallowNull] TStream stream,
                                           Int64 offset,
                                           out UInt64 read,
                                           SerializationFinishAction actionAfter,
                                           [AllowNull] out TSerializable? result)
        where TStream : IReadableStream =>
            m_Deserializer.TryDeserialize(stream: stream,
                                          offset: offset,
                                          actionAfter: actionAfter,
                                          read: out read,
                                          result: out result);

}

partial class ByteSerializerDeserializer<TSerializable> : ISerializer<TSerializable>
{
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [AllowNull] TSerializable? graph,
                            Int64 offset,
                            SerializationFinishAction actionAfter) =>
        m_Serializer.Serialize(stream: stream,
                               graph: graph,
                               offset: offset,
                               actionAfter: actionAfter);
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TStream>([DisallowNull] TStream stream,
                                     [AllowNull] TSerializable? graph,
                                     Int64 offset,
                                     SerializationFinishAction actionAfter)
        where TStream : IWriteableStream =>
            m_Serializer.Serialize(stream: stream,
                                   graph: graph,
                                   offset: offset,
                                   actionAfter: actionAfter);

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                Int64 offset,
                                out UInt64 written,
                                SerializationFinishAction actionAfter) =>
        m_Serializer.TrySerialize(stream: stream,
                                  graph: graph,
                                  offset: offset,
                                  actionAfter: actionAfter,
                                  written: out written);
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public Boolean TrySerialize<TStream>([DisallowNull] TStream stream,
                                         [AllowNull] TSerializable? graph,
                                         Int64 offset,
                                         out UInt64 written,
                                         SerializationFinishAction actionAfter)
        where TStream : IWriteableStream =>
            m_Serializer.TrySerialize(stream: stream,
                                      graph: graph,
                                      offset: offset,
                                      actionAfter: actionAfter,
                                      written: out written);
}