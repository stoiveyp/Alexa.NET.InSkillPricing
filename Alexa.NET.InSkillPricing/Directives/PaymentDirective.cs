using System.Text;
using Alexa.NET.Response;
using Alexa.NET.Response.Converters;
using Newtonsoft.Json;

namespace Alexa.NET.InSkillPricing.Directives
{
    public class PaymentDirective:IDirective
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
            }
        }

        public PaymentDirective() { }

        public PaymentDirective(string paymentType, string correlationToken, PaymentPayload payload)
        {
            Name = paymentType;
            Payload = payload;//new PaymentPayload(productId);
            Token = correlationToken;
        }

        [JsonProperty("type")]
        public string Type => DirectiveType;

        [JsonProperty("name")] public string Name { get; set; }

        [JsonProperty("token")] public string Token { get; set; }

        [JsonProperty("payload")] public PaymentPayload Payload { get; set; }
    }
}
