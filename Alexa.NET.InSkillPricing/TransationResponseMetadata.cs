using Newtonsoft.Json;

namespace Alexa.NET
{
    public class TransationResponseMetadata
    {
        [JsonProperty("resultSet",NullValueHandling = NullValueHandling.Ignore)]
        public TransactionResultSet ResultSet { get; set; }
    }
}