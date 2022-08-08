namespace Narumikazuchi.Serialization.Bytes;

[DebuggerDisplay("{Name}")]
internal readonly struct __HeaderNameEntry : __IStreamWriteable<__HeaderNameEntry>
{
    public void WriteToStream<TStream>(TStream stream)
        where TStream : IWriteableStream
    {
        stream.Write(BitConverter.GetBytes(this.Id));
        stream.Write(BitConverter.GetBytes(this.ByteCount));
        stream.Write(this.NameBytes);
    }

    public static __HeaderNameEntry FromStream<TStream>(TStream stream,
                                                        ref UInt64 read)
        where TStream : IReadableStream
    {
        UInt32 id = stream.ReadUInt32(ref read);
        Int32 byteCount = stream.ReadInt32(ref read);
        Byte[] nameBytes = new Byte[byteCount];
        stream.Read(nameBytes);
        read += (UInt64)byteCount;
        return new()
        {
            Id = id,
            Name = Encoding.UTF8.GetString(nameBytes)
        };
    }

    internal UInt32 Id
    {
        get;
        init;
    }

    internal Int32 ByteCount =>
        m_NameBytes.Length;

    internal String Name
    {
        get => m_Name;
        init
        {
            ArgumentNullException.ThrowIfNull(nameof(value));

            m_Name = value;
            m_NameBytes = Encoding.UTF8.GetBytes(value);
        }
    }

    internal Byte[] NameBytes => 
        m_NameBytes;

    private readonly String m_Name;
    private readonly Byte[] m_NameBytes;
}