namespace Narumikazuchi.Serialization.Bytes;

/// <summary>
/// Represents the functionality to deserialize an an array of <see cref="Byte"/>[] back into an object of type <typeparamref name="TSerializable"/>.
/// </summary>
public sealed partial class ByteDeserializer<TSerializable>
{
    /// <summary>
    /// Creates a new <see cref="ByteDeserializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.
    /// </summary>
    /// <remarks>
    /// The <see cref="ByteDeserializer{TSerializable}"/> this method creates requires the type <typeparamref name="TSerializable"/>
    /// to implement the <see cref="ISerializable"/>interface. If you want to serialize a type that doesn't implement the 
    /// <see cref="ISerializable"/> interface, use the <see cref="Create(Func{ReadOnlySerializationInfo, TSerializable?}, Action{MemberRegister})"/>
    /// method instead.
    /// </remarks>
    /// <returns>A new <see cref="ByteDeserializer{TSerializable}"/> object for the type <typeparamref name="TSerializable"/>.</returns>
    /// <exception cref="NotAllowed"/>
    public static ByteDeserializer<TSerializable> Create() =>
        Create(Array.Empty<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>>());
    public static ByteDeserializer<TSerializable> Create<TStrategies>([DisallowNull] TStrategies strategies)
        where TStrategies : IEnumerable<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>>
    {
        ArgumentNullException.ThrowIfNull(strategies);

        KeyValuePair<Type, IDeserializationStrategy<Byte[]>>[] all = IntegratedSerializationStrategies.AllDeserialization.Concat(strategies)
                                                                                                                         .Where(x => x.Key is not null &&
                                                                                                                                     x.Value is not null)
                                                                                                                         .ToArray();

        if (!CanDeserialize(type: typeof(TSerializable),
                            strategies: all))
        {
            throw new NotAllowed("The specified type does not implement the IDeserializable`1 interface. This means you need to provide a data-getter and data-setter method to the Create() method.");
        }
        else
        {
            Dictionary<Type, MemberRegister> map = CreateMemberMap(type: typeof(TSerializable),
                                                                   strategies: all);
            return new(strategies: all,
                       register: map);
        }
    }
    public static ByteDeserializer<TSerializable> Create([DisallowNull] Func<ReadOnlySerializationInfo, TSerializable?> dataSetter,
                                                         [DisallowNull] Action<MemberRegister> memberRegistration) =>
        Create(strategies: Array.Empty<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>>(),
               dataSetter: dataSetter,
               memberRegistration: memberRegistration);
    public static ByteDeserializer<TSerializable> Create<TStrategies>([DisallowNull] TStrategies strategies,
                                                                      [DisallowNull] Func<ReadOnlySerializationInfo, TSerializable?> dataSetter,
                                                                      [DisallowNull] Action<MemberRegister> memberRegistration)
        where TStrategies : IEnumerable<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>>
    {
        ArgumentNullException.ThrowIfNull(strategies);
        ArgumentNullException.ThrowIfNull(dataSetter);
        ArgumentNullException.ThrowIfNull(memberRegistration);

        Dictionary<Type, MemberRegister> map = typeof(TSerializable).BuildMemberTypeRegister(memberRegistration);
        return new(strategies: IntegratedSerializationStrategies.AllDeserialization.Concat(strategies),
                   register: map,
                   dataSetter: dataSetter);
    }
}

// Non-Public
partial class ByteDeserializer<TSerializable>
{
    static ByteDeserializer()
    {
        s_InfoFromSerializableMethod = typeof(SerializationInfo).GetMethod(name: nameof(SerializationInfo.CreateFromSerializable),
                                                                           bindingAttr: BindingFlags.Public | BindingFlags.Static)!;
    }

