using Narumikazuchi.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTest;

public class Methods
{
    public static void DateOnlySetter(DateOnly value,
                                      ISerializationInfoAdder info)
    {
        info.AddState("Day",
                      value.Day);
        info.AddState("Month",
                      value.Month);
        info.AddState("Year",
                      value.Year);
    }

    public static DateOnly DateOnlyGetter(ISerializationInfoGetter info)
    {
        Int32 day = info.GetState<Int32>("Day");
        Int32 month = info.GetState<Int32>("Month");
        Int32 year = info.GetState<Int32>("Year");
        return new(year, month, day);
    }
}
