using Google.Protobuf;

namespace FileReaderGrpcService.Services
{
    public interface IFileReader
    {
        IEnumerable<ByteString> ReadFileInChunks(string filePath, int bytesPerRead);
    }
}
