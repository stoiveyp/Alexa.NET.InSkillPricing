using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Responses
{
    public class ConnectionResponsePayload
    {
        [JsonProperty("purchaseResult")]
        public string PurchaseResult { get; set; }

        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}