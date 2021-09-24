using Newtonsoft.Json;

namespace Alexa.NET
{
    public class TransactionResultSet
    {
        [JsonProperty("nextToken",NullValueHandling = NullValueHandling.Ignore)]
        public string NextToken { get; set; }
    }
}