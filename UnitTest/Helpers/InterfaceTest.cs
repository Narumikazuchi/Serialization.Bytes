using Narumikazuchi.Serialization;
using System;

namespace UnitTest
{
    public partial struct InterfaceTest
    {
        public InterfaceTest()
        { }
        public InterfaceTest(Guid guid,
                             String name,
                             String description,
                             Int32 count,
                             Single rate,
                             Half small)
        {
            this.Guid = guid;
            this.Name = name;
            this.Description = description;
            this.Count = count;
            this.Rate = rate;
            this.Small = small;
        }

        public String Name { get; set; } = "Foo";
        public String Description { get; set; } = "Lorem ipsum colorem";
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Int32 Count { get; set; } = 128;
        public Single Rate { get; set; } = 0.96445f;
        public Half Small { get; set; } = (Half)0.45554f;
    }

    partial struct InterfaceTest : IDeserializable<InterfaceTest>
    {
        public static InterfaceTest ConstructFromSerializationData(ISerializationInfoGetter info)
        {
            return new(info.GetState<Guid>(nameof(Guid)),
                       info.GetState<String>(nameof(Name))!,
                       info.GetState<String>(nameof(Description))!,
                       info.GetState<Int32>(nameof(Count)),
                       info.GetState<Single>(nameof(Rate)),
                       info.GetState<Half>(nameof(Small)));
        }
    }

    partial struct InterfaceTest : IEquatable<InterfaceTest>
    {
        public Boolean Equals(InterfaceTest other) =>
            this.Guid == other.Guid &&
            this.Name == other.Name &&
            this.Description == other.Description &&
            this.Count == other.Count &&
            this.Rate == other.Rate &&
            this.Small == other.Small;
    }

    partial struct InterfaceTest : ISerializable
    {
        public void GetSerializationData(ISerializationInfoAdder info)
        {
            info.AddState(nameof(this.Guid),
                     this.Guid);
            info.AddState(nameof(this.Name),
                     this.Name);
            info.AddState(nameof(this.Description),
                     this.Description);
            info.AddState(nameof(this.Count),
                     this.Count);
            info.AddState(nameof(this.Rate),
                     this.Rate);
            info.AddState(nameof(this.Small),
                     this.Small);
        }
    }
}
