namespace Narumikazuchi.Serialization.Bytes;

[DebuggerDisplay("{Id}")]
internal readonly struct __HeaderTypeEntry : __IStreamWriteable<__HeaderTypeEntry>
{
    public void WriteToStream<TStream>(TStream stream)
        where TStream : IWriteableStream
    {
        stream.Write(BitConverter.GetBytes(this.Id));
        stream.Write(BitConverter.GetBytes(this.NameId));
        stream.Write(BitConverter.GetBytes(this.AssemblyId));
        this.Properties.WriteToStream(stream);
    }

    public static __HeaderTypeEntry FromStream<TStream>(TStream stream,
                                                        ref UInt64 read)
        where TStream : IReadableStream
    {
        UInt32 id = stream.ReadUInt32(ref read);
        UInt32 nameId = stream.ReadUInt32(ref read);
        UInt32 assemblyId = stream.ReadUInt32(ref read);
        List<__HeaderTypeProperty> properties = stream.ReadList<__HeaderTypeProperty, TStream>(ref read);
        return new()
        {
            Id = id,
            NameId = nameId,
            AssemblyId = assemblyId,
            Properties = properties
        };
    }

    [return: NotNull]
    public Type CreateType(Dictionary<String, __HeaderNameEntry> names)
    {
        String typename = $"{names.ElementAt((Int32)this.NameId).Key}, {names.ElementAt((Int32)this.AssemblyId).Key}";
        return Type.GetType(typename, false)!;
    }

    internal UInt32 Id
    {
        get;
        init;
    }

    internal UInt32 NameId
    {
        get;
        init;
    }

    internal UInt32 AssemblyId
    {
        get;
        init;
    }

    internal List<__HeaderTypeProperty> Properties
    {
        get;
        init;
    }
}