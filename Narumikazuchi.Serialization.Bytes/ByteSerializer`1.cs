using System.Diagnostics.Metrics;

namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Represents the functionality to serialize an object of type <typeparamref name="TSerializable"/> into an array of <see cref="Byte"/>[].
/// </summary>
/// <remarks>
/// The serializer contains integrated strategies for the following types: <see cref="Boolean"/>, <see cref="Byte"/>,
/// <see cref="Char"/>, <see cref="DateOnly"/>, <see cref="DateTime"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Guid"/>,
/// <see cref="Half"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="IntPtr"/>, <see cref="SByte"/>,
/// <see cref="Single"/>, <see cref="String"/>, <see cref="TimeOnly"/>, <see cref="TimeSpan"/>, <see cref="UInt16"/>, <see cref="UInt32"/>,
/// <see cref="UInt64"/>, <see cref="UIntPtr"/>.<para/>
/// You can also override the behavior of any of these strategies by providing a strategy with a higher <see cref="ISerializationStrategy{TReturn}.Priority"/>
/// or using an <see cref="Action{T1, T2}"/>.
/// </remarks>
public sealed partial class ByteSerializer<TSerializable>
{
    /// <summary>
    /// Creates a new <see cref="ByteSerializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="ByteSerializer{TSerializable}"/> this method creates requires the type <typeparamref name="TSerializable"/>
    /// to implement the <see cref="ISerializable"/> interface or be of any of the following types: (<see cref="Boolean"/>, <see cref="Byte"/>,
    /// <see cref="Char"/>, <see cref="DateOnly"/>, <see cref="DateTime"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Guid"/>,
    /// <see cref="Half"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="IntPtr"/>, <see cref="SByte"/>,
    /// <see cref="Single"/>, <see cref="String"/>, <see cref="TimeOnly"/>, <see cref="TimeSpan"/>, <see cref="UInt16"/>, <see cref="UInt32"/>,
    /// <see cref="UInt64"/>, <see cref="UIntPtr"/>).
    /// If you want to serialize any other type, use the <see cref="Create(Action{TSerializable, WriteableSerializationInfo}, Action{MemberRegister})"/>
    /// method instead.
    /// </remarks>
    /// <returns>A new <see cref="ByteSerializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.</returns>
    /// <exception cref="NotAllowed"/>
    public static ByteSerializer<TSerializable> Create() =>
        Create(Array.Empty<KeyValuePair<Type, ISerializationStrategy<Byte[]>>>());
    /// <summary>
    /// Creates a new <see cref="ByteSerializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <param name="strategies">An <see cref="IEnumerable{T}"/> containing all needed strategies for serializing the type <typeparamref name="TSerializable"/>.</param>
    /// <remarks>
    /// The <see cref="ByteSerializer{TSerializable}"/> this method creates requires the type <typeparamref name="TSerializable"/>
    /// to implement the <see cref="ISerializable"/> interface or be of any of the following types: (<see cref="Boolean"/>, <see cref="Byte"/>,
    /// <see cref="Char"/>, <see cref="DateOnly"/>, <see cref="DateTime"/>, <see cref="Decimal"/>, <see cref="Double"/>, <see cref="Guid"/>,
    /// <see cref="Half"/>, <see cref="Int16"/>, <see cref="Int32"/>, <see cref="Int64"/>, <see cref="IntPtr"/>, <see cref="SByte"/>,
    /// <see cref="Single"/>, <see cref="String"/>, <see cref="TimeOnly"/>, <see cref="TimeSpan"/>, <see cref="UInt16"/>, <see cref="UInt32"/>,
    /// <see cref="UInt64"/>, <see cref="UIntPtr"/>).
    /// If you want to serialize any other type, either use the <see cref="Create(Action{TSerializable, WriteableSerializationInfo}, Action{MemberRegister})"/>
    /// method or implement a strategy for the specified type <typeparamref name="TSerializable"/> by implementing the <see cref="ISerializationStrategy{TReturn, TInput}"/>
    /// interface.
    /// </remarks>
    /// <returns>A new <see cref="ByteSerializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.</returns>
    /// <exception cref="NotAllowed"/>
    public static ByteSerializer<TSerializable> Create<TStrategies>([DisallowNull] TStrategies strategies)
        where TStrategies : IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>>
    {
        ArgumentNullException.ThrowIfNull(strategies);

        KeyValuePair<Type, ISerializationStrategy<Byte[]>>[] all = IntegratedSerializationStrategies.AllSerialization.Concat(strategies)
                                                                                                                     .Where(x => x.Key is not null &&
                                                                                                                                 x.Value is not null)
                                                                                                                     .ToArray();

        if (!CanSerialize(type: typeof(TSerializable),
                          strategies: all))
        {
            throw new NotAllowed(NOT_SERIALIZABLE_TYPE);
        }
        else
        {
            Dictionary<Type, MemberRegister> map = CreateMemberMap(type: typeof(TSerializable),
                                                                   strategies: all);
            return new(strategies: all,
                       register: map);
        }
    }
    public static ByteSerializer<TSerializable> Create([DisallowNull] Action<TSerializable, WriteableSerializationInfo> dataGetter,
                                                       [DisallowNull] Action<MemberRegister> memberRegistration) =>
        Create(strategies: Array.Empty<KeyValuePair<Type, ISerializationStrategy<Byte[]>>>(),
               dataGetter: dataGetter,
               memberRegistration: memberRegistration);
    public static ByteSerializer<TSerializable> Create<TStrategies>([DisallowNull] TStrategies strategies,
                                                                    [DisallowNull] Action<TSerializable, WriteableSerializationInfo> dataGetter,
                                                                    [DisallowNull] Action<MemberRegister> memberRegistration)
        where TStrategies : IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>>
    {
        ArgumentNullException.ThrowIfNull(strategies);
        ArgumentNullException.ThrowIfNull(dataGetter);
        ArgumentNullException.ThrowIfNull(memberRegistration);

        Dictionary<Type, MemberRegister> map = typeof(TSerializable).BuildMemberTypeRegister(memberRegistration);
        return new(strategies: IntegratedSerializationStrategies.AllSerialization.Concat(strategies),
                   register: map,
                   dataGetter: dataGetter);
    }
}

// Non-Public
partial class ByteSerializer<TSerializable>
{
    static ByteSerializer()
    {
        s_InfoFromSerializableMethod = typeof(SerializationInfo).GetMethod(name: nameof(SerializationInfo.CreateFromSerializable),
                                                                           bindingAttr: BindingFlags.Public | BindingFlags.Static)!;
    }

    internal ByteSerializer(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies,
                            Dictionary<Type, MemberRegister> register)
    {
        m_Strategies = ReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>, EqualityComparer<Type>>.CreateFrom(items: strategies.Where(x => x.Key is not null &&
                                                                                                                                                x.Value is not null),
                                                                                                                   equalityComparer: EqualityComparer<Type>.Default);
        m_TypeRegister = register;
        m_MemberRegister = register.SelectMany(x => x.Value)
                                   .Select(x => x.Key)
                                   .ToArray();
        this.RegisteredStrategies = ReadOnlyList<Type>.CreateFrom(strategies.Select(x => x.Key));
    }
    internal ByteSerializer(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies,
                            Dictionary<Type, MemberRegister> register,
                            Action<TSerializable, WriteableSerializationInfo> dataGetter) :
        this(strategies: strategies,
             register: register)
    {
        m_DataGetter = dataGetter;
    }

    private UInt64 SerializeInternal<TStream>(TStream stream,
                                              Option<TSerializable> graph)
        where TStream : IWriteableStream
    {
        Option<ReadOnlySerializationInfo> info = null;
        Option<Type> type = graph.Map(x => x!.GetType());

        if (!graph.HasValue)
        {
            info = SerializationInfo.CreateFromType(type: typeof(TSerializable),
                                                    isNull: true)
                                    .Seal();
        }
        else if (m_DataGetter is not null)
        {
            info = (ReadOnlySerializationInfo)SerializationInfo.CreateFromAny(from: graph,
                                                                              writer: m_DataGetter);
        }
        else if (type.Map(x => x.IsSerializable()))
        {
            MethodInfo method = s_InfoFromSerializableMethod.MakeGenericMethod(type!);
            info = (ReadOnlySerializationInfo)method.Invoke(obj: null,
                                                            parameters: new Object?[] { graph })!;

        }
        else if (type.Map(x => x.IsTypeEnumerable()))
        {
            graph.TryGetValue(out TSerializable? serializable);
            return this.SerializeAsEnumerable(stream: stream,
                                              graph: serializable!);
        }

        if (info.HasValue)
        {
            info.TryGetValue(out ReadOnlySerializationInfo? states);
            __Header header = this.ConfigureHeader(states!);
            return this.SerializeWithHeader(stream: stream,
                                            info: states!,
                                            header: header,
                                            properties: header.Properties);
        }
        else
        {
            return this.SerializeWithStrategy(stream: stream,
                                              graph: graph!);
        }
    }

    private __Header ConfigureHeader(ReadOnlySerializationInfo info)
    {
        __Header header = new();
        if (info.IsNull)
        {
            header.Flags |= __HeaderFlags.IsNull;
        }
        else
        {
            header.Flags |= __HeaderFlags.NotNull;
        }
        header.RegisterNames(m_MemberRegister);
        header.RegisterTypes(m_TypeRegister);
        header.TypeId = header.TypeDictionary[info.Type].Id;

        return header;
    }
    private __Header ConfigureHeader(Type type,
                                     Boolean isNull)
    {
        __Header header = new();
        if (isNull)
        {
            header.Flags |= __HeaderFlags.IsNull;
        }
        else
        {
            header.Flags |= __HeaderFlags.NotNull;
        }
        header.RegisterNames(m_MemberRegister);
        header.RegisterTypes(m_TypeRegister);
        header.TypeId = header.TypeDictionary[type].Id;

        return header;
    }

    private UInt64 SerializeWithHeader<TStream>(TStream stream,
                                                __Header header,
                                                List<__HeaderObjectProperty> properties,
                                                ReadOnlySerializationInfo info,
                                                Boolean writeHeader = true)
        where TStream : IWriteableStream
    {
        using __InMemoryStream body = new();

        foreach (MemberState member in info)
        {
            this.SerializeMember(header: header,
                                 properties: properties,
                                 body: body,
                                 member: member);
        }

        body.Position = 0;

        UInt64 written = 0;
        if (writeHeader)
        {
            using __InMemoryStream head = header.AsMemory(body.Length);
            head.CopyTo(stream);
            written += (UInt64)head.Length;
        }
        body.CopyTo(stream);
        written += (UInt64)body.Length;

        return written;
    }

    private UInt64 SerializeWithStrategy<TStream>(TStream stream,
                                                  TSerializable graph)
        where TStream : IWriteableStream
    {
        Type type = graph!.GetType();
        __Header header = this.ConfigureHeader(type: type,
                                               isNull: false);

        using __InMemoryStream body = new();
        ISerializationStrategy<Byte[]> strategy = m_Strategies.Where(x => x.Value.CanBeAppliedTo(type))
                                                              .Select(x => x.Value)
                                                              .OrderByDescending(x => x.Priority)
                                                              .First();

        Byte[] bytes = strategy.Serialize(graph);
        body.Write(bytes);

        body.Position = 0;
        if (body.Length == 0)
        {
            InvalidOperationException exception = new(message: COULD_NOT_SERIALIZE_TYPE);
            exception.Data.Add(key: "Typename",
                               value: type.FullName);
            throw exception;
        }

        UInt64 written = 0;
        using __InMemoryStream head = header.AsMemory(body.Length);
        head.CopyTo(stream);
        written += (UInt64)head.Length;
        body.CopyTo(stream);
        written += (UInt64)body.Length;

        return written;
    }

    private UInt64 SerializeAsEnumerable<TStream>(TStream stream,
                                                  TSerializable graph)
        where TStream : IWriteableStream
    {
        Type type = graph!.GetType();
        __Header header = this.ConfigureHeader(type: type,
                                               isNull: false);
        using __InMemoryStream body = new();

        this.SerializeAsEnumerable(header: header,
                                   properties: header.Properties,
                                   body: body,
                                   name: "array<$>",
                                   enumerable: graph);

        body.Position = 0;
        //if (body.Length == 0)
        //{
        //    InvalidOperationException exception = new(message: COULD_NOT_SERIALIZE_TYPE);
        //    exception.Data.Add(key: "Typename",
        //                       value: type.FullName);
        //    throw exception;
        //}

        UInt64 written = 0;
        using __InMemoryStream head = header.AsMemory(body.Length);
        head.CopyTo(stream);
        written += (UInt64)head.Length;
        body.CopyTo(stream);
        written += (UInt64)body.Length;

        return written;
    }

    private void SerializeMember(__Header header,
                                 List<__HeaderObjectProperty> properties,
                                 __InMemoryStream body,
                                 MemberState member)
    {
        if (member.Value is null)
        {
            __HeaderFlags flags = __HeaderFlags.IsNull;
            if (member.Name.Contains("<$>"))
            {
                flags |= __HeaderFlags.IsEnumerated;
            }

            properties.Add(new()
            {
                Flags = flags,
                NameId = header.NameDictionary[member.Name].Id,
                TypeId = header.TypeDictionary[member.MemberType].Id,
                Position = body.Position,
                Length = 0L,
                Properties = new()
            });
        }
        else
        {
            if (this.TrySerializeMemberWithStrategy(header: header,
                                                    properties: properties,
                                                    body: body,
                                                    member: member))
            {
                return;
            }
            else if (this.TrySerializeMemberThroughInterface(header: header,
                                                             properties: properties,
                                                             body: body,
                                                             member: member))
            {
                return;
            }
            else if (this.TrySerializeAsEnumerable(header: header,
                                                   properties: properties,
                                                   body: body,
                                                   member: member))
            {
                return;
            }
            else
            {
                InvalidOperationException exception = new(message: COULD_NOT_SERIALIZE_TYPE);
                exception.Data.Add(key: "Typename",
                                   value: member.MemberType.FullName);
                List<String> values = new();
                foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> kv in m_Strategies)
                {
                    String value = $"key: {kv.Key.FullName};";
                    values.Add(value);
                }
                exception.Data.Add(key: "Strategies",
                                   value: values);
                throw exception;
            }
        }
    }

    private Boolean TrySerializeMemberWithStrategy(__Header header,
                                                   List<__HeaderObjectProperty> properties,
                                                   __InMemoryStream body,
                                                   MemberState member)
    {
        ISerializationStrategy<Byte[]>? strategy = m_Strategies.Where(x => x.Value.CanBeAppliedTo(member.MemberType))
                                                               .Select(x => x.Value)
                                                               .OrderByDescending(x => x.Priority)
                                                               .FirstOrDefault();

        if (strategy is null)
        {
            return false;
        }
        else
        {
            __HeaderObjectProperty property = SerializeWithStrategy(body: body,
                                                                    header: header,
                                                                    data: member,
                                                                    strategy: strategy);

            properties.Add(property);
            return true;
        }
    }

    private Boolean TrySerializeMemberThroughInterface(__Header header,
                                                       List<__HeaderObjectProperty> properties,
                                                       __InMemoryStream body,
                                                       MemberState member)
    {
        if (member.MemberType.IsSerializable())
        {
            __HeaderObjectProperty property = this.SerializeThroughInterface(body: body,
                                                                             header: header,
                                                                             data: member);

            properties.Add(property);
            return true;
        }
        else
        {
            return false;
        }
    }

    private Boolean TrySerializeAsEnumerable(__Header header,
                                             List<__HeaderObjectProperty> properties,
                                             __InMemoryStream body,
                                             MemberState member)
    {
        if (member.MemberType.IsTypeEnumerable())
        {
            String itemName = $"{member.Name}<$>";
            header.RegisterNames(itemName);

            Type enumerableType = member.MemberType.InferEnumerableElementType();
            Int64 position = body.Position;
            List<__HeaderObjectProperty> contents = new();

            IEnumerable items = (IEnumerable)member.Value!;
            // Maybe unnessecary?
            //body.Write(BitConverter.GetBytes(items.Count()));
            foreach (Object item in items)
            {
                MemberState memberState = MemberState.CreateFromObject(name: itemName,
                                                                       @object: item,
                                                                       type: enumerableType);
                this.SerializeMember(header: header,
                                     properties: contents,
                                     body: body,
                                     member: memberState);
            }

            __HeaderFlags flags = __HeaderFlags.NotNull;
            if (member.Name.Contains("<$>"))
            {
                flags |= __HeaderFlags.IsEnumerated;
            }

            properties.Add(new()
            {
                Flags = flags,
                NameId = header.NameDictionary[member.Name].Id,
                TypeId = header.TypeDictionary[member.MemberType].Id,
                Position = position,
                Length = body.Position - position,
                Properties = contents
            });
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SerializeAsEnumerable(__Header header,
                                       List<__HeaderObjectProperty> properties,
                                       __InMemoryStream body,
                                       String name,
                                       Option<Object> enumerable)
    {
        header.RegisterNames(name);

        Option<Type> enumerableType = enumerable.Map(x => x.GetType().InferEnumerableElementType());
        Option<IEnumerable> items = enumerable.Map(x => (x as IEnumerable)!);
        Int64 position = body.Position;
        enumerableType.TryGetValue(out Type? enumType);

        if (items.TryGetValue(out IEnumerable? objects))
        {
            foreach (Object item in objects)
            {
                MemberState memberState = MemberState.CreateFromObject(name: name,
                                                                       @object: item,
                                                                       type: enumType!);
                this.SerializeMember(header: header,
                                     properties: properties,
                                     body: body,
                                     member: memberState);
            }
        }
    }

    private __HeaderObjectProperty SerializeThroughInterface(__InMemoryStream body,
                                                             __Header header,
                                                             MemberState data)
    {
        Int64 position = body.Position;
        ByteSerializer<Object> serializer = new(m_Strategies, m_TypeRegister);
        MethodInfo method = s_InfoFromSerializableMethod.MakeGenericMethod(data.MemberType);
        ReadOnlySerializationInfo info = (ReadOnlySerializationInfo)method.Invoke(obj: null,
                                                                                  parameters: new Object?[] { data.Value })!;
        using __InMemoryStream temp = new();

        List<__HeaderObjectProperty> properties = new();
        serializer.SerializeWithHeader(stream: temp,
                                       header: header,
                                       properties: properties,
                                       info: info,
                                       writeHeader: false);
        temp.Position = 0;
        temp.CopyTo(body);

        __HeaderFlags flags = __HeaderFlags.NotNull;
        if (data.Name.Contains("<$>"))
        {
            flags |= __HeaderFlags.IsEnumerated;
        }

        return new()
        {
            Flags = flags,
            NameId = header.NameDictionary[data.Name].Id,
            TypeId = header.TypeDictionary[data.MemberType].Id,
            Position = position,
            Length = temp.Length,
            Properties = properties
        };
    }

    private static __HeaderObjectProperty SerializeWithStrategy(__InMemoryStream body,
                                                                __Header header,
                                                                MemberState data,
                                                                ISerializationStrategy<Byte[]> strategy)
    {
        Int64 position = body.Position;

        Byte[] bytes = strategy.Serialize(data.Value);
        body.Write(bytes);

        __HeaderFlags flags = __HeaderFlags.NotNull;
        if (data.Name.Contains("<$>"))
        {
            flags |= __HeaderFlags.IsEnumerated;
        }

        return new()
        {
            Flags = flags,
            NameId = header.NameDictionary[data.Name].Id,
            TypeId = header.TypeDictionary[data.MemberType].Id,
            Position = position,
            Length = bytes.Length,
            Properties = new()
        };
    }

    private static Boolean CanSerialize(Type type,
                                        KeyValuePair<Type, ISerializationStrategy<Byte[]>>[] strategies)
    {
        if (type.IsSerializable() ||
            strategies.Any(x => x.Value.CanBeAppliedTo(type)))
        {
            return true;
        }
        else if (type.IsTypeEnumerable() &&
                 CanSerialize(type: type.InferEnumerableElementType(),
                              strategies: strategies))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private static Dictionary<Type, MemberRegister> CreateMemberMap(Type type,
                                                                    KeyValuePair<Type, ISerializationStrategy<Byte[]>>[] strategies)
    {
        if (type.IsSerializable())
        {
            return type.BuildMemberTypeRegister();
        }
        else if (strategies.Any(x => x.Value.CanBeAppliedTo(type)))
        {
            return new()
            {
                { type, new() }
            };
        }
        else if (type.IsTypeEnumerable())
        {
            Dictionary<Type, MemberRegister> map = CreateMemberMap(type: type.InferEnumerableElementType(),
                                                                   strategies: strategies);
            map.Add(key: type,
                    value: new());
            return map;
        }
        else
        {
            throw new ImpossibleState();
        }
    }

    internal readonly ReadOnlyDictionary<Type, ISerializationStrategy<Byte[]>, EqualityComparer<Type>> m_Strategies;
    internal readonly Action<TSerializable, WriteableSerializationInfo>? m_DataGetter;
    internal readonly Dictionary<Type, MemberRegister> m_TypeRegister;
    internal readonly String[] m_MemberRegister;
    internal static readonly MethodInfo s_InfoFromSerializableMethod;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal const String STREAM_DOES_NOT_SUPPORT_WRITING = "The specified stream does not support writing.";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal const String STREAM_DOES_NOT_SUPPORT_SEEKING = "The specified stream does not support seeking.";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal const String COULD_NOT_SERIALIZE_TYPE = "Couldn't serialize type. (Are you missing a serialization strategy? Consider implementing the ISerializable interface, if it's a type you own.)";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal const String NOT_SERIALIZABLE_TYPE = "The specified type does not implement the ISerializable interface. This means you need to provide a data-getter and data-setter method to the Create() method.";
}

// ISerializer<T>
partial class ByteSerializer<TSerializable> : ISerializer<TSerializable>
{
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize([DisallowNull] Stream stream,
                            [AllowNull] TSerializable? graph,
                            Int64 offset,
                            SerializationFinishAction actionAfter)
    {
        if (!stream.CanWrite)
        {
            throw new InvalidOperationException(message: STREAM_DOES_NOT_SUPPORT_WRITING);
        }
        if (offset > -1 &&
            !stream.CanSeek)
        {
            throw new InvalidOperationException(message: STREAM_DOES_NOT_SUPPORT_SEEKING);
        }

        if (offset > -1)
        {
            stream.Seek(offset: offset,
                        origin: SeekOrigin.Begin);
        }

        UInt64 result = this.SerializeInternal(stream: stream.AsWriteableStream(),
                                               graph: graph);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Position = 0;
        }
        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.DisposeStream))
        {
            stream.Dispose();
        }

        return result;
    }
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public UInt64 Serialize<TStream>([DisallowNull] TStream stream,
                                     [AllowNull] TSerializable? graph,
                                     Int64 offset,
                                     SerializationFinishAction actionAfter)
        where TStream : IWriteableStream
    {
        if (offset > -1)
        {
            if (stream is INonContinousStream nonContinousStream)
            {
                nonContinousStream.Position = offset;
            }
            else if (stream is ISeekableStream seekableStream)
            {
                seekableStream.Position = offset;
            }
            else
            {
                throw new NotSupportedException("The stream does not support an offset.");
            }
        }

        UInt64 result = this.SerializeInternal(stream: stream,
                                               graph: graph);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            if (stream is INonContinousStream nonContinousStream)
            {
                nonContinousStream.Position = 0;
            }
            else if (stream is ISeekableStream seekableStream)
            {
                seekableStream.Position = 0;
            }
            else
            {
                throw new NotSupportedException("The stream does not support moving.");
            }
        }
        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.DisposeStream))
        {
            stream.Dispose();
        }

        return result;
    }

    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public Boolean TrySerialize([DisallowNull] Stream stream,
                                [AllowNull] TSerializable? graph,
                                Int64 offset,
                                out UInt64 written,
                                SerializationFinishAction actionAfter)
    {
        if (stream is null ||
            !stream.CanWrite ||
            (offset > -1 &&
            !stream.CanSeek))
        {
            written = 0;
            return false;
        }

        if (offset > -1)
        {
            stream.Seek(offset: offset,
                        origin: SeekOrigin.Begin);
        }
        try
        {
            written = this.SerializeInternal(stream: stream.AsWriteableStream(),
                                             graph: graph);
        }
        catch
        {
            written = 0UL;
            return false;
        }

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Position = 0;
        }
        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.DisposeStream))
        {
            stream.Dispose();
        }

        return true;
    }
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    public Boolean TrySerialize<TStream>([DisallowNull] TStream stream,
                                         [AllowNull] TSerializable? graph,
                                         Int64 offset,
                                         out UInt64 written,
                                         SerializationFinishAction actionAfter)
        where TStream : IWriteableStream
    {
        if (offset > -1)
        {
            if (stream is INonContinousStream nonContinousStream)
            {
                nonContinousStream.Position = offset;
            }
            else if (stream is ISeekableStream seekableStream)
            {
                seekableStream.Position = offset;
            }
            else
            {
                written = 0;
                return false;
            }
        }
        try
        {
            written = this.SerializeInternal(stream: stream,
                                             graph: graph);
        }
        catch
        {
            written = 0UL;
            return false;
        }

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            if (stream is INonContinousStream nonContinousStream)
            {
                nonContinousStream.Position = 0;
            }
            else if (stream is ISeekableStream seekableStream)
            {
                seekableStream.Position = 0;
            }
        }
        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.DisposeStream))
        {
            stream.Dispose();
        }

        return true;
    }

    /// <inheritdoc/>
    public ReadOnlyList<Type> RegisteredStrategies { get; }
}