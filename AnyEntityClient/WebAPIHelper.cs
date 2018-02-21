using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AnyEntityClient
{
    public class WebApiHelper
    {
        private readonly HttpClient client = new HttpClient();

        public WebApiHelper(string username, string password)
        {
            client.BaseAddress = new Uri("http://localhost:7265/");

            var token = GetTokenAsync(username, password).Result;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> GetTokenAsync(string username, string password)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "/oauth/token");
            var formData = new List<KeyValuePair<string, string>>();
            formData.Add(new KeyValuePair<string, string>("grant_type", "password"));
            formData.Add(new KeyValuePair<string, string>("username", username));
            formData.Add(new KeyValuePair<string, string>("password", password));
            formData.Add(new KeyValuePair<string, string>("scope", "all"));

            request.Content = new FormUrlEncodedContent(formData);
            var response = await client.SendAsync(request);

            var bearerData = await response.Content.ReadAsStringAsync();
            var bearerToken = JObject.Parse(bearerData)["access_token"].ToString();

            return bearerToken;
        }

        public void ShowAnyEntity(AnyEntity entity)
        {
            Console.WriteLine($"Id: {entity.Id}\tDescription: {entity.Description}");
        }

        public async Task<AnyEntity[]> GetAllEntitiesAsync()
        {
            AnyEntity[] entities = null;
            HttpResponseMessage response = await client.GetAsync("api/AnyEntities");
            if (response.IsSuccessStatusCode)
            {
                entities = await response.Content.ReadAsAsync<AnyEntity[]>();
            }
            return entities;
        }

        public async Task<Uri> CreateAnyEntityAsync(AnyEntity entity)
        {
            HttpResponseMessage response = await client.PostAsJsonAsync("api/AnyEntities", entity);
            response.EnsureSuccessStatusCode();

            return response.Headers.Location;
        }

        public async Task<AnyEntity> GetAnyEntityAsync(string path)
        {
            AnyEntity entity = null;
            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                entity = await response.Content.ReadAsAsync<AnyEntity>();
            }
            return entity;
        }

        public async Task<AnyEntity> UpdateAnyEntityAsync(AnyEntity entity)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync($"api/AnyEntities/{entity.Id}", entity);
            response.EnsureSuccessStatusCode();
            entity = await response.Content.ReadAsAsync<AnyEntity>();
            return entity;
        }

        public async Task<HttpStatusCode> DeleteAnyEntityAsync(string id)
        {
            HttpResponseMessage response = await client.DeleteAsync($"api/AnyEntities/{id}");
            return response.StatusCode;
        }
    }
}
