namespace Narumikazuchi.Serialization.Bytes;

[Flags]
internal enum __HeaderFlags : Byte
{
    None = 0x0,
    NotNull = 0x1,
    IsNull = 0x2,
    IsEnumerated = 0x4
}