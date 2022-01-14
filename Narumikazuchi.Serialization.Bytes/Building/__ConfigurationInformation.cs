namespace Narumikazuchi.Serialization.Bytes;

internal sealed partial class __ConfigurationInformation
{ }

#pragma warning disable CS8617
// IByteDeserializerTypeConfigurator
partial class __ConfigurationInformation : IByteDeserializerTypeConfigurator
{
    IByteDeserializerDefaultStrategyAppender<TSerializable> IByteDeserializerTypeConfigurator.ConfigureForForeignType<TSerializable>(Func<ISerializationInfoGetter, TSerializable> constructFromSerializationData)
    {
        __BuildInformation<TSerializable> result = new()
        {
            DataSetter = constructFromSerializationData
        };
        return result;
    }
    IByteDeserializerDefaultStrategyAppender<TSerializable> IByteDeserializerTypeConfigurator.ConfigureForForeignType<TSerializable>(IDeserializationStrategy<Byte[], TSerializable> strategy)
    {
        __BuildInformation<TSerializable> result = new();
        result.DeserializationStrategies
              .Add(key: typeof(TSerializable),
                   value: strategy);
        return result;
    }

    IByteDeserializerDefaultStrategyAppender<TSerializable> IByteDeserializerTypeConfigurator.ConfigureForOwnedType<TSerializable>()
    {
        __BuildInformation<TSerializable> result = new();
        return result;
    }
}

// IByteSerializerTypeConfigurator
partial class __ConfigurationInformation : IByteSerializerTypeConfigurator
{
    IByteSerializerDefaultStrategyAppender<TSerializable> IByteSerializerTypeConfigurator.ConfigureForForeignType<TSerializable>(Action<TSerializable, ISerializationInfoAdder> getSerializationData)
    {
        __BuildInformation<TSerializable> result = new()
        {
            DataGetter = getSerializationData
        };
        return result;
    }
    IByteSerializerDefaultStrategyAppender<TSerializable> IByteSerializerTypeConfigurator.ConfigureForForeignType<TSerializable>(ISerializationStrategy<Byte[], TSerializable> strategy)
    {
        __BuildInformation<TSerializable> result = new();
        result.SerializationStrategies
              .Add(key: typeof(TSerializable),
                   value: strategy);
        return result;
    }

    IByteSerializerDefaultStrategyAppender<TSerializable> IByteSerializerTypeConfigurator.ConfigureForOwnedType<TSerializable>()
    {
        __BuildInformation<TSerializable> result = new();
        return result;
    }
}

// IByteSerializerDeserializerTypeConfigurator
partial class __ConfigurationInformation : IByteSerializerDeserializerTypeConfigurator
{
    IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> IByteSerializerDeserializerTypeConfigurator.ConfigureForForeignType<TSerializable>(Action<TSerializable, ISerializationInfoAdder> getSerializationData, 
                                                                                                                                                         Func<ISerializationInfoGetter, TSerializable> constructFromSerializationData)
    {
        __BuildInformation<TSerializable> result = new()
        {
            DataGetter = getSerializationData,
            DataSetter = constructFromSerializationData
        };
        return result;
    }
    IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> IByteSerializerDeserializerTypeConfigurator.ConfigureForForeignType<TSerializable>(ISerializationDeserializationStrategy<Byte[], TSerializable> strategy)
    {
        __BuildInformation<TSerializable> result = new();
        result.TwoWayStrategies
              .Add(key: typeof(TSerializable),
                   value: strategy);
        return result;
    }

    IByteSerializerDeserializerDefaultStrategyAppender<TSerializable> IByteSerializerDeserializerTypeConfigurator.ConfigureForOwnedType<TSerializable>()
    {
        __BuildInformation<TSerializable> result = new();
        return result;
    }
}