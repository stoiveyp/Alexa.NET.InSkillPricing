using System.Text;
using Alexa.NET.Response;
using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Directives
{
    public class PaymentDirective:IDirective
    {
        public PaymentDirective() { }

        public PaymentDirective(string paymentType, string correlationToken, PaymentPayload payload)
        {
            Name = paymentType;
            Payload = payload;//new PaymentPayload(productId);
            Token = correlationToken;
        }

        [JsonProperty("type")]
        public string Type => "Connections.SendRequest";

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("token")] public string Token { get; set; }

        [JsonProperty("payload")] public PaymentPayload Payload { get; set; }
    }
}
