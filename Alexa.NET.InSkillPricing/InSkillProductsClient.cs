using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
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

        public InSkillProductsClient(SkillRequest request):this(request,new HttpClient())
        {
        }

        public InSkillProductsClient(SkillRequest request, HttpClient client) : 
            this(request.Context.System.ApiAccessToken, request.Request.Locale, request.Context.System.ApiEndpoint, client)
        {

        }

        public InSkillProductsClient(string token, string locale, string host) : this(token,locale,host, new HttpClient())
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

        public async Task<InSkillProductsResponse> GetProducts()
        {
            var response = await Client.GetStreamAsync(InSkillProductBasePath).ConfigureAwait(false);
            using (var reader = new JsonTextReader(new StreamReader(response)))
            {
                return Serializer.Deserialize<InSkillProductsResponse>(reader);
            }
        }
    }
}
