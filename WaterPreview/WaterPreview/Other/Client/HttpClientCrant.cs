using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WaterPreview.Other.Client
{
    public class HttpClientCrant
    {
        private HttpClient _httpClient;

        public HttpClientCrant()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri("http://localhost:51490/");
        }

        //[Fact]
        public async Task Get_Accesss_Token_By_Resource_Owner_Password_Credentials_Grant()
        {
            Console.WriteLine(await GetAccessToken());
        }

        public async Task Call_WebAPI_By_Resource_Owner_Password_Credentials_Grant()
        {
            var token = await GetAccessToken();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            Console.WriteLine(await (await _httpClient.GetAsync("/api/users/current")).Content.ReadAsStringAsync());
        }

        private async Task<string> GetAccessToken()
        {
            var clientId = "1234";
            var clientSecret = "5678";

            var parameters = new Dictionary<string, string>();
            parameters.Add("grant_type", "password");
            parameters.Add("username", "username");
            parameters.Add("password", "password");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(clientId + ":" + clientSecret))
                );

            var response = _httpClient.PostAsync("/token", new FormUrlEncodedContent(nameValueCollection: parameters)).Result;
            var responseValue = await response.Content.ReadAsStringAsync();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine(responseValue);
                Console.ReadKey();
                return JObject.Parse(responseValue)["access_token"].Value<string>();
            }
            else
            {
                Console.WriteLine(responseValue);
                Console.ReadKey();

                return string.Empty;
            }
        }
    }
}