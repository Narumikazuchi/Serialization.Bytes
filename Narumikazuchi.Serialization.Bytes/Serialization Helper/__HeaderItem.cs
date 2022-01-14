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
        result.Write(this._typenameRaw);
        result.Write(BitConverter.GetBytes(this.NameGlyphs));
        result.Write(this._nameRaw);

        result.Position = 0;
        return result;
    }

    public Int64 Position { get; set; }
    public Int64 Length { get; set; }
    public Boolean IsNull { get; set; }
    public Int32 TypenameGlyphs => this._typenameRaw.Length;
    public String Typename
    {
        get => this._typename;
        set
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            this._typename = value;
            this._typenameRaw = Encoding.UTF8.GetBytes(value);
        }
    }
    public Int32 NameGlyphs => this._nameRaw.Length;
    public String Name
    {
        get => this._name;
        set
        {
            if (String.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }
            this._name = value;
            this._nameRaw = Encoding.UTF8.GetBytes(value);
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

    private String _typename = String.Empty;
    private String _name = String.Empty;
    private Byte[] _typenameRaw = Array.Empty<Byte>();
    private Byte[] _nameRaw = Array.Empty<Byte>();
}