namespace Narumikazuchi.Serialization.Bytes;

internal sealed partial class __ByteSerializerBuilder<TSerializable>
{
    public __ByteSerializerBuilder()
    { }

    public IDictionary<Type, ISerializationStrategy<Byte[]>> SerializationStrategies =>
        m_SerializationStrategies;
    public IDictionary<Type, IDeserializationStrategy<Byte[]>> DeserializationStrategies =>
        m_DeserializationStrategies;
    public IDictionary<Type, ISerializationDeserializationStrategy<Byte[]>> TwoWayStrategies =>
        m_TwoWayStrategies;

    public Action<TSerializable, ISerializationInfoAdder>? DataGetter
    {
        get => m_DataGetter;
        set => m_DataGetter = value;
    }

    public Func<ISerializationInfoGetter, TSerializable>? DataSetter
    {
        get => m_DataSetter;
        set => m_DataSetter = value;
    }
}

// Non-Public
partial class __ByteSerializerBuilder<TSerializable>
{
    private readonly IDictionary<Type, ISerializationStrategy<Byte[]>> m_SerializationStrategies = new Dictionary<Type, ISerializationStrategy<Byte[]>>();
    private readonly IDictionary<Type, IDeserializationStrategy<Byte[]>> m_DeserializationStrategies = new Dictionary<Type, IDeserializationStrategy<Byte[]>>();
    private readonly IDictionary<Type, ISerializationDeserializationStrategy<Byte[]>> m_TwoWayStrategies = new Dictionary<Type, ISerializationDeserializationStrategy<Byte[]>>();
    private Action<TSerializable, ISerializationInfoAdder>? m_DataGetter = null;
    private Func<ISerializationInfoGetter, TSerializable>? m_DataSetter = null;
}

// IByteDeserializerDefaultStrategyAppender<T>
partial class __ByteSerializerBuilder<TSerializable> : IByteDeserializerDefaultStrategyAppender<TSerializable>
{
    IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteDeserializerDefaultStrategyAppender<TSerializable>.UseDefaultStrategies()
    {
        foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> kv in IntegratedSerializationStrategies.All)
        {
            m_DeserializationStrategies.Add(key: kv.Key,
                                            value: kv.Value);
        }
        return this;
    }
}

// IByteDeserializerStrategyAppender<T>
partial class __ByteSerializerBuilder<TSerializable> : IByteDeserializerStrategyAppender<TSerializable>
{
    IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteDeserializerStrategyAppender<TSerializable>.UseStrategies([DisallowNull] IEnumerable<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>> strategies!!)
    {
        foreach (KeyValuePair<Type, IDeserializationStrategy<Byte[]>> kv in strategies)
        {
            m_DeserializationStrategies.Add(key: kv.Key,
                                            value: kv.Value);
        }
        return this;
    }

    IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteDeserializerStrategyAppender<TSerializable>.UseStrategyForType<TFrom>([DisallowNull] IDeserializationStrategy<Byte[], TFrom> strategy!!)
    {
        m_DeserializationStrategies.Add(key: typeof(TFrom),
                                        value: strategy);
        return this;
    }
}

// IByteDeserializerStrategyAppenderOrFinalizer<T>
partial class __ByteSerializerBuilder<TSerializable> : IByteDeserializerStrategyAppenderOrFinalizer<TSerializable>
{
    IByteDeserializer<TSerializable> IByteDeserializerStrategyAppenderOrFinalizer<TSerializable>.Create()
    {
        __ByteSerializer<TSerializable> deserializer;
        if (__Cache.CreatedDeserializers
                   .ContainsKey(typeof(TSerializable)))
        {
            IList<Object> deserializers = __Cache.CreatedDeserializers[typeof(TSerializable)];
            for (Int32 i = 0; i < deserializers.Count; i++)
            {
                deserializer = (__ByteSerializer<TSerializable>)deserializers[i];
                if (m_DataGetter != deserializer.DataGetter)
                {
                    continue;
                }
                if (m_DataSetter != deserializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, IDeserializationStrategy<Byte[]>> kv in m_DeserializationStrategies)
                {
                    if (!deserializer.DeserializationStrategies
                                     .Contains(kv))
                    {
                        next = true;
                        break;
                    }
                }
                if (next)
                {
                    continue;
                }

                return deserializer;
            }
        }
        else
        {
            __Cache.CreatedDeserializers
                   .Add(key: typeof(TSerializable),
                        value: new List<Object>());
        }

        deserializer = new __ByteSerializer<TSerializable>(this);
        __Cache.CreatedDeserializers[typeof(TSerializable)]
               .Add(deserializer);
        return deserializer;
    }
}

