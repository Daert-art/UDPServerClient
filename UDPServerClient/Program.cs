using UDPServerClient.Infrastructure;
using UDPServerClient.Interfaces;
using UDPServerClient.UseCases;

class Program
{
    static void Main(string[] args)
    {
        var componentPriceService = new ComponentPriceService();
        var requestLimiter = new RequestLimiter();
        var clientManager = new ClientManager();
        var handleClientRequestUseCase = new HandleClientRequestUseCase(requestLimiter, componentPriceService);


        Console.WriteLine("Enter 'server' to run as server or 'client' to run as client:");
        string mode = Console.ReadLine().ToLower();

        if (mode == "server")
        {
            IServer server = new UdpServer(handleClientRequestUseCase, clientManager);
            server.Start();
        }
        else if (mode == "client")
        {
            IClient client = new CustomUdpClient();
            client.Start();
        }
        else
        {
            Console.WriteLine("Unknown mode. Please restart the application and enter 'server' or 'client'.");
        }
    }
}