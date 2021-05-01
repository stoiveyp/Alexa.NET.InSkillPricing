using System;
using Alexa.NET.InSkillPricing;

namespace Alexa.NET
{
    public class TransactionRequest
    {
        public TransactionRequest(){}

        public TransactionRequest(string productId)
        {
            ProductId = productId;
        }

        public string ProductId { get; set; }
        public string NextToken { get; set; }
        public int? MaxResults { get; set; }
        public TransactionStatus? Status { get; set; }
        public DateTime? FromModifiedDateTime { get; set; }
        public DateTime? ToModifiedDateTime { get; set; }

    }
}