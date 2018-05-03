using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Responses
{
    public class ConnectionResponseRequest:Alexa.NET.Request.Type.Request
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("status")]
        public ConnectionResponseStatus Status { get; set; }

        [JsonProperty("payload")]
        public ConnectionResponsePayload Payload { get; set; }
    }
}
