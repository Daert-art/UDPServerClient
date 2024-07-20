using UDPServerClient.Entities;

namespace UDPServerClient.UseCases
{
    public interface IRequestLimiter
    {
        bool IsRequestAllowed(ClientRequestInfo clientInfo);
    }
}
