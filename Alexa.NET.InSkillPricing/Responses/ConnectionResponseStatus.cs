using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Responses
{
    public class ConnectionResponseStatus
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}