// IByteSerializerDefaultStrategyAppender<T>
partial class __ByteSerializerBuilder<TSerializable> : IByteSerializerDefaultStrategyAppender<TSerializable>
{
    IByteSerializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDefaultStrategyAppender<TSerializable>.UseDefaultStrategies()
    {
        foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> kv in IntegratedSerializationStrategies.All)
        {
            m_SerializationStrategies.Add(key: kv.Key,
                                          value: kv.Value);
        }
        return this;
    }
}

// IByteSerializerStrategyAppender<T>
partial class __ByteSerializerBuilder<TSerializable> : IByteSerializerStrategyAppender<TSerializable>
{
    IByteSerializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerStrategyAppender<TSerializable>.UseStrategies(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies!!)
    {
        foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> kv in strategies)
        {
            m_SerializationStrategies.Add(key: kv.Key,
                                          value: kv.Value);
        }
        return this;
    }

    IByteSerializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerStrategyAppender<TSerializable>.UseStrategyForType<TFrom>(ISerializationStrategy<Byte[], TFrom> strategy!!)
    {
        m_SerializationStrategies.Add(key: typeof(TFrom),
                                      value: strategy);
        return this;
    }
}

// IByteSerializerStrategyAppenderOrFinalizer<T>
partial class __ByteSerializerBuilder<TSerializable> : IByteSerializerStrategyAppenderOrFinalizer<TSerializable>
{
    IByteSerializer<TSerializable> IByteSerializerStrategyAppenderOrFinalizer<TSerializable>.Construct()
    {
        __ByteSerializer<TSerializable> serializer;
        if (__Cache.CreatedSerializers
                   .ContainsKey(typeof(TSerializable)))
        {
            IList<Object> deserializers = __Cache.CreatedSerializers[typeof(TSerializable)];
            for (Int32 i = 0; i < deserializers.Count; i++)
            {
                serializer = (__ByteSerializer<TSerializable>)deserializers[i];
                if (m_DataGetter != serializer.DataGetter)
                {
                    continue;
                }
                if (m_DataSetter != serializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> kv in m_SerializationStrategies)
                {
                    if (!serializer.SerializationStrategies
                                   .Contains(kv))
                    {
                        next = true;
                        break;
                    }
                }
                if (next)
                {
                    continue;
                }

                return serializer;
            }
        }
        else
        {
            __Cache.CreatedSerializers
                   .Add(key: typeof(TSerializable),
                        value: new List<Object>());
        }

        serializer = new __ByteSerializer<TSerializable>(this);
        __Cache.CreatedSerializers[typeof(TSerializable)]
               .Add(serializer);
        return serializer;
    }
}

// IByteSerializerDeserializerDefaultStrategyAppender<T>
partial class __ByteSerializerBuilder<TSerializable> : IByteSerializerDeserializerDefaultStrategyAppender<TSerializable>
{
    IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDeserializerDefaultStrategyAppender<TSerializable>.UseDefaultStrategies()
    {
        foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> kv in IntegratedSerializationStrategies.All)
        {
            m_TwoWayStrategies.Add(kv);
        }
        return this;
    }
}

// IByteSerializerDeserializerStrategyAppender<T>
partial class __ByteSerializerBuilder<TSerializable> : IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDeserializerStrategyAppender<TSerializable>.UseStrategies(IEnumerable<KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>>> strategies!!)
    {
        foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> kv in strategies)
        {
            m_TwoWayStrategies.Add(kv);
        }
        return this;
    }

    IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDeserializerStrategyAppender<TSerializable>.UseStrategyForType<TFrom>(ISerializationDeserializationStrategy<Byte[], TFrom> strategy!!)
    {
        m_TwoWayStrategies.Add(key: typeof(TFrom),
                               value: strategy);
        return this;
    }
}

