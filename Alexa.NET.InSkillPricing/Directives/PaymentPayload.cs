using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Directives
{
    public class PaymentPayload
    {
        public PaymentPayload()
        {
        }

        public PaymentPayload(string productId, string upsellMessage)
        {
            InSkillProduct = new PaymentPayloadProduct(productId);
            UpsellMessage = upsellMessage;
        }

        public PaymentPayload(string productId)
        {
            InSkillProduct = new PaymentPayloadProduct(productId);
            UpsellMessage = null;
        }


        [JsonProperty("InSkillProduct")] public PaymentPayloadProduct InSkillProduct { get; set; }

        [JsonProperty("upsellMessage",NullValueHandling = NullValueHandling.Ignore)] public string UpsellMessage { get; set; }
    }
}