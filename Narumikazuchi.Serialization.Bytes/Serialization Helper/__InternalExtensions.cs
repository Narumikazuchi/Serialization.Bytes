namespace Narumikazuchi.Serialization.Bytes;

internal static class __InternalExtensions
{
    internal static Int32 Count(this IEnumerable items)
    {
        if (items.GetType()
                 .GetInterfaces()
                 .Any(x => x.IsGenericType &&
                           x.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>)))
        {
            PropertyInfo countProperty = items.GetType().GetProperty(name: "Count",
                                                                     bindingAttr: BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!;
            return (Int32)countProperty.GetValue(obj: items,
                                                 index: null)!;
        }
        else
        {
            Int32 count = 0;
            foreach (Object _ in items)
            {
                count++;
            }
            return count;
        }
    }

    internal static Boolean IsTypeEnumerable(this Type type)
    {
        if (type.IsInterface &&
            type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return true;
        }
        else if (type.GetInterfaces()
                     .Any(IsTypeEnumerable))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    internal static Type InferEnumerableElementType(this Type type)
    {
        if (type.IsInterface &&
            type.IsGenericType &&
            type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
        {
            return type.GetGenericArguments()[0];
        }
        else
        {
            return type.GetInterfaces()
                       .First(IsTypeEnumerable)
                       .GetGenericArguments()[0];
        }
    }

    internal static Boolean IsSerializable(this Type type) =>
        !type.IsInterface &&
        !type.IsEnum &&
        !type.IsPointer &&
        type.IsAssignableTo(typeof(ISerializable));

    internal static Boolean IsDeserializable(this Type type)
    {
        if (type.IsInterface ||
            type.IsEnum ||
            type.IsPointer)
        {
            return false;
        }

        foreach (Type @interface in type.GetInterfaces())
        {
            if (@interface.IsGenericType)
            {
                if (@interface.GetGenericTypeDefinition() == typeof(IDeserializable<>) &&
                    (@interface.GenericTypeArguments[0] == type ||
                    @interface.GenericTypeArguments[0].IsAssignableTo(type)))
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static Dictionary<Type, MemberRegister> BuildMemberTypeRegister(this Type type)
    {
        if (!type.IsAssignableTo(typeof(ISerializationTarget)))
        {
            throw new InvalidOperationException();
        }
        else
        {
            Dictionary<Type, MemberRegister> map = new();
            MethodInfo method = s_BuildMemberTypesMethod.MakeGenericMethod(type);
            method.Invoke(obj: null,
                          parameters: new Object?[] { map });
            return map;
        }
    }
    internal static Dictionary<Type, MemberRegister> BuildMemberTypeRegister(this Type type,
                                                                             Action<MemberRegister> memberRegistration)
    {
        Dictionary<Type, MemberRegister> map = new();
        MemberRegister register = new();
        memberRegistration.Invoke(register);

        map.Add(key: type,
                value: register);

        foreach ((String _, Type memberType) in register)
        {
            if (memberType.IsAssignableTo(typeof(ISerializable)))
            {
                MethodInfo method = s_BuildMemberTypesMethod.MakeGenericMethod(memberType);
                method.Invoke(obj: null,
                              parameters: new Object?[] { map });
                continue;
            }
            if (memberType.IsTypeEnumerable())
            {
                BuildEnumerableTypeRegister(map: map,
                                            type: memberType);
            }
            else
            {
                map.Add(key: memberType,
                        value: new());
            }
        }
        return map;
    }

    internal static IEnumerable ConstructList(this Type type,
                                              ReadOnlySerializationInfo info)
    {
        Type listType = typeof(List<>).MakeGenericType(type);
        ConstructorInfo constructor = listType.GetConstructor(Array.Empty<Type>())!;
        Object result = constructor.Invoke(Array.Empty<Object>());

        Type enumType = typeof(IEnumerable<>).MakeGenericType(type);
        IEnumerable items = info.Select(x => x.Value);
        MethodInfo ofTypeMethod = s_OfTypeMethod.MakeGenericMethod(type);
        items = (IEnumerable)ofTypeMethod.Invoke(obj: null,
                                                 parameters: new Object?[] { items })!;

        MethodInfo addMethod = listType.GetMethod("AddRange")!;
        addMethod.Invoke(obj: result,
                         parameters: new Object?[] { items });

        return (IEnumerable)result;
    }

    internal static Int32 ReadInt32<TStream>(this TStream stream,
                                             ref UInt64 read)
        where TStream : IReadableStream
    {
        Byte[] data = new Byte[sizeof(Int32)];
        stream.Read(buffer: data,
                    offset: 0,
                    count: sizeof(Int32));
        read += sizeof(Int32);
        return BitConverter.ToInt32(value: data);
    }

    internal static UInt32 ReadUInt32<TStream>(this TStream stream,
                                               ref UInt64 read)
        where TStream : IReadableStream
    {
        Byte[] data = new Byte[sizeof(UInt32)];
        stream.Read(buffer: data,
                    offset: 0,
                    count: sizeof(UInt32));
        read += sizeof(UInt32);
        return BitConverter.ToUInt32(value: data);
    }

    internal static Int64 ReadInt64<TStream>(this TStream stream,
                                             ref UInt64 read)
        where TStream : IReadableStream
    {
        Byte[] data = new Byte[sizeof(Int64)];
        stream.Read(buffer: data,
                    offset: 0,
                    count: sizeof(Int64));
        read += sizeof(Int64);
        return BitConverter.ToInt64(value: data);
    }

    internal static UInt64 ReadUInt64<TStream>(this TStream stream,
                                               ref UInt64 read)
        where TStream : IReadableStream
    {
        Byte[] data = new Byte[sizeof(UInt64)];
        stream.Read(buffer: data,
                    offset: 0,
                    count: sizeof(UInt64));
        read += sizeof(UInt64);
        return BitConverter.ToUInt64(value: data);
    }

    internal static List<TStreamWriteable> ReadList<TStreamWriteable, TStream>(this TStream stream,
                                                                               ref UInt64 read)
        where TStreamWriteable : __IStreamWriteable<TStreamWriteable>
        where TStream : IReadableStream
    {
        List<TStreamWriteable> list = new();
        Int32 count = stream.ReadInt32(ref read);
        for (Int32 i = 0;
             i < count;
             i++)
        {
            TStreamWriteable value = TStreamWriteable.FromStream(stream: stream,
                                                                 read: ref read);
            list.Add(value);
        }
        return list;
    }

    internal static void WriteToStream<TStreamWriteable, TStream>(this IReadOnlyCollection<TStreamWriteable> collection,
                                                                  TStream stream)
        where TStreamWriteable : __IStreamWriteable<TStreamWriteable>
        where TStream : IWriteableStream
    {
        stream.Write(BitConverter.GetBytes(collection.Count));
        foreach (TStreamWriteable value in collection)
        {
            value.WriteToStream(stream);
        }
    }

    private static void BuildMemberTypeRegisterInternal<T>(Dictionary<Type, MemberRegister> map)
        where T : ISerializationTarget
    {
        MemberRegister register = new();
        T.RegisterSerializedTypes(register);

        foreach ((String _, Type memberType) in register)
        {
            if (memberType.IsAssignableTo(typeof(ISerializable)))
            {
                MethodInfo method = s_BuildMemberTypesMethod.MakeGenericMethod(memberType);
                method.Invoke(obj: null,
                              parameters: new Object?[] { map });
            }
            else if (memberType.IsTypeEnumerable())
            {
                BuildEnumerableTypeRegister(map: map,
                                            type: memberType);
            }
            else
            {
                map.TryAdd(key: memberType,
                           value: new());
            }
        }

        map.TryAdd(key: typeof(T),
                   value: register);
    }

    private static void BuildEnumerableTypeRegister(Dictionary<Type, MemberRegister> map,
                                                    Type type)
    {
        Type elementType = type.InferEnumerableElementType();
        if (elementType.IsAssignableTo(typeof(ISerializable)))
        {
            MethodInfo method = s_BuildMemberTypesMethod.MakeGenericMethod(elementType);
            method.Invoke(obj: null,
                          parameters: new Object?[] { map });
        }
        else if (elementType.IsTypeEnumerable())
        {
            BuildEnumerableTypeRegister(map: map,
                                        type: elementType);
        }
        else
        {
            map.TryAdd(key: elementType,
                       value: new());
        }

        map.TryAdd(key: type,
                   value: new());
    }

    private static readonly MethodInfo s_BuildMemberTypesMethod = typeof(__InternalExtensions).GetMethod(name: nameof(BuildMemberTypeRegisterInternal),
                                                                                                         bindingAttr: BindingFlags.NonPublic | BindingFlags.Static)!;
    private static readonly MethodInfo s_OfTypeMethod = typeof(Enumerable).GetMethod("OfType")!;
}