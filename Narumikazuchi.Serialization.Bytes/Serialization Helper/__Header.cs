namespace Narumikazuchi.Serialization.Bytes;

internal class __Header
{
    internal __Header()
    { }

    internal __InMemoryStream AsMemory(Int64 sizeOfBody)
    {
        __InMemoryStream stream = new();

        if (this.Flags.HasFlag(__HeaderFlags.IsNull))
        {
            List<Byte> bytes = new();
            // Entire object = 8 byte (sizeof Head) + 8 byte (sizeof Body) + 1 byte (null flag)
            bytes.AddRange(BitConverter.GetBytes(17L));
            bytes.AddRange(BitConverter.GetBytes(9L));
            bytes.AddRange(BitConverter.GetBytes(0L));
            bytes.Add((Byte)__HeaderFlags.IsNull);
            stream.Write(bytes.ToArray());

            stream.Position = 0;
            return stream;
        }
        else if (this.Flags.HasFlag(__HeaderFlags.NotNull))
        {
            stream.Position = 24;
            stream.WriteByte((Byte)this.Flags);
            this.NameDictionary.Values.WriteToStream(stream);
            this.TypeDictionary.Values.WriteToStream(stream);
            stream.Write(BitConverter.GetBytes(this.TypeId));
            this.Properties.WriteToStream(stream);

            this.SizeOfHeader = stream.Length;
            this.SizeOfBody = sizeOfBody;

            stream.Position = 0;
            stream.Write(BitConverter.GetBytes(this.SizeOfEntireObject));
            stream.Write(BitConverter.GetBytes(this.SizeOfHeader));
            stream.Write(BitConverter.GetBytes(this.SizeOfBody));

            stream.Position = 0;
            return stream;
        }
        else
        {
            throw new NotSupportedException();
        }

    }

    internal void RegisterTypes(Dictionary<Type, MemberRegister> types)
    {
        foreach (KeyValuePair<Type, MemberRegister> kv in types)
        {
            this.RegisterType(types: types,
                              type: kv.Key,
                              register: kv.Value);
        }
    }

    internal void RegisterNames(params String[] names)
    {
        foreach (String name in names)
        {
            this.RegisterName(name);
        }
    }

    internal static __Header FromStream<TStream>(TStream stream,
                                                 Int64 sizeOfHead,
                                                 Int64 sizeOfBody,
                                                 out UInt64 read)
        where TStream : IReadableStream
    {
        read = sizeof(Int64) * 3;
        if (!stream.ReadByte(out Byte? singleByte))
        {
            throw new IOException();
        }

        __HeaderFlags flags = (__HeaderFlags)singleByte;
        read++;

        if (flags is __HeaderFlags.IsNull)
        {
            return new()
            {
                Flags = flags,
                SizeOfHeader = sizeOfHead,
                SizeOfBody = sizeOfBody
            };
        }
        else if (flags is __HeaderFlags.NotNull)
        {
            List<__HeaderNameEntry> names = stream.ReadList<__HeaderNameEntry, TStream>(ref read);
            List<__HeaderTypeEntry> types = stream.ReadList<__HeaderTypeEntry, TStream>(ref read);
            UInt32 myTypeId = stream.ReadUInt32(ref read);
            List<__HeaderObjectProperty> properties = stream.ReadList<__HeaderObjectProperty, TStream>(ref read);

            Dictionary<String, __HeaderNameEntry> nameDictionary = names.OrderBy(x => x.Id)
                                                                        .ToDictionary(x => x.Name,
                                                                                      x => x);
            foreach (__HeaderTypeEntry type in types)
            {
                String typename = $"{nameDictionary.ElementAt((Int32)type.NameId).Key}, {nameDictionary.ElementAt((Int32)type.AssemblyId).Key}";
                Option<Type> assemblyType = type.CreateType(nameDictionary);
                if (!assemblyType.HasValue)
                {
                    throw new FormatException($"Couldn't find the type \"{nameDictionary.ElementAt((Int32)type.NameId).Key}, {nameDictionary.ElementAt((Int32)type.AssemblyId).Key}\" in any of the loaded assemblies. Can't deserialize without type information.");
                }
            }

            Dictionary<Type, __HeaderTypeEntry> typeDictionary = types.OrderBy(x => x.Id)
                                                                      .ToDictionary(x => x.CreateType(nameDictionary),
                                                                                    x => x);

            return new()
            {
                SizeOfHeader = sizeOfHead,
                SizeOfBody = sizeOfBody,
                Flags = flags,
                NameDictionary = nameDictionary,
                TypeDictionary = typeDictionary,
                TypeId = myTypeId,
                Properties = properties
            };
        }
        else
        {
            throw new IOException();
        }
    }

    private UInt32 RegisterName(String name)
    {
        if (!this.NameDictionary.ContainsKey(name))
        {
            __HeaderNameEntry entry = new()
            {
                Id = (UInt32)this.NameDictionary.Count,
                Name = name
            };
            this.NameDictionary.Add(key: name,
                                    value: entry);
            return entry.Id;
        }
        else
        {
            return this.NameDictionary[name].Id;
        }
    }

    private void RegisterType(Dictionary<Type, MemberRegister> types,
                              Type type,
                              MemberRegister register)
    {
        List<__HeaderTypeProperty> properties = new();
        foreach ((String memberName, Type memberType) in register)
        {
            if (!this.TypeDictionary.ContainsKey(memberType))
            {
                if (types.ContainsKey(memberType))
                {
                    this.RegisterType(types: types,
                                      type: memberType,
                                      register: types[memberType]);
                }
            }

            __HeaderTypeProperty property = new()
            {
                NameId = (Int32)this.NameDictionary[memberName].Id,
                TypeId = this.TypeDictionary[memberType].Id
            };
            properties.Add(property);
        }

        if (!this.TypeDictionary.ContainsKey(type))
        {
            if (type.IsTypeEnumerable())
            {
                Type elementType = type.InferEnumerableElementType();
                if (!this.TypeDictionary.ContainsKey(elementType))
                {
                    this.RegisterType(types: types,
                                      type: elementType,
                                      register: types[elementType]);
                }

                __HeaderTypeProperty property = new()
                {
                    NameId = -1,
                    TypeId = this.TypeDictionary[elementType].Id
                };
                properties.Add(property);
            }

            UInt32 assemblyId = this.RegisterName(type.Assembly.FullName!);
            UInt32 nameId = this.RegisterName(type.FullName!);
            __HeaderTypeEntry entry = new()
            {
                Id = (UInt32)this.TypeDictionary.Count,
                NameId = nameId,
                AssemblyId = assemblyId,
                Properties = properties
            };
            this.TypeDictionary.Add(key: type,
                                    value: entry);
        }
    }

    internal Int64 SizeOfEntireObject => 
        3 * sizeof(Int64) + this.SizeOfHeader + this.SizeOfBody;

    internal Int64 SizeOfHeader
    {
        get;
        set;
    }

    internal Int64 SizeOfBody
    {
        get;
        set;
    }

    internal __HeaderFlags Flags
    {
        get;
        set;
    }

    internal Dictionary<String, __HeaderNameEntry> NameDictionary
    {
        get;
        init;
    } = new();

    internal Dictionary<Type, __HeaderTypeEntry> TypeDictionary
    {
        get;
        init;
    } = new();

    internal UInt32 TypeId
    {
        get;
        set;
    }

    internal List<__HeaderObjectProperty> Properties
    {
        get;
        init;
    } = new();
}