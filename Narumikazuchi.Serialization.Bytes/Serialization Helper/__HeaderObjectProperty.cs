namespace Narumikazuchi.Serialization.Bytes;

[DebuggerDisplay("{Flags}")]
internal readonly struct __HeaderObjectProperty : __IStreamWriteable<__HeaderObjectProperty>
{
    public void WriteToStream<TStream>(TStream stream)
        where TStream : IWriteableStream
    {
        stream.WriteByte((Byte)this.Flags);
        stream.Write(BitConverter.GetBytes(this.NameId));
        stream.Write(BitConverter.GetBytes(this.TypeId));
        stream.Write(BitConverter.GetBytes(this.Position));
        stream.Write(BitConverter.GetBytes(this.Length));
        this.Properties.WriteToStream(stream);
    }

    public static __HeaderObjectProperty FromStream<TStream>(TStream stream,
                                                             ref UInt64 read)
        where TStream : IReadableStream
    {
        if (!stream.ReadByte(out Byte? singleByte))
        {
            throw new IOException();
        }

        __HeaderFlags flags = (__HeaderFlags)singleByte;
        read++;
        UInt32 nameId = stream.ReadUInt32(ref read);
        UInt32 typeId = stream.ReadUInt32(ref read);
        Int64 position = stream.ReadInt64(ref read);
        Int64 length = stream.ReadInt64(ref read);
        List<__HeaderObjectProperty> properties = stream.ReadList<__HeaderObjectProperty, TStream>(ref read);
        return new()
        {
            Flags = flags,
            NameId = nameId,
            TypeId = typeId,
            Position = position,
            Length = length,
            Properties = properties
        };
    }

    internal __HeaderFlags Flags
    {
        get;
        init;
    }

    internal UInt32 NameId
    {
        get;
        init;
    }

    internal UInt32 TypeId
    {
        get;
        init;
    }

    internal Int64 Position
    {
        get;
        init;
    }

    internal Int64 Length
    {
        get;
        init;
    }

    internal List<__HeaderObjectProperty> Properties
    {
        get;
        init;
    }
}