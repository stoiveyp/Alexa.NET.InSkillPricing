using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alexa.NET.Request.Type;

namespace Alexa.NET.InSkillPricing.Responses
{
    public class ConnectionResponseHandler:IRequestTypeConverter
    {
        public bool CanConvert(string requestType)
        {
            return requestType == "Connections.Response";
        }

        public Request.Type.Request Convert(string requestType)
        {
            return new ConnectionResponseRequest();
        }

        public void AddToRequestConverter()
        {
            if (RequestConverter.RequestConverters.Where(rc => rc != null)
                .All(rc => rc.GetType() != typeof(ConnectionResponseHandler)))
            {
                RequestConverter.RequestConverters.Add(this);
            }
        }
    }
}