    internal ByteDeserializer(IEnumerable<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>> strategies,
                              Dictionary<Type, MemberRegister> register)
    {
        m_Strategies = ReadOnlyDictionary<Type, IDeserializationStrategy<Byte[]>, EqualityComparer<Type>>.CreateFrom(items: strategies.Where(x => x.Key is not null &&
                                                                                                                                                  x.Value is not null),
                                                                                                                     equalityComparer: EqualityComparer<Type>.Default);
        m_TypeRegister = register;
        m_MemberRegister = register.SelectMany(x => x.Value)
                                   .Select(x => x.Key)
                                   .ToArray();
        this.RegisteredStrategies = ReadOnlyList<Type>.CreateFrom(strategies.Select(x => x.Key));
    }
    internal ByteDeserializer(IEnumerable<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>> strategies,
                              Dictionary<Type, MemberRegister> register,
                              Func<ReadOnlySerializationInfo, TSerializable?> dataSetter) :
        this(strategies: strategies,
             register: register)
    {
        m_DataSetter = dataSetter;
    }

    private TSerializable? DeserializeInternal<TStream>(TStream stream,
                                                        out UInt64 read)
        where TStream : IReadableStream
    {
        read = 0;

        if (stream.Length - stream.Position < sizeof(Int64) * 3 + 1)
        {
            return default;
        }

        Int64 start = stream.Position;
        __Header header = __Header.FromStream(stream: stream,
                                              read: out read);

        if (header.Flags.HasFlag(__HeaderFlags.IsNull))
        {
            return default;
        }
        else
        {
            // Read all bytes from the body
            Byte[] body = new Byte[header.SizeOfBody];
            stream.Position = start + header.SizeOfHeader;
            stream.Read(body);
            read = (UInt64)header.SizeOfEntireObject;
            // Serialized type
            Type type = header.TypeDictionary.ElementAt((Int32)header.TypeId).Key;
            if (!type.IsAssignableTo(typeof(TSerializable)))
            {
                throw new FormatException($"Can't assign an object of type {type.Name} to the type {typeof(TSerializable).Name}.");
            }

            return this.DeserializeWithHeader(header: header,
                                              properties: header.Properties,
                                              type: type,
                                              body: body);
        }
    }

    private TSerializable? DeserializeWithHeader(__Header header,
                                                 List<__HeaderObjectProperty> properties,
                                                 Type type,
                                                 Byte[] body)
    {
        // Strategy-approach
        if (m_Strategies.ContainsKey(type))
        {
            return (TSerializable?)m_Strategies[type].Deserialize(body);
        }

        // No strategy means we do it ourselves
        ReadOnlySerializationInfo info = this.DeserializeMembers(header: header,
                                                                 properties: properties,
                                                                 type: type,
                                                                 body: body);

        // Do we have a setter?
        if (m_DataSetter is not null)
        {
            return m_DataSetter.Invoke(info);
        }
        // Do we implement the interface?
        else if (type.IsDeserializable())
        {
            MethodInfo method = this.GetType()
                                    .GetMethod(name: nameof(CreateObject),
                                               bindingAttr: BindingFlags.Static | BindingFlags.NonPublic)!
                                    .MakeGenericMethod(type);
            return (TSerializable?)method.Invoke(obj: this,
                                                 parameters: new Object[] { info });
        }
        else if (type.IsTypeEnumerable())
        {
            return (TSerializable?)CreateEnumerableType(type: type,
                                                        info: info);
        }
        // No way of transforming the members into an instance of TSerializable
        else
        {
            InvalidOperationException exception = new(message: COULD_NOT_DESERIALIZE_TYPE);
            exception.Data.Add(key: "Typename",
                               value: type.FullName);
            throw exception;
        }
    }

