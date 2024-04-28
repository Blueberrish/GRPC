Project Type :Asp.netcore.GRPC service
FileReaderService :This is microservice which reads the file in chunks and writes to the stream in chunks
which can be read by the clients

FileReaderHelper: This helper class holds the logic to read the file in chunk

filereader.proto : Defines the contract between the client and server for the communication.

Unit tests are implemented using Xunit and Moq
