using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing
{
    public class InSkillProduct
    {
        [JsonProperty("productId")]
        public string ProductId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("purchasable")]
        public string Purchasable { get; set; }

        [JsonProperty("entitled")]
        public string Entitled { get; set; }

        [JsonProperty("entitledReason")]
        public string EntitledReason { get; set; }

        [JsonProperty("referenceName")]
        public string ReferenceName { get; set; }

        [JsonProperty("activeEntitlementCount")]
        public int ActiveEntitlementCount { get; set; }

        [JsonProperty("purchaseMode")]
        public string PurchaseMode { get; set; }
    }
}
