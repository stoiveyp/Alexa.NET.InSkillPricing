using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Xunit;

namespace Alexa.NET.InSkillPricing.Tests
{
    public class InSkillProductClientTests
    {
        private const string ProductId = "amzn1.adg.product.unique-id-1";
        private const string ReferenceName = "your_product_name_1";
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
                Type = ProductType.Entitlement,
                ProductId = ProductId,
                Summary = ProductSummary,
                Entitled = Entitlement.NotEntitled,
                Purchasable = PurchaseState.Purchasable,
                PurchaseMode = PurchaseMode.Test,
                ReferenceName = ReferenceName,
                ActiveEntitlementCount = 1
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
                 Assert.Equal("api.eu.amazonalexa.com",message.RequestUri.Host);
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

            Assert.NotNull(response);
            Assert.Single(response.Products);
            Assert.True(response.IsTruncated);
            Assert.Equal("abcdef", response.NextToken);
        }

        [Fact]
        public async Task InSkillProductClientGetProductReturnsResponse()
        {
            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
            {
                Assert.Equal("/v1/users/~current/skills/~current/inSkillProducts/aaa", message.RequestUri.PathAndQuery);

            }, Utility.ExampleFileContent<InSkillProduct>("InSkillProduct.json"))));

            var response = await client.GetProduct("aaa");

            Assert.NotNull(response);
            AssertDefaultProduct(response);
        }

        [Fact]
        public async Task InSkillProductClientGetProductsWithTokenReturnsResponse()
        {
            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
            {
                Assert.Equal("/v1/users/~current/skills/~current/inSkillProducts?nextToken=abcdef", message.RequestUri.PathAndQuery);

            }, Utility.ExampleFileContent<InSkillProductsResponse>("InSkillProductsResponse.json"))));

            var response = await client.GetProducts("abcdef");

            Assert.NotNull(response);
        }

        [Fact]
        public async Task InSkillProductQueryOptionsGeneratedCorrectly()
        {
            var filters = new ProductFilters
            {
                Purchasable = PurchaseState.NotPurchasable,
                Entitled = Entitlement.NotEntitled,
                ProductType = ProductType.Entitlement,
                MaxResults = 99
            };

            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request,new HttpClient(new ActionHandler(message =>
            {
                Assert.Equal(
                    "/v1/users/~current/skills/~current/inSkillProducts?purchasable=NOT_PURCHASABLE&entitled=NOT_ENTITLED&productType=ENTITLEMENT&maxResults=99", 
                    message.RequestUri.PathAndQuery);
            },Utility.ExampleFileContent<InSkillProductsResponse>("InSkillProductsResponse.json"))));
            var results = await client.GetProducts(filters);
            Assert.Single(results.Products);
        }

        [Fact]
        public async Task InSkillProductQueryOptionsGeneratesSingleFilterCorrectly()
        {
            var filters = new ProductFilters
            {
                MaxResults = 99
            };

            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
            {
                Assert.Equal("/v1/users/~current/skills/~current/inSkillProducts?maxResults=99", message.RequestUri.PathAndQuery);
            }, Utility.ExampleFileContent<InSkillProductsResponse>("InSkillProductsResponse.json"))));
            var results = await client.GetProducts(filters);
            Assert.Single(results.Products);
        }

        [Fact]
        public async Task InSkillProductQueryOptionDoesntAllow101MaxResults()
        {
            var filters = new ProductFilters
            {
                MaxResults = 101
            };

            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
            {

            }, Utility.ExampleFileContent<InSkillProductsResponse>("InSkillProductsResponse.json"))));
            await Assert.ThrowsAsync<InvalidOperationException>(() => client.GetProducts(filters));
        }

        [Fact]
        public async Task VoicePurchasing()
        {
            //https://developer.amazon.com/en-US/docs/alexa/in-skill-purchase/isp-kid-skills.html#voicePurchasing-api
            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
            {
                Assert.Equal(message.RequestUri, new Uri($"{request.Context.System.ApiEndpoint}/v1/users/~current/skills/~current/settings/voicePurchasing.enabled"));
                Assert.Equal(HttpMethod.Get, message.Method);
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                    {Content = new StringContent(true.ToString())});
            })));
            var result = await client.VoicePurchasingEnabled();
            Assert.True(result);
        }

        [Fact]
        public async Task TransactionsWithProduct()
        {
            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
                {
                    Assert.Equal($"{InSkillProductsClient.TransactionsAPI}?productId=productid", message.RequestUri.PathAndQuery);
                },
                Utility.ExampleFileContent<TransactionResponse>("TransactionResponse.json"))));
            var response = await client.Transactions("productid");
            Utility.CompareJson(response, "TransactionResponse.json");
        }

        [Fact]
        public async Task TransactionsWithProductAndNextToken()
        {
            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
                {
                    Assert.Equal($"{InSkillProductsClient.TransactionsAPI}?productId=productid&nextToken=ABCDEFGH", message.RequestUri.PathAndQuery);
                },
                Utility.ExampleFileContent<TransactionResponse>("TransactionResponse.json"))));
            var response = await client.Transactions("productid", "ABCDEFGH");
            Utility.CompareJson(response, "TransactionResponse.json");
        }

        [Fact]
        public async Task TransactionsWithRequest()
        {
            var request = DummyLaunchRequest();
            var client = new InSkillProductsClient(request, new HttpClient(new ActionHandler(message =>
                {
                    Assert.Equal($"{InSkillProductsClient.TransactionsAPI}?productId=ABC&nextToken=DEF&status=EXPIRED_NO_ACTION_BY_PARENT&maxResults=50&fromLastModifiedTime={WebUtility.UrlEncode(DateTime.UnixEpoch.ToString("O"))}&toLastModifiedTime={WebUtility.UrlEncode(DateTime.UnixEpoch.AddDays(1).ToString("O"))}", message.RequestUri.PathAndQuery);
                },
                Utility.ExampleFileContent<TransactionResponse>("TransactionResponse.json"))));
            var response = await client.Transactions(new TransactionRequest
            {
                ProductId = "ABC",
                Status = TransactionStatus.ExpiredNoActionByParent,
                MaxResults = 50,
                FromModifiedDateTime = DateTime.UnixEpoch,
                ToModifiedDateTime = DateTime.UnixEpoch.AddDays(1),
                NextToken = "DEF"
            });
            Utility.CompareJson(response, "TransactionResponse.json");
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
                        ApiEndpoint = "https://api.eu.amazonalexa.com"
                    }
                }
            };
        }

        private void AssertDefaultProduct(InSkillProduct result)
        {
            Assert.Equal(ProductId, result.ProductId);
            Assert.Equal(ProductType.Entitlement, result.Type);
            Assert.Equal(ProductName, result.Name);
            Assert.Equal(ProductSummary, result.Summary);
            Assert.Equal(Entitlement.NotEntitled, result.Entitled);
            Assert.Equal(PurchaseState.Purchasable, result.Purchasable);
            Assert.Equal(ReferenceName, result.ReferenceName);
            Assert.Equal(1, result.ActiveEntitlementCount);
            Assert.Equal(PurchaseMode.Test,result.PurchaseMode);
        }
    }
}
