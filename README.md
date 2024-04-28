Project Type :Asp.netcore.GRPC service 
Nuget Packages installed: Grpc.AspNetCore(2.57.0), Grpc.AspNetCore.Server.Reflection(2.62.0)

FileReaderService :This is microservice which reads the file in chunks and writes to the stream in chunks
which can be read by the clients

FileReaderHelper: This helper class holds the logic to read the file in chunk

filereader.proto : Defines the contract between the client and server for the communication.

ReaderGrpcClient: Grpc client to test client server communication. Nuget packages: Google.protobuf(3.26.1),Grpc.Net.Client(2.62.0),Grpc.Tools(2.62.0)

Unit tests are implemented using Xunit and Moq. Nuget Packages installed: Moq(4.20.70),xunit(2.5.3),xunit.runner.visualStudio(2.5.3)
