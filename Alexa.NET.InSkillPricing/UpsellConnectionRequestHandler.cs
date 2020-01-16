using Alexa.NET.InSkillPricing.Directives;
using Alexa.NET.Response.Directive;
using Newtonsoft.Json.Linq;

namespace Alexa.NET.InSkillPricing
{
    public class UpsellConnectionRequestHandler : IConnectionSendRequestHandler
    {
        public bool CanCreate(JObject data)
        {
            return data.Value<string>("name") == "Upsell";
        }

        public ConnectionSendRequest Create()
        {
            return new UpsellDirective();
        }
    }
}