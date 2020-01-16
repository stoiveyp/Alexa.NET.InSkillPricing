using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.InSkillPricing.Directives;
using Alexa.NET.Request.Type;
using Newtonsoft.Json.Linq;

namespace Alexa.NET.InSkillPricing.Responses
{
    public class ConnectionRequestHandler
    { 
        public static void AddToRequestConverter()
        {
            if (ConnectionResponseTypeConverter.Handlers.All(c => c.GetType() != typeof(PaymentConnectionResponseHandler)))
            {
                ConnectionResponseTypeConverter.Handlers.Add(new PaymentConnectionResponseHandler());
            }
        }
    }

    internal class PaymentConnectionResponseHandler : IConnectionResponseHandler
    {
        private string[] Names = {"Buy", "Upsell", "Cancel"};
        public bool CanCreate(JObject data)
        {
            return Names.Contains(data.Value<string>("name"));
        }

        public Request.Type.ConnectionResponseRequest Create(JObject data)
        {
            return new ConnectionResponseRequest<ConnectionResponsePayload>();
        }
    }
}