    private ReadOnlySerializationInfo DeserializeMembers(__Header header,
                                                         List<__HeaderObjectProperty> properties,
                                                         Type type,
                                                         Byte[] body)
    {
        WriteableSerializationInfo info = SerializationInfo.CreateFromType(type: type,
                                                                           isNull: false);

        Int32 counter = -1;

        foreach (__HeaderObjectProperty property in properties)
        {
            counter++;

            String name = header.NameDictionary.ElementAt((Int32)property.NameId).Key;
            if (property.Flags.HasFlag(__HeaderFlags.IsNull))
            {
                if (property.Flags.HasFlag(__HeaderFlags.IsEnumerated))
                {
                    Int32 markerIndex = name.LastIndexOf("$");
                    String indexedName = name[..markerIndex] + counter.ToString() + name[++markerIndex..];
                    info.SetState(memberName: indexedName,
                                  memberValue: (Object?)null);
                }
                else
                {
                    info.SetState(memberName: name,
                                  memberValue: (Object?)null);
                }
            }
            else if (property.Flags.HasFlag(__HeaderFlags.NotNull))
            {
                if (this.TryDeserializeMemberWithStrategy(header: header,
                                                          property: property,
                                                          body: body,
                                                          info: info,
                                                          counter: counter))
                {
                    continue;
                }
                else if (this.TryDeserializeMemberThroughInterface(header: header,
                                                                   property: property,
                                                                   body: body,
                                                                   info: info,
                                                                   counter: counter))
                {
                    continue;
                }
                else if (this.TryDeserializeMemberAsEnumerable(header: header,
                                                               property: property,
                                                               body: body,
                                                               info: info,
                                                               counter: counter))
                {
                    continue;
                }
                else
                {
                    InvalidOperationException exception = new(message: COULD_NOT_DESERIALIZE_TYPE);
                    exception.Data.Add(key: "Typename",
                                       value: header.TypeDictionary.ElementAt((Int32)property.TypeId).Key.Name);
                    throw exception;
                }
            }
        }

        return info.Seal();
    }

    private Boolean TryDeserializeMemberWithStrategy(__Header header,
                                                     __HeaderObjectProperty property,
                                                     Byte[] body,
                                                     WriteableSerializationInfo info,
                                                     Int32 counter)
    {
        Object member;
        String name = header.NameDictionary.ElementAt((Int32)property.NameId).Key;
        Type type = header.TypeDictionary.ElementAt((Int32)property.TypeId).Key;
        Byte[] bytes = body[(Int32)property.Position..(Int32)(property.Position + property.Length)];

        foreach (KeyValuePair<Type, IDeserializationStrategy<Byte[]>> kv in m_Strategies)
        {
            if (kv.Value.CanBeAppliedTo(type))
            {
                member = kv.Value.Deserialize(bytes)!;
                if (property.Flags.HasFlag(__HeaderFlags.IsEnumerated))
                {
                    Int32 markerIndex = name.LastIndexOf("$");
                    String indexedName = name[..markerIndex++] + counter.ToString() + name[markerIndex..];
                    info.SetState(memberName: indexedName,
                                  memberValue: member);
                }
                else
                {
                    info.SetState(memberName: name,
                                  memberValue: member);
                }
                return true;
            }
        }

        return false;
    }

    private Boolean TryDeserializeMemberThroughInterface(__Header header,
                                                         __HeaderObjectProperty property,
                                                         Byte[] body,
                                                         WriteableSerializationInfo info,
                                                         Int32 counter)
    {
        Object member;
        String name = header.NameDictionary.ElementAt((Int32)property.NameId).Key;
        Type type = header.TypeDictionary.ElementAt((Int32)property.TypeId).Key;
        Byte[] bytes = body[(Int32)property.Position..(Int32)(property.Position + property.Length)];

        if (type.IsDeserializable())
        {
            MethodInfo method = this.GetType()
                                    .GetMethod(name: nameof(DeserializeThroughInterface),
                                               bindingAttr: BindingFlags.Instance | BindingFlags.NonPublic,
                                               types: new Type[] { typeof(__Header), typeof(List<__HeaderObjectProperty>), typeof(Byte[]) })!
                                    .MakeGenericMethod(type)!;

            Object?[] parameters = new Object?[] { header, property.Properties, bytes };
            member = method.Invoke(obj: this,
                                   parameters: parameters)!;

            if (property.Flags.HasFlag(__HeaderFlags.IsEnumerated))
            {
                Int32 markerIndex = name.LastIndexOf("$");
                String indexedName = name[..markerIndex++] + counter.ToString() + name[markerIndex..];
                info.SetState(memberName: indexedName,
                              memberValue: member);
            }
            else
            {
                info.SetState(memberName: name,
                              memberValue: member);
            }

            return true;
        }
        return false;
    }

