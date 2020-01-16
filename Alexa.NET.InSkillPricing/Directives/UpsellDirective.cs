namespace Alexa.NET.InSkillPricing.Directives
{
    public class UpsellDirective : PaymentDirective
    {
        public UpsellDirective() { }
        public UpsellDirective(string productId, string token, string upsellMessage):
            base(PaymentType.Upsell,token,new PaymentPayload(productId,upsellMessage))
        {

        }
    }
}