// IByteSerializerDeserializerStrategyAppenderOrFinalizer<T>
partial class __ByteSerializerBuilder<TSerializable> : IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable>
{
    IByteSerializerDeserializer<TSerializable> IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable>.Construct()
    {
        __ByteSerializer<TSerializable> serializer;
        if (__Cache.CreatedSerializersDeserializers
                   .ContainsKey(typeof(TSerializable)))
        {
            IList<Object> deserializers = __Cache.CreatedSerializersDeserializers[typeof(TSerializable)];
            for (Int32 i = 0; i < deserializers.Count; i++)
            {
                serializer = (__ByteSerializer<TSerializable>)deserializers[i];
                if (m_DataGetter != serializer.DataGetter)
                {
                    continue;
                }
                if (m_DataSetter != serializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> kv in m_TwoWayStrategies)
                {
                    if (!serializer.TwoWayStrategies
                                   .Contains(kv))
                    {
                        next = true;
                        break;
                    }
                }
                if (next)
                {
                    continue;
                }

                return serializer;
            }
        }
        else
        {
            __Cache.CreatedSerializersDeserializers
                   .Add(key: typeof(TSerializable),
                        value: new List<Object>());
        }

        serializer = new __ByteSerializer<TSerializable>(this);
        __Cache.CreatedSerializersDeserializers[typeof(TSerializable)]
               .Add(serializer);
        return serializer;
    }
}

// ISerializationOwnedTypeConfigurator<T>
partial class __ByteSerializerBuilder<TSerializable> : ISerializationForeignTypeConfigurator<TSerializable>
{
    IByteDeserializerDefaultStrategyAppender<TSerializable> ISerializationForeignTypeConfigurator<TSerializable>.ForDeserialization(Func<ISerializationInfoGetter, TSerializable> constructFromSerializationData!!)
    {
        m_DataSetter = constructFromSerializationData;
        return this;
    }
    IByteDeserializerDefaultStrategyAppender<TSerializable> ISerializationForeignTypeConfigurator<TSerializable>.ForDeserialization(IDeserializationStrategy<Byte[], TSerializable> strategy!!)
    {
        m_DeserializationStrategies.Add(key: typeof(TSerializable),
                                        value: strategy);
        return this;
    }

    IByteSerializerDefaultStrategyAppender<TSerializable> ISerializationForeignTypeConfigurator<TSerializable>.ForSerialization(Action<TSerializable, ISerializationInfoAdder> getSerializationData!!)
    {
        m_DataGetter = getSerializationData;
        return this;
    }
    IByteSerializerDefaultStrategyAppender<TSerializable> ISerializationForeignTypeConfigurator<TSerializable>.ForSerialization(ISerializationStrategy<Byte[], TSerializable> strategy!!)
    {
        m_SerializationStrategies.Add(key: typeof(TSerializable),
                                      value: strategy);
        return this;
    }

    IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ISerializationForeignTypeConfigurator<TSerializable>.ForSerializationAndDeserialization(Action<TSerializable, ISerializationInfoAdder> getSerializationData!!, 
                                                                                                                                                              Func<ISerializationInfoGetter, TSerializable> constructFromSerializationData!!)
    {
        m_DataGetter = getSerializationData;
        m_DataSetter = constructFromSerializationData;
        return this;
    }
    IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ISerializationForeignTypeConfigurator<TSerializable>.ForSerializationAndDeserialization(ISerializationDeserializationStrategy<Byte[], TSerializable> strategy!!)
    {
        m_TwoWayStrategies.Add(key: typeof(TSerializable),
                               value: strategy);
        return this;
    }
}

// ISerializationOwnedTypeConfigurator<T>
partial class __ByteSerializerBuilder<TSerializable> : ISerializationOwnedTypeConfigurator<TSerializable>
{
    IByteDeserializerDefaultStrategyAppender<TSerializable> ISerializationOwnedTypeConfigurator<TSerializable>.ForDeserialization() =>
        this;
    IByteSerializerDefaultStrategyAppender<TSerializable> ISerializationOwnedTypeConfigurator<TSerializable>.ForSerialization() =>
        this;
    IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> ISerializationOwnedTypeConfigurator<TSerializable>.ForSerializationAndDeserialization() =>
        this;
}