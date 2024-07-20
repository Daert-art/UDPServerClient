namespace UDPServerClient.UseCases
{
    public class ComponentPriceService : IComponentPriceService
    {
        private readonly Dictionary<string, string> _componentsPrices = new Dictionary<string, string>()
        {
            { "processor", "200 USD" },
            { "graphics card", "500 USD" },
            { "motherboard", "150 USD" },
            { "ram", "80 USD" },
            { "hard drive", "100 USD" }
        };

        public string GetComponentPrice(string componentName)
        {
            return _componentsPrices.TryGetValue(componentName.ToLower(), out var price)
                ? price
                : "Component not found";
        }
    }
}
