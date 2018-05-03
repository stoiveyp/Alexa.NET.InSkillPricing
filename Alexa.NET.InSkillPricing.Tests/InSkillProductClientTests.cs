using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Xunit;

namespace Alexa.NET.InSkillPricing.Tests
{
    public class InSkillProductClientTests
    {
        private const string ProductId = "amzn1.adg.product.some-id";
        private const string ProductName = "Friendly Name";
        private const string ProductSummary = "Description of the product.";
        private const string NextToken = "abcdef";

        [Fact]
        public void InSkillProductDeserializesCorrectly()
        {
            var result = Utility.ExampleFileContent<InSkillProduct>("InSkillProduct.json");

            AssertDefaultProduct(result);
        }

        [Fact]
        public void InSkillSerializesCorrectly()
        {
            var expected = new InSkillProduct
            {
                Name = ProductName,
                Type = ProductType.Subscription,
                ProductId = ProductId,
                Summary = ProductSummary,
                Entitled = Entitlement.NotEntitled,
                Purchasable = PurchaseState.Purchasable
            };

            Assert.True(Utility.CompareJson(expected, "InSkillProduct.json"));
        }

        [Fact]
        public void InSkillProductsResponseDeserializesCorrectly()
        {
            var result = Utility.ExampleFileContent<InSkillProductsResponse>("InSkillProductsResponse.json");

            Assert.True(result.IsTruncated);
            Assert.Equal(NextToken, result.NextToken);
            Assert.Single(result.Products);
            AssertDefaultProduct(result.Products.First());
        }

        [Fact]
        public void InSkillProductsResponseSerializesCorrectly()
        {
            var expected = new InSkillProductsResponse
            {
                IsTruncated = true,
                NextToken = "abcdef",
                Products = new[]
                {
                    new InSkillProduct
                    {
                        Name = ProductName,
                        Type = ProductType.Subscription,
                        ProductId = ProductId,
                        Summary = ProductSummary,
                        Entitled = Entitlement.NotEntitled,
                        Purchasable = PurchaseState.Purchasable
                    }
                }
            };
            Utility.CompareJson(expected, "InSkillProductsResponse.json");
        }

        [Fact]
        public async Task InSkillProductClientGeneratesCorrectHeaders()
        {
            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
             {
                 Assert.Single(message.Headers.AcceptLanguage);
                 Assert.Equal("en-GB",message.Headers.AcceptLanguage.First().Value);
                 Assert.Equal("Bearer", message.Headers.Authorization.Scheme);
                 Assert.Equal("abcdef", message.Headers.Authorization.Parameter);
                 Assert.Equal("api.alexa.com",message.RequestUri.Host);
                 Assert.Equal("https",message.RequestUri.Scheme);
             },Utility.ExampleFileContent<InSkillProductsResponse>("InSkillProductsResponse.json"))));

            var products = await client.GetProducts();
        }

        [Fact]
        public async Task InSkillProductClientGetProductsReturnsResponse()
        {
            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
            {
                Assert.Equal("/v1/users/~current/skills/~current/inSkillProducts", message.RequestUri.PathAndQuery);

            }, Utility.ExampleFileContent<InSkillProductsResponse>("InSkillProductsResponse.json"))));

            var response = await client.GetProducts();

            Assert.Single(response.Products);
            Assert.True(response.IsTruncated);
            Assert.Equal("abcdef", response.NextToken);
        }

        private SkillRequest DummyLaunchRequest()
        {
            return new SkillRequest
            {
                Request = new Request.Type.LaunchRequest { Locale = "en-GB" },
                Context = new Context
                {
                    System = new AlexaSystem
                    {
                        ApiAccessToken = "abcdef",
                        ApiEndpoint = "https://api.alexa.com"
                    }
                }
            };
        }

        private void AssertDefaultProduct(InSkillProduct result)
        {
            Assert.Equal(ProductId, result.ProductId);
            Assert.Equal(ProductType.Subscription, result.Type);
            Assert.Equal(ProductName, result.Name);
            Assert.Equal(ProductSummary, result.Summary);
            Assert.Equal(Entitlement.NotEntitled, result.Entitled);
            Assert.Equal(PurchaseState.Purchasable, result.Purchasable);
        }
    }
}
