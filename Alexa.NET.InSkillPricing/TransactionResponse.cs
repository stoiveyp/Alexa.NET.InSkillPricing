using Alexa.NET.InSkillPricing;
using Newtonsoft.Json;

namespace Alexa.NET
{
    public class TransactionResponse
    {
        [JsonProperty("results",NullValueHandling = NullValueHandling.Ignore)]
        public Transaction[] Results { get; set; }

        [JsonProperty("metadata",NullValueHandling = NullValueHandling.Ignore)]
        public TransationResponseMetadata Metadata { get; set; }
    }
}