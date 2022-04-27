namespace Narumikazuchi.Serialization.Bytes;

internal class __HeaderItem
{
    public MemoryStream AsMemory()
    {
        MemoryStream result = new();

        result.WriteByte(this.NullValue);
        result.Write(BitConverter.GetBytes(this.Position));
        result.Write(BitConverter.GetBytes(this.Length));
        result.Write(BitConverter.GetBytes(this.TypenameGlyphs));
        result.Write(m_TypenameRaw);
        result.Write(BitConverter.GetBytes(this.NameGlyphs));
        result.Write(m_NameRaw);

        result.Position = 0;
        return result;
    }

    public Int64 Position { get; set; }

    public Int64 Length { get; set; }

    public Boolean IsNull { get; set; }

    public Int32 TypenameGlyphs => 
        m_TypenameRaw.Length;

    public String Typename
    {
        get => m_Typename;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            m_Typename = value;
            m_TypenameRaw = Encoding.UTF8.GetBytes(value);
        }
    }

    public Int32 NameGlyphs => 
        m_NameRaw.Length;

    public String Name
    {
        get => m_Name;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            m_Name = value;
            m_NameRaw = Encoding.UTF8
                                .GetBytes(s: value);
        }
    }

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

    private String m_Typename = String.Empty;
    private String m_Name = String.Empty;
    private Byte[] m_TypenameRaw = Array.Empty<Byte>();
    private Byte[] m_NameRaw = Array.Empty<Byte>();
}