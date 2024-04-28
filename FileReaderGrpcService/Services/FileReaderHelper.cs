using Google.Protobuf;

namespace FileReaderGrpcService.Services
{
    public class FileReaderHelper : IFileReader
    {
        public IEnumerable<ByteString> ReadFileInChunks(string filePath, int bytesPerRead)
        {
            using (var fileStream = new FileStream(filePath, FileMode.Open))
            {
                var buffer = new byte[bytesPerRead];
                int bytesRead;

                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
                    yield return ByteString.CopyFrom(buffer, 0, bytesRead);
                }
            }

        }
    }
}
