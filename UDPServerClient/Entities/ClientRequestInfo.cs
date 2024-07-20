namespace UDPServerClient.Entities
{
    public class ClientRequestInfo
    {
        public List<DateTime> Requests { get; } = new List<DateTime>();
        public DateTime LastActive { get; set; } = DateTime.Now;
    }
}
