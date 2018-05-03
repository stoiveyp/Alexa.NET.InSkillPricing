using System.Text;
using Alexa.NET.Response;
using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Directives
{
    public class BuyDirective:IDirective
    {
        public BuyDirective(string productId, string correlationToken)
        {
            Payload = new BuyDirectivePayload(productId);
            Token = correlationToken;
        }

        [JsonProperty("type")]
        public string Type => "Connections.SendRequest";

        [JsonProperty("name")] public string Name => "Buy";

        [JsonProperty("token")] public string Token { get; set; }

        [JsonProperty("payload")] public BuyDirectivePayload Payload { get; set; }
    }
}
