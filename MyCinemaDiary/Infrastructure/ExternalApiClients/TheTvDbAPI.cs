﻿using System.Reflection.PortableExecutable;
using System.Text;
using System.Text.Json;
using System.IO;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using MyCinemaDiary.Domain.Entities;

namespace MyCinemaDiary.Infrastructure.ExternalApiClients
{
    public class TheTvDbAPI
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        private string BearerToken { get; set; }

        public async void initialize()
        {
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));
            string filePath = Path.Combine(solutionDirectory, "MyCinemaDiary", "bearertoken.txt");
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
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            HttpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + BearerToken);
            title = title.Replace(" ", "+");
            var url = $"https://api4.thetvdb.com/v4/search?query={title}&limit={results}&type=movie";

            HttpResponseMessage response = await HttpClient.GetAsync(url);
            JsonDocument jsonObject;
            // Gets a new bearer token if the current one is invalid/out of date
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                BearerToken = await GetNewBearerTokenAsync();
                jsonObject = await Search(title, results);
            }
            else jsonObject = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            return jsonObject;
        }

        private async Task<string> GetNewBearerTokenAsync()
        {
            HttpClient.DefaultRequestHeaders.Clear();
            HttpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            var apiKey = GetSecretsJson().GetProperty("tvdb_api_key").GetString();
            var obj = $"{{ \"apikey\": \"{apiKey}\" }}";
            var content = new StringContent(obj, Encoding.UTF8, "application/json");

            var response = await HttpClient.PostAsync("https://api4.thetvdb.com/v4/login", content);
            var jsonObject = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var token = jsonObject.RootElement.GetProperty("data").GetProperty("token").GetString();
            return token;
        }


        private JsonElement GetSecretsJson()
        {
            // Get the path to the solution directory
            string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

            // Combine the solution directory with the file name
            string filePath = Path.Combine(solutionDirectory, "MyCinemaDiary", "secrets.json");
            StreamReader reader = new(filePath);
            var text = reader.ReadToEnd();

            return JsonDocument.Parse(text).RootElement;
        }
    }
}
