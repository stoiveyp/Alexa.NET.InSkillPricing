using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
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

        public const string ISPBasePath = "/v1/users/~current/skills/~current";

        public const string InSkillProductsAPI = "/inSkillProducts";
        public const string VoicePurchasingAPI = "/settings/voicePurchasing.enabled";
        public const string TransactionsAPI = "/inSkillProductsTransactions";

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
            var response = await Client.GetStreamAsync(ISPBasePath + InSkillProductsAPI + query).ConfigureAwait(false);
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


            var response = await Client.GetStreamAsync($"{ISPBasePath}{InSkillProductsAPI}{query}").ConfigureAwait(false);
            using (var reader = new JsonTextReader(new StreamReader(response)))
            {
                return Serializer.Deserialize<InSkillProductsResponse>(reader);
            }
        }

        public async Task<InSkillProduct> GetProduct(string productId)
        {
            var response = await Client.GetStreamAsync($"{ISPBasePath}{InSkillProductsAPI}/{productId}").ConfigureAwait(false);
            using (var reader = new JsonTextReader(new StreamReader(response)))
            {
                return Serializer.Deserialize<InSkillProduct>(reader);
            }
        }

        public async Task<bool> VoicePurchasingEnabled()
        {
            var response = await Client.GetStringAsync($"{ISPBasePath}{VoicePurchasingAPI}").ConfigureAwait(false);
            return bool.Parse(response);
        }

        public Task<TransactionResponse> Transactions(string productId)
        {
            return Transactions(new TransactionRequest(productId));
        }

        public Task<TransactionResponse> Transactions(string productId, string nextToken)
        {
            return Transactions(new TransactionRequest(productId) {NextToken = nextToken});
        }

        public async Task<TransactionResponse> Transactions(TransactionRequest request)
        {
            var osb = new StringBuilder(50);

            void NewParam(string key, string value)
            {
                if (osb.Length > 0)
                {
                    osb.Append('&');
                }
                osb.Append(key);
                osb.Append('=');
                osb.Append(WebUtility.UrlEncode(value));
            }

            if (!string.IsNullOrWhiteSpace(request.ProductId))
            {
                NewParam("productId",request.ProductId);
            }

            if (!string.IsNullOrWhiteSpace(request.NextToken))
            {
                NewParam("nextToken", request.NextToken);
            }

            if (request.Status.HasValue)
            {
                NewParam("status", ToEnumString(typeof(TransactionStatus), request.Status.Value));
            }

            if (request.MaxResults.HasValue)
            {
                NewParam("maxResults", request.MaxResults.Value.ToString());
            }

            if (request.FromModifiedDateTime.HasValue)
            {
                NewParam("fromLastModifiedTime", request.FromModifiedDateTime.Value.ToString("O"));
            }

            if (request.ToModifiedDateTime.HasValue)
            {
                NewParam("toLastModifiedTime", request.ToModifiedDateTime.Value.ToString("O"));
            }

            var response = await Client.GetStreamAsync($"{TransactionsAPI}?{osb}").ConfigureAwait(false);
            using (var reader = new JsonTextReader(new StreamReader(response)))
            {
                return Serializer.Deserialize<TransactionResponse>(reader);
            }
        }

        private static string ToEnumString(Type enumType, object type)
        {
            var name = Enum.GetName(enumType, type);
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetTypeInfo().GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).FirstOrDefault();
            return enumMemberAttribute?.Value ?? type.ToString();
        }
    }
}
