using Alexa.NET.InSkillPricing.Responses;
using Xunit;

namespace Alexa.NET.InSkillPricing.Tests
{
    public class ConnectionResponseRequestTests
    {
        [Fact]
        public void ConnectionResponseRequestDeserializesProperly()
        {
            var request = Utility.ExampleFileContent<ConnectionResponseRequest>("ConnectionResponseRequest.json");

            Assert.Equal("Upsell", request.Name);
            Assert.Equal("token", request.Token);

            Assert.Equal("200", request.Status.Code);
            Assert.Equal("test message", request.Status.Message);

            Assert.Equal("optional additional message", request.Payload.Message);
            Assert.Equal("amzn1.productId", request.Payload.ProductId);
            Assert.Equal(PurchaseResult.Accepted,request.Payload.PurchaseResult);
        }

        [Fact]
        public void ConnectionResponseTypeDeserializesProperty()
        {

        }
    }
}
