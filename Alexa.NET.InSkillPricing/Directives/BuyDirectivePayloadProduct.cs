using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Directives
{
    public class BuyDirectivePayloadProduct
    {
        public BuyDirectivePayloadProduct(string productId)
        {
            ProductId = productId;
        }

        [JsonProperty("productId")] public string ProductId { get; private set; }
    }
}