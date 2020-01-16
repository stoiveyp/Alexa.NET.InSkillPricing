using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.InSkillPricing.Directives;
using Alexa.NET.Response.Directive;
using Newtonsoft.Json.Linq;

namespace Alexa.NET.InSkillPricing
{
    public class CancelConnectionRequestHandler : IConnectionSendRequestHandler
    {
        public bool CanCreate(JObject data)
        {
            return data.Value<string>("name") == "Cancel";
        }

        public ConnectionSendRequest Create()
        {
            return new CancelDirective();
        }
    }
}
