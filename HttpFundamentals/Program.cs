using HttpFundamentals;

BasicHttpServer server = new();
server.Start();

Console.WriteLine("press any key to exit ...");
Console.ReadKey();