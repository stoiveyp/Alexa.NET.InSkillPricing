using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Alexa.NET.InSkillPricing;
using Alexa.NET.Request;
using Newtonsoft.Json;

namespace Alexa.NET
{
    public class InSkillProductsClient
    {
        public HttpClient Client { get; }

        private const string InSkillProductBasePath = "/v1/users/~current/skills/~current/inSkillProducts";

        private static readonly JsonSerializer Serializer = JsonSerializer.CreateDefault();

        public InSkillProductsClient(SkillRequest request) : this(request, new HttpClient())
        {
        }

        public InSkillProductsClient(SkillRequest request, HttpClient client) :
            this(request.Context.System.ApiAccessToken, request.Request.Locale, request.Context.System.ApiEndpoint, client)
        {

        }

        public InSkillProductsClient(string token, string locale, string host) : this(token, locale, host, new HttpClient())
        {
        }

        public InSkillProductsClient(string token, string locale, string host, HttpClient client)
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(host, UriKind.Absolute);
            }

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(locale));
            Client = client;
        }

        public async Task<InSkillProductsResponse> GetProducts(string nextToken = null)
        {
            var query = string.IsNullOrWhiteSpace(nextToken) ? string.Empty : "?nextToken=" + nextToken;
            var response = await Client.GetStreamAsync(InSkillProductBasePath + query).ConfigureAwait(false);
            using (var reader = new JsonTextReader(new StreamReader(response)))
            {
                return Serializer.Deserialize<InSkillProductsResponse>(reader);
            }
        }

        public async Task<InSkillProductsResponse> GetProducts(ProductFilters filters)
        {
            var query = string.Empty;
            if (filters?.AnySet ?? false)
            {
                if (filters.MaxResults.HasValue && (filters.MaxResults < 0 || filters.MaxResults > 100))
                {
                    throw new InvalidOperationException("When set MaxResults must be between 1 and 100");
                }

                var content = new Dictionary<string, string>();

                if (!string.IsNullOrWhiteSpace(filters.Purchasable))
                {
                    content.Add("purchasable", filters.Purchasable);
                }

                if (!string.IsNullOrWhiteSpace(filters.Entitled))
                {
                    content.Add("entitled", filters.Entitled);
                }

                if (!string.IsNullOrWhiteSpace(filters.ProductType))
                {
                    content.Add("productType", filters.ProductType);
                }

                if (filters.MaxResults.HasValue)
                {
                    content.Add("maxResults", filters.MaxResults.ToString());
                }


                query = "?" + string.Join("&", content.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            }


            var response = await Client.GetStreamAsync(InSkillProductBasePath + query).ConfigureAwait(false);
            using (var reader = new JsonTextReader(new StreamReader(response)))
            {
                return Serializer.Deserialize<InSkillProductsResponse>(reader);
            }
        }

        public async Task<InSkillProduct> GetProduct(string productId)
        {
            var response = await Client.GetStreamAsync($"{InSkillProductBasePath}/{productId}").ConfigureAwait(false);
            using (var reader = new JsonTextReader(new StreamReader(response)))
            {
                return Serializer.Deserialize<InSkillProduct>(reader);
            }
        }
    }
}
