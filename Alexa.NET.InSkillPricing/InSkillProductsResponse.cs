using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing
{
    public class InSkillProductsResponse
    {
        [JsonProperty("inSkillProducts")]
        public InSkillProduct[] Products { get; set; }

        [JsonProperty("isTruncated")]
        public bool IsTruncated { get; set; }

        [JsonProperty("nextToken")]
        public string NextToken { get; set; }
    }
}
