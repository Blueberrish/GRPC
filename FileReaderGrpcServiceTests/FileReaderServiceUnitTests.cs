using FileReaderGrpcService;
using FileReaderGrpcService.Services;
using Google.Protobuf;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using Moq;

namespace FileReaderGrpcServiceTests
{
    public class FileReaderServiceUnitTests
    {

        [InlineData(" ")]
        [InlineData("")]
        [Theory]
        public void IsInputStringValidTest_ReturnsException(string filepath)
        {
            var fileReaderMock = new Mock<IFileReader>();
            var logger = Mock.Of<ILogger<FileReaderService>>();
            FileReaderService fileReaderService = new FileReaderService(logger, fileReaderMock.Object);
            var request = CreateRequest(filepath);


            Assert.ThrowsAsync<RpcException>(() => fileReaderService.GetFileBytes(request, null, null));

        }



        [Fact]
        public void FileReadTest_ReturnsData()
        {
            //arrange
            var filepath = @"C:\temp\test.txt";
            var fileReaderMock = new Mock<IFileReader>();
            var logger = Mock.Of<ILogger<FileReaderService>>();
            var streamServerWriter = new Mock<IServerStreamWriter<GetFileBytesResponse>>();
            var serverCallContextMock = new Mock<ServerCallContext>();

            ByteString[] bytes = new ByteString[16];
            ByteString xyz = ByteString.CopyFromUtf8("quick brown fox jumps on lazy rabbit");
            bytes[0] = xyz;
            bytes[1] = xyz;

            GetFileBytesResponse response = new GetFileBytesResponse() { Data = xyz };
            fileReaderMock.Setup(x => x.ReadFileInChunks(filepath, 16)).Returns(bytes);
            //  streamServerWriter.Setup(x => x.WriteAsync(response)).Returns((GetFileBytesResponse)response);

            FileReaderService fileReaderService = new FileReaderService(logger, fileReaderMock.Object);
            GetFileBytesRequest request = CreateRequest(filepath);
            CreateFile(filepath);

            //Act assert

            using (var call = fileReaderService.GetFileBytes(request, streamServerWriter.Object, serverCallContextMock.Object))
            {

                fileReaderMock.Verify(v => v.ReadFileInChunks(@"C:\temp\test.txt", 16));
                streamServerWriter.Verify(v => v.WriteAsync(response), Times.AtLeastOnce());
            }

            DeleteFile(filepath);
        }

        private static GetFileBytesRequest CreateRequest(string filepath)
        {
            return new GetFileBytesRequest
            {
                InputString = filepath
            };
        }

        private void CreateFile(string filepath)
        {
            if (!File.Exists(filepath))
            {
                File.Create(filepath);
            }
        }

        private void DeleteFile(string filepath)
        {
            if (File.Exists(filepath))
            {
                File.Delete(filepath);
            }
        }
    }
}