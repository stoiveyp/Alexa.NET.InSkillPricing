using Alexa.NET.InSkillPricing.Directives;
using Alexa.NET.Response.Directive;
using Newtonsoft.Json.Linq;

namespace Alexa.NET.InSkillPricing
{
    public class BuyConnectionRequestHandler:IConnectionSendRequestHandler
    {
        public bool CanCreate(JObject data)
        {
            return data.Value<string>("name") == "Buy";
        }

        public ConnectionSendRequest Create()
        {
            return new BuyDirective();
        }
    }
}