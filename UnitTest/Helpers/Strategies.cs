using Narumikazuchi;
using Narumikazuchi.Serialization;
//using Narumikazuchi.Serialization.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest;

public partial class TimeOnlyStrategy : Singleton, ISerializationDeserializationStrategy<Byte[], TimeOnly>
{
    private TimeOnlyStrategy()
    { }
}

partial class TimeOnlyStrategy : IDeserializationStrategy<Byte[]>
{
    Object? IDeserializationStrategy<Byte[]>.Deserialize(Byte[] input) =>
        this.Deserialize(input);
}


partial class TimeOnlyStrategy : IDeserializationStrategy<Byte[], TimeOnly>
{
    public TimeOnly Deserialize(Byte[] input) =>
        new(BitConverter.ToInt64(input));
}

partial class TimeOnlyStrategy : ISerializationStrategy<Byte[]>
{
    Byte[] ISerializationStrategy<Byte[]>.Serialize(Object? input) =>
        this.Serialize((TimeOnly)input!);
}


partial class TimeOnlyStrategy : ISerializationStrategy<Byte[], TimeOnly>
{
    public Byte[] Serialize(TimeOnly input) => 
        BitConverter.GetBytes(input.Ticks);
}

//partial class TimeOnlyStrategy : ISerializationStrategy<JsonElement>
//{
//    Object? ISerializationStrategy<JsonElement>.Deserialize(JsonElement input) =>
//        this.Deserialize(input);

//    JsonElement ISerializationStrategy<JsonElement>.Serialize(Object? input) =>
//        this.Serialize((TimeOnly)input!);
//}


//partial class TimeOnlyStrategy : ISerializationStrategy<JsonElement, TimeOnly>
//{
//    public TimeOnly Deserialize(JsonElement input) =>
//        new(BitConverter.ToInt64(input));

//    public JsonElement Serialize(TimeOnly input) =>
//        BitConverter.GetBytes(input.Ticks);
//}