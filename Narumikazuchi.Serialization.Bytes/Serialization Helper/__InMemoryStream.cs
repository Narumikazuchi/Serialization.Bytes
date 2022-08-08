namespace Narumikazuchi.Serialization.Bytes;

internal partial class __InMemoryStream
{
    internal __InMemoryStream()
    {
        m_Stream = new();
    }

    private readonly MemoryStream m_Stream;
}

// IAsyncDisposable
partial class __InMemoryStream : IAsyncDisposable
{
    public ValueTask DisposeAsync() =>
        m_Stream.DisposeAsync();
}

// IContainedStream
partial class __InMemoryStream : IContainedStream
{
    public Int64 Length =>
        m_Stream.Length;

    public Int64 Position
    {
        get => m_Stream.Position;
        set => m_Stream.Position = value;
    }
}

// IDisposable
partial class __InMemoryStream : IDisposable
{
    public void Dispose() =>
        m_Stream.Dispose();
}

// IReadableStream
partial class __InMemoryStream : IReadableStream
{
    public void CopyTo<TStream>([DisallowNull] TStream destination,
                                Int32 bufferSize)
        where TStream : IWriteableStream
    {
        ArgumentNullException.ThrowIfNull(destination);

        Byte[] buffer = new Byte[bufferSize];
        Int32 read = m_Stream.Read(buffer: buffer);
        while (read != 0)
        {
            destination.Write(buffer.AsSpan()[..read]);
            read = m_Stream.Read(buffer: buffer);
        }
    }

    public async ValueTask CopyToAsync<TStream>([DisallowNull] TStream destination,
                                                Int32 bufferSize,
                                                CancellationToken cancellationToken)
        where TStream : IWriteableStream
    {
        ArgumentNullException.ThrowIfNull(destination);

        Byte[] buffer = new Byte[bufferSize];
        Int32 read = await m_Stream.ReadAsync(buffer: buffer,
                                              cancellationToken: cancellationToken);
        while (read != 0)
        {
            await destination.WriteAsync(buffer: buffer.AsMemory()[..read],
                                         cancellationToken: cancellationToken);
            read = await m_Stream.ReadAsync(buffer: buffer,
                                              cancellationToken: cancellationToken);
        }
    }

    public Int32 Read(Span<Byte> buffer) =>
        m_Stream.Read(buffer);

    public ValueTask<Int32> ReadAsync(Memory<Byte> buffer,
                                      CancellationToken cancellationToken) =>
        m_Stream.ReadAsync(buffer: buffer,
                           cancellationToken: cancellationToken);

    public Boolean ReadByte([NotNullWhen(true)] out Byte? value)
    {
        Int32 result = m_Stream.ReadByte();
        if (result != -1)
        {
            value = (Byte)result;
            return true;
        }
        else
        {
            value = default;
            return false;
        }
    }
}

// IStream
partial class __InMemoryStream : IStream
{
    public void Close() =>
        m_Stream.Close();
}

// IWriteableStream
partial class __InMemoryStream : IWriteableStream
{
    /// <inheritdoc/>
    public void Flush() =>
        m_Stream.Flush();

    /// <inheritdoc/>
    public async ValueTask FlushAsync() =>
        await m_Stream.FlushAsync();

    /// <inheritdoc/>
    public void Write(ReadOnlySpan<Byte> buffer) =>
        m_Stream.Write(buffer: buffer);

    /// <inheritdoc/>
    public ValueTask WriteAsync(ReadOnlyMemory<Byte> buffer,
                                CancellationToken cancellationToken) =>
        m_Stream.WriteAsync(buffer: buffer,
                            cancellationToken: cancellationToken);

    /// <inheritdoc/>
    public void WriteByte(Byte value) =>
        m_Stream.WriteByte(value);

    /// <inheritdoc/>
    public void SetLength(Int64 length) =>
        m_Stream.SetLength(length);
}