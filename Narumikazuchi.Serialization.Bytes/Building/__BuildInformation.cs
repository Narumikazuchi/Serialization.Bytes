namespace Narumikazuchi.Serialization.Bytes;

internal sealed partial class __BuildInformation<TSerializable>
{
    public IDictionary<Type, ISerializationStrategy<Byte[]>> SerializationStrategies =>
        this._serializationStrategies;
    public IDictionary<Type, IDeserializationStrategy<Byte[]>> DeserializationStrategies =>
        this._deserializationStrategies;
    public IDictionary<Type, ISerializationDeserializationStrategy<Byte[]>> TwoWayStrategies =>
        this._twoWayStrategies;

    public Action<TSerializable, ISerializationInfoAdder>? DataGetter
    {
        get => this._dataGetter;
        set => this._dataGetter = value;
    }

    public Func<ISerializationInfoGetter, TSerializable>? DataSetter
    {
        get => this._dataSetter;
        set => this._dataSetter = value;
    }
}

// Non-Public
partial class __BuildInformation<TSerializable>
{
    private readonly IDictionary<Type, ISerializationStrategy<Byte[]>> _serializationStrategies = new Dictionary<Type, ISerializationStrategy<Byte[]>>();
    private readonly IDictionary<Type, IDeserializationStrategy<Byte[]>> _deserializationStrategies = new Dictionary<Type, IDeserializationStrategy<Byte[]>>();
    private readonly IDictionary<Type, ISerializationDeserializationStrategy<Byte[]>> _twoWayStrategies = new Dictionary<Type, ISerializationDeserializationStrategy<Byte[]>>();
    private Action<TSerializable, ISerializationInfoAdder>? _dataGetter = null;
    private Func<ISerializationInfoGetter, TSerializable>? _dataSetter = null;
}

// IByteDeserializerDefaultStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteDeserializerDefaultStrategyAppender<TSerializable>
{
    IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteDeserializerDefaultStrategyAppender<TSerializable>.UseDefaultStrategies()
    {
        foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> strategy in __SerializationStrategies.Integrated)
        {
            if (this._deserializationStrategies
                    .ContainsKey(strategy.Key))
            {
                continue;
            }
            this._deserializationStrategies
                .Add(key: strategy.Key,
                     value: strategy.Value);
        }
        return this;
    }
}

// IByteDeserializerStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteDeserializerStrategyAppender<TSerializable>
{
    IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteDeserializerStrategyAppender<TSerializable>.UseStrategies(IEnumerable<KeyValuePair<Type, IDeserializationStrategy<Byte[]>>> strategies)
    {
        foreach (KeyValuePair<Type, IDeserializationStrategy<Byte[]>> strategy in strategies)
        {
            if (this._deserializationStrategies
                    .ContainsKey(strategy.Key))
            {
                continue;
            }
            this._deserializationStrategies
                .Add(item: strategy);
        }
        return this;
    }

    IByteDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteDeserializerStrategyAppender<TSerializable>.UseStrategyForType<TFrom>(IDeserializationStrategy<Byte[], TFrom> strategy)
    {
        ExceptionHelpers.ThrowIfArgumentNull(strategy);

        if (this._deserializationStrategies
                .ContainsKey(typeof(TFrom)))
        {
            this._deserializationStrategies[typeof(TFrom)] = strategy;
            return this;
        }
        this._deserializationStrategies
            .Add(key: typeof(TFrom),
                 value: strategy);
        return this;
    }
}

