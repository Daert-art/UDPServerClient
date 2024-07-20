using System.Net;
using System.Net.Sockets;
using System.Text;
using UDPServerClient.Interfaces;
using UDPServerClient.UseCases;

namespace UDPServerClient.Infrastructure
{
    public class UdpServer : IServer
    {
        private readonly UdpClient _udpClient;
        private readonly HandleClientRequestUseCase _handleClientRequestUseCase;
        private readonly IClientManager _clientManager;

        public UdpServer(HandleClientRequestUseCase handleClientRequestUseCase, IClientManager clientManager)
        {
            _udpClient = new UdpClient(8080);
            _handleClientRequestUseCase = handleClientRequestUseCase;
            _clientManager = clientManager;
        }

        public void Start()
        {
            Console.WriteLine("Server started...");
            while (true)
            {
                IPEndPoint remoteEP = null;
                byte[] data = _udpClient.Receive(ref remoteEP);
                string componentName = Encoding.UTF8.GetString(data);

                if (_clientManager.TryAddClient(remoteEP, out var clientInfo))
                {
                    clientInfo.LastActive = DateTime.Now;
                    string response = _handleClientRequestUseCase.HandleRequest(remoteEP, componentName, clientInfo);
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    _udpClient.Send(responseBytes, responseBytes.Length, remoteEP);
                }
                else
                {
                    string response = "Server is full. Please try again later.";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    _udpClient.Send(responseBytes, responseBytes.Length, remoteEP);
                }
            }
        }
    }
}
