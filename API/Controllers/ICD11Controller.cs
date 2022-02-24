using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http.Headers;
using Nancy.Json;
using System.Text.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Http.Cors;

namespace API.Controllers
{
    [ApiController]
    [Route("api/who-icd")]
    public class ICD11Controller : Controller
    {
        string token;

        public ICD11Controller()
        {
            var task = Task.Run(async () => { token = await Authenticate(); });
            task.Wait();
        }
        public IActionResult Index()
        {
            return View();
        }


        [HttpGet("search/{entity?}")]
        public async ValueTask<ActionResult<string>> Search(string entity)
        {
            var client = new HttpClient();
            client.SetBearerToken(token.ToString());
            HttpRequestMessage request;

            if (string.IsNullOrWhiteSpace(entity) || string.IsNullOrEmpty(entity))
            {
                request = new HttpRequestMessage(HttpMethod.Get, "https://id.who.int/icd/entity");
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
                request.Headers.Add("API-Version", "v2");
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }

                var resultJson = response.Content.ReadAsStringAsync().Result;
                var prettyJson = JValue.Parse(resultJson).ToString(Formatting.Indented); //convert json to a more human readable fashion
                                                                                         //Console.WriteLine(prettyJson);
                return prettyJson;
            } else
            {
                request = new HttpRequestMessage(HttpMethod.Get, "https://id.who.int/icd/release/11/2022-02/mms/search?q=" + entity);
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
                request.Headers.Add("API-Version", "v2");
                var response = await client.SendAsync(request);

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine(response.StatusCode);
                }

                var resultJson = response.Content.ReadAsStringAsync().Result; //Now resultJson has the resulting json string
                var prettyJson = JValue.Parse(resultJson).ToString(Formatting.Indented); //convert json to a more human readable fashion

                return prettyJson;
            }

        }


        [HttpGet("getEntity/{url}")]
        public async ValueTask<ActionResult<string>> getEntity(string entity)
        {
            var client = new HttpClient();
            client.SetBearerToken(token.ToString());
            HttpRequestMessage request;

            Console.WriteLine("Here: " + token);


            request = new HttpRequestMessage(HttpMethod.Get, "https://id.who.int/icd/entity/" + entity);

            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            request.Headers.AcceptLanguage.Add(new StringWithQualityHeaderValue("en"));
            request.Headers.Add("API-Version", "v2");
            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }

            var resultJson = response.Content.ReadAsStringAsync().Result;
            var prettyJson = JValue.Parse(resultJson).ToString(Formatting.Indented);
            return prettyJson;
        }
        public async ValueTask<string> Authenticate()
        {
            string _secureFile = @"c:\users\Akha Magaqana\documents\_secureFile.txt";
            var lines = System.IO.File.ReadLines(_secureFile).ToArray();
            if (lines.Count() != 2)
            {
                Console.WriteLine("the securefile should have two lines in it. The first line contains the client id and the second line client key");
                return "";
            }

            var clientId = lines[0];
            var clientSecret = lines[1];

            var client = new HttpClient();

            //Instead of the following two lines you could directly use the token endpoint as https://icdaccessmanagement.who.int/connect/token
            var disco = await client.GetDiscoveryDocumentAsync("https://icdaccessmanagement.who.int");
            if (disco.IsError) throw new Exception(disco.Error);

            var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = clientId,
                ClientSecret = clientSecret,
                Scope = "icdapi_access",
                GrantType = "client_credentials",
                ClientCredentialStyle = ClientCredentialStyle.AuthorizationHeader
            });

            if (tokenResponse.IsError)
            {
                Console.WriteLine("Error: " + tokenResponse.Error);
                return "";
            } 
            return tokenResponse.AccessToken;
        }
   
    }
}
