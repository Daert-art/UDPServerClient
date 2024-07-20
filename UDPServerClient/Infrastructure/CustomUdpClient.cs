using System.Net;
using System.Net.Sockets;
using System.Text;
using UDPServerClient.Interfaces;

namespace UDPServerClient.Infrastructure
{
    public class CustomUdpClient : IClient
    {
        private readonly UdpClient _udpClient;
        private readonly IPEndPoint _serverEndPoint;

        public CustomUdpClient()
        {
            _udpClient = new UdpClient();
            _serverEndPoint = new IPEndPoint(IPAddress.Loopback, 8080);
            _udpClient.Connect(_serverEndPoint);
        }

        public void Start()
        {
            while (true)
            {
                Console.Write("Enter component name (or 'exit' to quit): ");
                string componentName = Console.ReadLine();

                if (componentName.ToLower() == "exit")
                {
                    break;
                }

                bool success = false;
                int attempt = 0;
                int maxAttempts = 5;
                TimeSpan delay = TimeSpan.FromSeconds(2);

                while (attempt < maxAttempts && !success)
                {
                    try
                    {
                        byte[] data = Encoding.UTF8.GetBytes(componentName);
                        _udpClient.Send(data, data.Length);

                        IPEndPoint remoteEP = null;
                        byte[] responseBytes = _udpClient.Receive(ref remoteEP);
                        string response = Encoding.UTF8.GetString(responseBytes);

                        Console.WriteLine($"Server response: {response}");
                        success = true;
                    }
                    catch (SocketException)
                    {
                        Console.WriteLine("Server not reachable. Retrying...");
                        attempt++;
                        Thread.Sleep(delay);
                    }
                }

                if (!success)
                {
                    Console.WriteLine("Failed to contact the server after several attempts.");
                }
            }

            _udpClient.Close();
        }
    }
}
