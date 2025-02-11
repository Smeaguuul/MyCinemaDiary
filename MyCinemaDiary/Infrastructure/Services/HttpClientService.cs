using System.Text;

namespace MyCinemaDiary.Infrastructure.Services
{
    public class HttpClientService
    {
        private static readonly HttpClient HttpClient = new HttpClient();

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(url));
            }

            try
            {
                HttpResponseMessage response = await HttpClient.GetAsync(url);
                return response;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                throw;
            }
        }
        public async Task<HttpResponseMessage> PostAsync(string url, string content, string mediaType = "application/json")
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL cannot be null or empty", nameof(url));
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentException("Content cannot be null or empty", nameof(content));
            }

            try
            {
                var httpContent = new StringContent(content, Encoding.UTF8, mediaType);
                HttpResponseMessage response = await HttpClient.PostAsync(url, httpContent);
                return response;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                throw;
            }
        }

        public void SetDefaultHeader(string name, string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Header name cannot be null or empty", nameof(name));
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("Header value cannot be null or empty", nameof(value));
            }

            HttpClient.DefaultRequestHeaders.Add(name, value);
        }

        public void ClearHeader(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Header name cannot be null or empty", nameof(name));
            }

            if (HttpClient.DefaultRequestHeaders.Contains(name))
            {
                HttpClient.DefaultRequestHeaders.Remove(name);
            }
        }

        public void ClearAllHeaders()
        {
            HttpClient.DefaultRequestHeaders.Clear();
        }
    }
}
