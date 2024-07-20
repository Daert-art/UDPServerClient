using System.Net;
using UDPServerClient.Entities;

namespace UDPServerClient.UseCases
{
    public interface IClientManager
    {
        bool TryAddClient(IPEndPoint remoteEP, out ClientRequestInfo clientInfo);
        void RemoveInactiveClients();
        bool IsClientActive(IPEndPoint remoteEP);
    }
}
