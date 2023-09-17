using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageDoorApp.Model;
using Microsoft.Extensions.Configuration;
using RestSharp;
using RestSharp.Authenticators;

namespace GarageDoorApp.Api
{
    internal class GarageDoorApi : IGarageDoorApi
    {
        private readonly RestClient _client;

        public GarageDoorApi(IConfiguration config)
        {
            var url = config.GetValue<string>("Api:Url");
            var userName = config.GetValue<string>("Api:UserName");
            var password = config.GetValue<string>("Api:Password");

            var clientOptions = new RestClientOptions(url)
            {
                Authenticator = new HttpBasicAuthenticator(userName, password)
            };

            _client = new RestClient(clientOptions);
        } 
        public async Task<GarageDoorStatus> GetAsync(string id)
        {
            var request = new RestRequest($"garage-door/{id}");
            var status = await _client.GetAsync<GarageDoorStatus>(request);

            return status;
        }
    }
}
