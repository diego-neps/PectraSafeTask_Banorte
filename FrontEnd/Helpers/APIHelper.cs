using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;

using System.Text;
using System.Threading.Tasks;

namespace PectraForms.WebApplication.Helpers
{
    class APIHelper
    {
        HttpClient client = new HttpClient();

        public APIHelper()
        {
            client.BaseAddress = new Uri(ConfigurationManager.AppSettings["APIUrl"] + "/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public APIHelper(string pszToken) : this()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", pszToken);
        }

        public string Get(string requestUri, string TrxId, string GUID, ref string Token)
        {
            if (this.client.DefaultRequestHeaders.Authorization != null)
                Token = client.DefaultRequestHeaders.Authorization.Parameter;

            string data = null;
            HttpResponseMessage response = AsyncHelper.RunSync<HttpResponseMessage>(() => client.GetAsync(requestUri));
            if (response.IsSuccessStatusCode)
            {
                data = AsyncHelper.RunSync<string>(() => response.Content.ReadAsStringAsync());
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                Token = AsyncHelper.RunSync<string>(() => Login(TrxId, GUID));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Token);
                return Get(requestUri, TrxId, GUID, ref Token);
            }
            return data;
        }

        private async Task<string> Login(string TrxId, string GUID)
        {
            string szToken = string.Empty;
            // Login
            var pairs = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>( "grant_type", "password"),
                    new KeyValuePair<string, string>( "trxid", TrxId ),
                    new KeyValuePair<string, string> ( "guid", GUID )
                };
            var content = new FormUrlEncodedContent(pairs);
            HttpResponseMessage response = await client.PostAsync("Token", content);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string szResp = await response.Content.ReadAsStringAsync();
                szToken = ((dynamic)((Newtonsoft.Json.Linq.JObject)(JsonConvert.DeserializeObject(szResp)))).access_token;
            }
            else
                throw new Exception(await response.Content.ReadAsStringAsync());

            return szToken;
        }

        public string PostAsync(string uri, JObject obj)
        {
            return this.PostAsync(uri, obj.ToString(Formatting.None));
        }

        public string PostAsync(string uri, string szContent)
        {
            StringContent content = new StringContent(szContent, Encoding.UTF8, "application/json");
            HttpResponseMessage response = AsyncHelper.RunSync<HttpResponseMessage>(() => client.PostAsync(uri, content));

            string data = null;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                data = AsyncHelper.RunSync<string>(() => response.Content.ReadAsStringAsync());
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                throw new Exception("Unauthorized");
            else
                throw new Exception(AsyncHelper.RunSync<string>(() => response.Content.ReadAsStringAsync()));

            return data;
        }

    }
}
