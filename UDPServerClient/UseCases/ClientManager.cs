using System.Net;
using UDPServerClient.Entities;

namespace UDPServerClient.UseCases
{
    public class ClientManager : IClientManager
    {
        private readonly Dictionary<IPEndPoint, ClientRequestInfo> _clientRequestInfos = new Dictionary<IPEndPoint, ClientRequestInfo>();
        private readonly int _maxClients = 100; // Максимальное количество подключений
        private readonly TimeSpan _inactiveThreshold = TimeSpan.FromMinutes(10);

        public bool TryAddClient(IPEndPoint remoteEP, out ClientRequestInfo clientInfo)
        {
            RemoveInactiveClients();

            if (_clientRequestInfos.Count >= _maxClients)
            {
                clientInfo = null;
                return false;
            }

            if (!_clientRequestInfos.TryGetValue(remoteEP, out clientInfo))
            {
                clientInfo = new ClientRequestInfo();
                _clientRequestInfos[remoteEP] = clientInfo;
            }

            clientInfo.LastActive = DateTime.Now;
            return true;
        }

        public void RemoveInactiveClients()
        {
            DateTime now = DateTime.Now;
            var inactiveClients = _clientRequestInfos
                .Where(entry => (now - entry.Value.LastActive) > _inactiveThreshold)
                .Select(entry => entry.Key)
                .ToList();

            foreach (var client in inactiveClients)
            {
                _clientRequestInfos.Remove(client);
            }
        }

        public bool IsClientActive(IPEndPoint remoteEP)
        {
            return _clientRequestInfos.TryGetValue(remoteEP, out var clientInfo)
                && (DateTime.Now - clientInfo.LastActive) <= _inactiveThreshold;
        }
    }
}
