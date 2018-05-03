using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Directives
{
    public class BuyDirectivePayload
    {
        public BuyDirectivePayload(string productId)
        {
            InSkillProduct = new BuyDirectivePayloadProduct(productId);
        }

        [JsonProperty("InSkillProduct")] public BuyDirectivePayloadProduct InSkillProduct { get; set; }
    }
}