    private Boolean TryDeserializeMemberAsEnumerable(__Header header,
                                                     __HeaderObjectProperty property,
                                                     Byte[] body,
                                                     WriteableSerializationInfo info,
                                                     Int32 counter)
    {
        //Object member;
        Type type = header.TypeDictionary.ElementAt((Int32)property.TypeId).Key;

        if (type.IsTypeEnumerable())
        {
            String name = header.NameDictionary.ElementAt((Int32)property.NameId).Key;
            // Maybe unnessecary?
            //Int32 count = BitConverter.ToInt32(bytes);

            ReadOnlySerializationInfo itemsInfo = this.DeserializeMembers(header: header,
                                                                          properties: property.Properties,
                                                                          type: type,
                                                                          body: body);

            __HeaderTypeProperty elementTypeProperty = header.TypeDictionary[type].Properties[0];
            Type elementType = header.TypeDictionary.ElementAt((Int32)elementTypeProperty.TypeId).Key;

            IEnumerable list = elementType.ConstructList(itemsInfo);

            if (property.Flags.HasFlag(__HeaderFlags.IsEnumerated))
            {
                Int32 markerIndex = name.LastIndexOf("$");
                String indexedName = name[..markerIndex++] + counter.ToString() + name[markerIndex..];
                info.SetState(memberName: indexedName,
                              memberValue: list);
            }
            else
            {
                info.SetState(memberName: name,
                              memberValue: list);
            }

            return true;
        }

        return false;
    }

    private TResult? DeserializeThroughInterface<TResult>(__Header header,
                                                          List<__HeaderObjectProperty> properties,
                                                          Byte[] body)
        where TResult : IDeserializable<TResult>
    {
        ByteDeserializer<TResult> serializer = new(strategies: m_Strategies,
                                                   register: m_TypeRegister);
        return serializer.DeserializeWithHeader(header: header,
                                                properties: properties,
                                                type: typeof(TResult),
                                                body: body);
    }

    private static TResult CreateObject<TResult>(ReadOnlySerializationInfo info)
        where TResult : IDeserializable<TResult> =>
            TResult.ConstructFromSerializationData(info);

    private static IEnumerable CreateEnumerableType(Type type,
                                                    ReadOnlySerializationInfo info)
    {
        Type itemType = type.InferEnumerableElementType();

        List<Object?> objects = new();
        foreach (MemberState item in info.OrderBy(x => x.Name))
        {
            objects.Add(item.Value);
        }

        Array array = Array.CreateInstance(elementType: itemType,
                                           length: objects.Count);
        Array.Copy(sourceArray: objects.ToArray(),
                   destinationArray: array,
                   length: objects.Count);

        if (type.BaseType == typeof(Array))
        {
            return array;
        }

        ConstructorInfo[] constructors = type.GetConstructors();
        foreach (ConstructorInfo constructor in constructors)
        {
            ParameterInfo[] parameters = constructor.GetParameters();
            if (parameters.Length == 1 &&
                parameters[0].ParameterType == typeof(IEnumerable))
            {
                IEnumerable result = (IEnumerable)constructor.Invoke(new Object?[] { objects });
                return result;
            }
            else if (parameters.Length == 1 &&
                     parameters[0].ParameterType.IsGenericType &&
                     parameters[0].ParameterType.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                IEnumerable result = (IEnumerable)constructor.Invoke(new Object?[] { array });
                return result;
            }
        }

        // No Constructor found
        throw new MissingMethodException();
    }

