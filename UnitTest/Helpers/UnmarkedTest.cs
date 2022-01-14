using Narumikazuchi.Serialization;
using System;

namespace UnitTest
{
    public sealed class UnmarkedTest
    {
        public static void GetInfo(UnmarkedTest? obj, ISerializationInfoAdder info)
        {
            if (obj is null)
            {
                return;
            }
            info.AddState(nameof(Name),
                     obj.Name);
            info.AddState(nameof(Description),
                     obj.Description);
            info.AddState(nameof(Guid),
                     obj.Guid);
            info.AddState(nameof(Count),
                     obj.Count);
            info.AddState(nameof(Rate),
                     obj.Rate);
            info.AddState(nameof(Small),
                     obj.Small);
            info.AddState(nameof(Interface),
                     obj.Interface);
        }

        public static UnmarkedTest? FromInfo(ISerializationInfoGetter info)
        {
            if (info.IsNull)
            {
                return null;
            }
            UnmarkedTest result = new();
            result.Name = info.GetState<String>(nameof(Name))!;
            result.Description = info.GetState<String>(nameof(Description))!;
            result.Guid = info.GetState<Guid>(nameof(Guid));
            result.Count = info.GetState<Int32>(nameof(Count));
            result.Rate = info.GetState<Single>(nameof(Rate));
            result.Small = info.GetState<Half>(nameof(Small));
            result.Interface = info.GetState<InterfaceTest>(nameof(Interface));
            return result;
        }

        public override Boolean Equals(Object? obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj is not UnmarkedTest other)
            {
                return false;
            }
            return this.Guid == other.Guid &&
                   this.Interface.Equals(other.Interface);
        }

        public String Name { get; set; } = "Foo";
        public String Description { get; set; } = "Lorem ipsum colorem";
        public Guid Guid { get; set; } = Guid.NewGuid();
        public Int32 Count { get; set; } = 128;
        public Single Rate { get; set; } = 0.96445f;
        public Half Small { get; set; } = (Half)0.45554f;
        public InterfaceTest Interface { get; set; }
    }
}
