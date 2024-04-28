using Grpc.Core;

namespace FileReaderGrpcService.Services
{
    public class FileReaderService : FileReader.FileReaderBase
    {
        private readonly ILogger<FileReaderService> _logger;
        private readonly IFileReader _fileReaderHelper;
        private const int bytesPerRead = 16;
        public FileReaderService(ILogger<FileReaderService> logger, IFileReader fileReaderHelper)
        {
            _logger = logger;
            _fileReaderHelper = fileReaderHelper;
        }

        public override async Task GetFileBytes(GetFileBytesRequest request, IServerStreamWriter<GetFileBytesResponse> responseStream, ServerCallContext context)
        {
            _logger.Log(LogLevel.Information, $"{nameof(GetFileBytes)} started.");

            var filepath = request.InputString;
            ValidateFilepath(filepath);

            try
            {
                foreach (var chunk in _fileReaderHelper.ReadFileInChunks(filepath, bytesPerRead))
                {
                    GetFileBytesResponse response = new GetFileBytesResponse
                    {
                        Data = chunk
                    };

                    await responseStream.WriteAsync(response);
                }

            }

            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"{nameof(GetFileBytes)} exception is {ex.Message}.");
                throw new RpcException(new Status(StatusCode.Internal, "Internal server error"), ex.Message);
            }
            finally
            {
                _logger.Log(LogLevel.Information, $"{nameof(GetFileBytes)} completed.");
            }
        }

        private void ValidateFilepath(string filepath)
        {
            if (string.IsNullOrEmpty(filepath) || string.IsNullOrWhiteSpace(filepath))
            {
                _logger.Log(LogLevel.Warning, $"{nameof(ValidateFilepath)} filepath is empty.");
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Filename is required.It cannot be blank."));
            }
            if (!File.Exists(filepath))
            {
                _logger.Log(LogLevel.Warning, $"{nameof(ValidateFilepath)} file does not exist.");
                throw new RpcException(new Status(StatusCode.NotFound, "File not found"));
            }
        }
    }
}
