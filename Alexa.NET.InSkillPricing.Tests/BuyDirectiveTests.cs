using System;
using System.Collections.Generic;
using System.Text;
using Alexa.NET.InSkillPricing.Directives;
using Alexa.NET.Response;
using Xunit;

namespace Alexa.NET.InSkillPricing.Tests
{
    public class BuyDirectiveTests
    {
        [Fact]
        public void BuyDirectiveSerializesCorrectly()
        {
            var directive = new BuyDirective("amzn1.adg.product", "correlationToken");

            Assert.True(Utility.CompareJson(directive,"BuyDirective.json"));
        }

        [Fact]
        public void BuyDirectiveDeserializesCorrectly()
        {
            var directive = Utility.ExampleFileContent<BuyDirective>("BuyDirective.json");
            Assert.Equal("Buy",directive.Name);
            Assert.Equal("Connections.SendRequest",directive.Type);
            Assert.Equal("correlationToken",directive.Token);
            Assert.Equal("amzn1.adg.product",directive.Payload.InSkillProduct.ProductId);
        }

    }
}
