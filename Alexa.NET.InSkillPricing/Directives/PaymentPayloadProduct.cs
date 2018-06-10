using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Directives
{
    public class PaymentPayloadProduct
    {
        public PaymentPayloadProduct() { }

        public PaymentPayloadProduct(string productId)
        {
            ProductId = productId;
        }

        [JsonProperty("productId")] public string ProductId { get; private set; }
    }
}