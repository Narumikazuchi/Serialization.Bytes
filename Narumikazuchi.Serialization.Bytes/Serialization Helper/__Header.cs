namespace Narumikazuchi.Serialization.Bytes;

internal class __Header
{
    private __Header()
    { }
    public __Header(ISerializationInfo info)
    {
        ExceptionHelpers.ThrowIfArgumentNull(info);
        ExceptionHelpers.ThrowIfNullOrEmpty(info.Type.AssemblyQualifiedName,
                                            nameof(info.Type));

        this.Typename = info.Type.AssemblyQualifiedName!;
        this.IsNull = info.IsNull;
    }
    public __Header(Type type,
                    Object? value)
    {
        ExceptionHelpers.ThrowIfArgumentNull(type);
        ExceptionHelpers.ThrowIfNullOrEmpty(type.AssemblyQualifiedName,
                                            nameof(type));

        this.Typename = type.AssemblyQualifiedName!;
        this.IsNull = value is null;
    }

    public static __Header FromStream(Stream source,
                                      Int64 size,
                                      out UInt64 read)
    {
        __Header result = new();

        Byte[] data = new Byte[size];
        Int64 index = 0;
        while (index < size)
        {
            Int32 b = source.ReadByte();
            if (b == -1)
            {
                // Unexpected end
                throw new IOException();
            }
            data[index++] = (Byte)b;
        }
        read = Convert.ToUInt64(size);

        Int32 offset = 0;

        Byte nullValue = data[offset++];
        if (nullValue == 1)
        {
            result.IsNull = true;
        }

        Int32 stringLength = BitConverter.ToInt32(data,
                                                  offset);
        offset += sizeof(Int32);
        String typename = Encoding.UTF8.GetString(data,
                                                  offset,
                                                  stringLength);
        offset += stringLength;
        result.Typename = typename;

        Int32 count = BitConverter.ToInt32(data, 
                                           offset);
        offset += sizeof(Int32);

        for (Int32 i = 0; i < count; i++)
        {
            __HeaderItem item = new();
            if (data[offset++] == 1)
            {
                item.IsNull = true;
            }
            item.Position = BitConverter.ToInt64(data,
                                                 offset);
            offset += sizeof(Int64);
            item.Length = BitConverter.ToInt64(data,
                                               offset);
            offset += sizeof(Int64);
            stringLength = BitConverter.ToInt32(data,
                                                offset);
            offset += sizeof(Int32);
            item.Typename = Encoding.UTF8.GetString(data, 
                                                    offset, 
                                                    stringLength);
            offset += stringLength;
            stringLength = BitConverter.ToInt32(data,
                                                offset);
            offset += sizeof(Int32);
            item.Name = Encoding.UTF8.GetString(data,
                                                offset,
                                                stringLength);
            offset += stringLength;
            result.Items.Add(item);
        }

        return result;
    }

    public MemoryStream AsMemory()
    {
        MemoryStream result = new();

        result.WriteByte(this.NullValue);
        result.Write(BitConverter.GetBytes(this.TypenameGlyphs));
        result.Write(this._typenameRaw);
        result.Write(BitConverter.GetBytes(this.MemberCount));
        for (Int32 i = 0; i < this.MemberCount; i++)
        {
            using MemoryStream item = this.Items[i]
                                          .AsMemory();
            item.CopyTo(result);
        }

        result.Position = 0;
        return result;
    }

    public Boolean IsNull { get; set; }
    public Int32 TypenameGlyphs => 
        this._typenameRaw
            .Length;
    public String Typename
    {
        get => this._typename;
        set
        {
            ExceptionHelpers.ThrowIfNullOrEmpty(value);

            this._typename = value;
            this._typenameRaw = Encoding.UTF8
                                        .GetBytes(value);
        }
    }
    public Int32 MemberCount => 
        this.Items
            .Count;
    public Int64 Size
    {
        get
        {
            using MemoryStream temp = this.AsMemory();
            return temp.Length;
        }
    }
    public IList<__HeaderItem> Items { get; } = new List<__HeaderItem>();

    private Byte NullValue
    {
        get
        {
            if (this.IsNull)
            {
                return 1;
            }
            return 0;
        }
    }

    private String _typename = String.Empty;
    private Byte[] _typenameRaw = Array.Empty<Byte>();
}