using UDPServerClient.Entities;

namespace UDPServerClient.UseCases
{
    public class RequestLimiter : IRequestLimiter
    {
        private readonly int _maxRequestsPerHour = 10;
        private readonly TimeSpan _timeWindow = TimeSpan.FromHours(1);

        public bool IsRequestAllowed(ClientRequestInfo clientInfo)
        {
            DateTime now = DateTime.Now;
            clientInfo.Requests.RemoveAll(time => (now - time) > _timeWindow);
            return clientInfo.Requests.Count < _maxRequestsPerHour;
        }
    }
}