    private static Boolean CanDeserialize(Type type,
                                          KeyValuePair<Type, IDeserializationStrategy<Byte[]>>[] strategies)
    {
        if (type.IsDeserializable() ||
            strategies.Any(x => x.Value.CanBeAppliedTo(type)))
        {
            return true;
        }
        else if (type.IsTypeEnumerable() &&
                 CanDeserialize(type: type.InferEnumerableElementType(),
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
                                                                    KeyValuePair<Type, IDeserializationStrategy<Byte[]>>[] strategies)
    {
        if (type.IsDeserializable())
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

    internal readonly ReadOnlyDictionary<Type, IDeserializationStrategy<Byte[]>, EqualityComparer<Type>> m_Strategies;
    internal readonly Func<ReadOnlySerializationInfo, TSerializable?>? m_DataSetter;
    internal readonly Dictionary<Type, MemberRegister> m_TypeRegister;
    internal readonly String[] m_MemberRegister;
    internal static readonly MethodInfo s_InfoFromSerializableMethod;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const String STREAM_DOES_NOT_SUPPORT_READING = "The specified stream does not support reading.";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    internal const String STREAM_DOES_NOT_SUPPORT_SEEKING = "The specified stream does not support seeking.";
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private const String COULD_NOT_DESERIALIZE_TYPE = "Couldn't deserialize type. (Are you missing a serialization strategy? Consider implementing the IDeserializable´1 interface, if it's a type you own.)";
}

// IDeserializer<T>
partial class ByteDeserializer<TSerializable> : IDeserializer<TSerializable>
{
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize([DisallowNull] Stream stream,
                                      Int64 offset,
                                      out UInt64 read,
                                      SerializationFinishAction actionAfter)
    {
        if (!stream.CanRead)
        {
            throw new InvalidOperationException(message: STREAM_DOES_NOT_SUPPORT_READING);
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

        TSerializable? result = this.DeserializeInternal(stream: stream.AsReadableStream(),
                                                         read: out read);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Seek(offset: 0,
                        origin: SeekOrigin.Begin);
        }
        if (actionAfter.HasFlag(SerializationFinishAction.FlushStream))
        {
            stream.Flush();
        }
        if (actionAfter.HasFlag(SerializationFinishAction.CloseStream))
        {
            stream.Close();
        }
        return result;
    }
    /// <inheritdoc/>
    /// <exception cref="InvalidOperationException"/>
    [return: MaybeNull]
    public TSerializable? Deserialize<TStream>([DisallowNull] TStream stream,
                                               Int64 offset,
                                               out UInt64 read,
                                               SerializationFinishAction actionAfter)
        where TStream : IReadableStream
    {
        if (offset > -1)
        {
            stream.Position = offset;
        }

        TSerializable? result = this.DeserializeInternal(stream: stream,
                                                         read: out read);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Position = 0;
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
    public Boolean TryDeserialize([DisallowNull] Stream stream,
                                  Int64 offset,
                                  out UInt64 read,
                                  SerializationFinishAction actionAfter,
                                  [AllowNull] out TSerializable? result)
    {
        if (stream is null ||
            !stream.CanRead ||
            (offset > -1 &&
            !stream.CanSeek))
        {
            read = 0;
            result = default;
            return false;
        }

        if (offset > -1)
        {
            stream.Seek(offset: offset,
                        origin: SeekOrigin.Begin);
        }

        result = this.DeserializeInternal(stream: stream.AsReadableStream(),
                                          read: out read);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Seek(offset: 0,
                        origin: SeekOrigin.Begin);
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
    public Boolean TryDeserialize<TStream>([DisallowNull] TStream stream,
                                           Int64 offset,
                                           out UInt64 read,
                                           SerializationFinishAction actionAfter,
                                           [AllowNull] out TSerializable? result)
        where TStream : IReadableStream
    {
        if (offset > -1)
        {
            stream.Position = offset;
        }

        result = this.DeserializeInternal(stream: stream,
                                          read: out read);

        if (actionAfter.HasFlag(SerializationFinishAction.MoveToBeginning))
        {
            stream.Position = 0;
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