// IByteDeserializerStrategyAppenderOrFinalizer<T>
partial class __BuildInformation<TSerializable> : IByteDeserializerStrategyAppenderOrFinalizer<TSerializable>
{
    IByteDeserializer<TSerializable> IByteDeserializerStrategyAppenderOrFinalizer<TSerializable>.Construct()
    {
        __ByteSerializer<TSerializable> deserializer;
        if (__Cache.CreatedDeserializers
                   .ContainsKey(typeof(TSerializable)))
        {
            IList<Object> deserializers = __Cache.CreatedDeserializers[typeof(TSerializable)];
            for (Int32 i = 0; i < deserializers.Count; i++)
            {
                deserializer = (__ByteSerializer<TSerializable>)deserializers[i];
                if (this._dataGetter != deserializer.DataGetter)
                {
                    continue;
                }
                if (this._dataSetter != deserializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, IDeserializationStrategy<Byte[]>> kv in this._deserializationStrategies)
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
partial class __BuildInformation<TSerializable> : IByteSerializerDefaultStrategyAppender<TSerializable>
{
    IByteSerializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDefaultStrategyAppender<TSerializable>.UseDefaultStrategies()
    {
        foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> strategy in __SerializationStrategies.Integrated)
        {
            if (this._serializationStrategies
                    .ContainsKey(strategy.Key))
            {
                continue;
            }
            this._serializationStrategies
                .Add(key: strategy.Key,
                     value: strategy.Value);
        }
        return this;
    }
}

// IByteSerializerStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteSerializerStrategyAppender<TSerializable>
{
    IByteSerializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerStrategyAppender<TSerializable>.UseStrategies(IEnumerable<KeyValuePair<Type, ISerializationStrategy<Byte[]>>> strategies)
    {
        foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> strategy in strategies)
        {
            if (this._serializationStrategies
                    .ContainsKey(strategy.Key))
            {
                continue;
            }
            this._serializationStrategies
                .Add(item: strategy);
        }
        return this;
    }

    IByteSerializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerStrategyAppender<TSerializable>.UseStrategyForType<TFrom>(ISerializationStrategy<Byte[], TFrom> strategy)
    {
        ExceptionHelpers.ThrowIfArgumentNull(strategy);

        if (this._serializationStrategies
                .ContainsKey(typeof(TFrom)))
        {
            this._serializationStrategies[typeof(TFrom)] = strategy;
            return this;
        }
        this._serializationStrategies
            .Add(key: typeof(TFrom),
                 value: strategy);
        return this;
    }
}

// IByteSerializerStrategyAppenderOrFinalizer<T>
partial class __BuildInformation<TSerializable> : IByteSerializerStrategyAppenderOrFinalizer<TSerializable>
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
                if (this._dataGetter != serializer.DataGetter)
                {
                    continue;
                }
                if (this._dataSetter != serializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, ISerializationStrategy<Byte[]>> kv in this._serializationStrategies)
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
partial class __BuildInformation<TSerializable> : IByteSerializerDeserializerDefaultStrategyAppender<TSerializable>
{
    IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDeserializerDefaultStrategyAppender<TSerializable>.UseDefaultStrategies()
    {
        foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> strategy in __SerializationStrategies.Integrated)
        {
            if (this._twoWayStrategies
                    .ContainsKey(strategy.Key))
            {
                continue;
            }
            this._twoWayStrategies
                .Add(item: strategy);
        }
        return this;
    }
}

// IByteSerializerDeserializerStrategyAppender<T>
partial class __BuildInformation<TSerializable> : IByteSerializerDeserializerStrategyAppender<TSerializable>
{
    IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDeserializerStrategyAppender<TSerializable>.UseStrategies(IEnumerable<KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>>> strategies)
    {
        foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> strategy in strategies)
        {
            if (this._twoWayStrategies
                    .ContainsKey(strategy.Key))
            {
                continue;
            }
            this._twoWayStrategies
                .Add(item: strategy);
        }
        return this;
    }

    IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable> IByteSerializerDeserializerStrategyAppender<TSerializable>.UseStrategyForType<TFrom>(ISerializationDeserializationStrategy<Byte[], TFrom> strategy)
    {
        ExceptionHelpers.ThrowIfArgumentNull(strategy);

        if (this._twoWayStrategies
                .ContainsKey(typeof(TFrom)))
        {
            this._twoWayStrategies[typeof(TFrom)] = strategy;
            return this;
        }
        this._twoWayStrategies
            .Add(key: typeof(TFrom),
                 value: strategy);
        return this;
    }
}

// IByteSerializerDeserializerStrategyAppenderOrFinalizer<T>
partial class __BuildInformation<TSerializable> : IByteSerializerDeserializerStrategyAppenderOrFinalizer<TSerializable>
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
                if (this._dataGetter != serializer.DataGetter)
                {
                    continue;
                }
                if (this._dataSetter != serializer.DataSetter)
                {
                    continue;
                }
                Boolean next = false;
                foreach (KeyValuePair<Type, ISerializationDeserializationStrategy<Byte[]>> kv in this._twoWayStrategies)
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