// See https://aka.ms/new-console-template for more information


using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using ReaderGrpcClient;
using System.Collections;
using System.Text;

//string serviceEndpoint = "http://localhost:5277";
string serviceEndpoint = "https://localhost:7029";
var channel = GrpcChannel.ForAddress(serviceEndpoint);
var client = new FileReader.FileReaderClient(channel);

Console.WriteLine("Enter the Filename:");
string fileName = Console.ReadLine();
GetFileBytesRequest request = new GetFileBytesRequest
{
    InputString = fileName// "" //@"C:\temp\test.txt"
};

try
{
    // Call the streaming RPC method and get the response stream
    using (var call = client.GetFileBytes(request))
    {
        // Receive file chunks from the server
        await foreach (var chunk in call.ResponseStream.ReadAllAsync())
        {
            // Process each chunk, for example, write to a file
            ProcessChunk(chunk);
        }
    }
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.NotFound)
{
    Console.WriteLine("File not found: " + ex.Status.Detail);
}
catch (RpcException ex) when (ex.StatusCode == StatusCode.Internal)
{
    Console.WriteLine("Internal server error: " + ex.Status.Detail);
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}

// Shutdown the channel
await channel.ShutdownAsync();

static void ProcessChunk(GetFileBytesResponse chunk)
{
    // Write the chunk to a file or process it in any other way
    // For example:
    Console.WriteLine("Processing in chunks");
    using (var fileStream = new FileStream(@"C:\temp\temp2\test1.txt", FileMode.Append))
    {
        fileStream.Write(chunk.Data.ToByteArray(), 0, chunk.Data.Length);
    }
}
