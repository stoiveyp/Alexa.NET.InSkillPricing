using System.Linq;
using System.Text;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using Alexa.NET.Response.Converters;
using Alexa.NET.Response.Directive;
using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Directives
{
    public class PaymentDirective: ConnectionSendRequest<PaymentPayload>,IEndSessionDirective
    {
        private const string DirectiveType = "Connections.SendRequest";
        private static readonly object directiveadd = new object();

        public static void AddSupport()
        {
            lock (directiveadd)
            {
                if (!DirectiveConverter.TypeFactories.ContainsKey(DirectiveType))
                {
                    DirectiveConverter.TypeFactories.Add(DirectiveType, () => new PaymentDirective());
                }

                AddRequests();
            }
        }

        private static void AddRequests()
        {
            if (ConnectionSendRequestFactory.Handlers.All(c => c.GetType() != typeof(BuyConnectionRequestHandler)))
            {
                ConnectionSendRequestFactory.Handlers.Add(new BuyConnectionRequestHandler());
            }

            if (ConnectionSendRequestFactory.Handlers.All(c => c.GetType() != typeof(UpsellConnectionRequestHandler)))
            {
                ConnectionSendRequestFactory.Handlers.Add(new UpsellConnectionRequestHandler());
            }


            if (ConnectionSendRequestFactory.Handlers.All(c => c.GetType() != typeof(CancelConnectionRequestHandler)))
            {
                ConnectionSendRequestFactory.Handlers.Add(new CancelConnectionRequestHandler());
            }
        }

        public PaymentDirective() { }

        public PaymentDirective(string paymentType, string correlationToken, PaymentPayload payload)
        {
            Name = paymentType;
            Payload = payload;//new PaymentPayload(productId);
            Token = correlationToken;
        }

        [JsonProperty("token")]
        public string Token { get; set; }

        public bool? ShouldEndSession => true;
    }
}
