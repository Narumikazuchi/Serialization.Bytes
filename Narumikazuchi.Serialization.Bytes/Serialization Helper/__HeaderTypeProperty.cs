namespace Narumikazuchi.Serialization.Bytes;

[DebuggerDisplay("NameId: {NameId}    TypeId:{TypeId}")]
internal readonly struct __HeaderTypeProperty : __IStreamWriteable<__HeaderTypeProperty>
{
    public void WriteToStream<TStream>(TStream stream)
        where TStream : IWriteableStream
    {
        stream.Write(BitConverter.GetBytes(this.NameId));
        stream.Write(BitConverter.GetBytes(this.TypeId));
    }

    public static __HeaderTypeProperty FromStream<TStream>(TStream stream,
                                                           ref UInt64 read)
        where TStream : IReadableStream
    {
        Int32 nameId = stream.ReadInt32(ref read);
        UInt32 typeId = stream.ReadUInt32(ref read);
        return new()
        {
            NameId = nameId,
            TypeId = typeId
        };
    }

    internal Int32 NameId
    {
        get;
        init;
    }

    internal UInt32 TypeId
    {
        get;
        init;
    }
}