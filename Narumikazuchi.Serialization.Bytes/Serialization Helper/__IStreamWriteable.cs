namespace Narumikazuchi.Serialization.Bytes;

internal interface __IStreamWriteable<T>
{
#if OUTPUT

    public void WriteToStream(StreamWriter writer);

    public static abstract T FromStream(Stream stream,
                                        ref UInt64 read);

#else

    public void WriteToStream<TStream>(TStream stream)
        where TStream : IWriteableStream;

    public static abstract T FromStream<TStream>(TStream stream,
                                                 ref UInt64 read)
        where TStream : IReadableStream;

#endif
}