namespace Alexa.NET.InSkillPricing.Directives
{
    public class CancelDirective : PaymentDirective
    {
        public CancelDirective(string productId, string token) :
            base(PaymentType.Cancel, token, new PaymentPayload(productId))
        {

        }
    }
}