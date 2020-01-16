using System;
using System.Collections.Generic;
using System.Text;

namespace Alexa.NET.InSkillPricing.Directives
{
    public class BuyDirective:PaymentDirective
    {
        public BuyDirective() { }

        public BuyDirective(string productId, string token) :
            base(PaymentType.Buy, token, new PaymentPayload(productId))
        {

        }
    }
}
