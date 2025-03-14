﻿using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MyCinemaDiary.Domain.Entities;
using MyCinemaDiary.Infrastructure.Services;

namespace MyCinemaDiary.Infrastructure.ExternalApiClients
{
    public class TheTvDbAPI
    {
        private static HttpClientService HttpClient;
        private string? BearerToken { get; set; }

        public TheTvDbAPI(HttpClientService httpClientService)
        {
            HttpClient = httpClientService ?? throw new ArgumentNullException(nameof(httpClientService));
        }
        public async void initialize()
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "bearertoken.txt");            
            //Only gets new bearer token if the file does not exist
            try 
            { 
                BearerToken = File.ReadAllText(filePath); 
            }
            catch (FileNotFoundException)
            {
                BearerToken = await GetNewBearerTokenAsync();
                File.WriteAllText(filePath, BearerToken);
            }
        }
        public async Task<JsonDocument> Search(string title, int results)
        {
            HttpClient.ClearAllHeaders();
            HttpClient.SetDefaultHeader("Accept", "application/json");
            HttpClient.SetDefaultHeader("Authorization", "Bearer " + BearerToken);
            title = title.Replace(" ", "+");
            var url = $"https://api4.thetvdb.com/v4/search?query={title}&limit={results}&type=movie";

            HttpResponseMessage response = await HttpClient.GetAsync(url);
            JsonDocument jsonObject;
            // Gets a new bearer token if the current one is invalid/out of date. And then tries again.
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                BearerToken = await GetNewBearerTokenAsync(); // TODO Make new bearertoken saved in file.
                jsonObject = await Search(title, results);
            }
            else jsonObject = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return jsonObject;
        }

        private async Task<string> GetNewBearerTokenAsync()
        {
            HttpClient.ClearAllHeaders();
            HttpClient.SetDefaultHeader("Accept", "application/json");
            var apiKey = GetSecretsJson().GetProperty("tvdb_api_key").GetString();
            var obj = $"{{ \"apikey\": \"{apiKey}\" }}";


            var response = await HttpClient.PostAsync("https://api4.thetvdb.com/v4/login", obj);
            var jsonObject = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var token = jsonObject.RootElement.GetProperty("data").GetProperty("token").GetString();
            return token;
        }


        private JsonElement GetSecretsJson()
        {
            // Get the path to the solution directory
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "secrets.json");

            // Check if the file exists
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"The secrets.json file was not found at {filePath}");
            }

            StreamReader reader = new(filePath);
            var text = reader.ReadToEnd();

            return JsonDocument.Parse(text).RootElement;
        }
    }
}
