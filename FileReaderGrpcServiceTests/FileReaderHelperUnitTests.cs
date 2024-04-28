using FileReaderGrpcService.Services;
using Google.Protobuf;

namespace FileReaderGrpcServiceTests
{
    public class FileReaderHelperUnitTests
    {
        [Fact]

        public void ReadingFileInChunksTest_ReturnsChunks()
        {
            // Arrange
            var filepath = @"C:\Temp\testData.txt";

            ByteString byteStringData = ByteString.CopyFromUtf8("quick brown fox ");//16chars
            ByteString[] chunks = new ByteString[16];
            chunks[0] = byteStringData;
            chunks[1] = byteStringData;

            using (var fileStream = new FileStream(filepath, FileMode.Append))
            {
                fileStream.Write(chunks[0].ToByteArray(), 0, chunks[0].Length);
                fileStream.Write(chunks[1].ToByteArray(), 0, chunks[1].Length);
            }

            FileReaderHelper fileReaderHelper = new FileReaderHelper();

            // Act
            var result = fileReaderHelper.ReadFileInChunks(filepath, 16);

            // Assert

            int count = 0;
            foreach (var item in result)
            {
                if (count == 2)
                    break;
                Assert.Equal(chunks[count++], item);
            }

            File.Delete(filepath);
        }


    }
}
