using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace suiren.Services
{
    public class AzureOpenaiApiService
    {
        private static readonly string endpoint = "https://hmaiservices.openai.azure.com/";
        private static readonly string apiKey = "8910966469db4008a81cd2a682ebca13";
        private static readonly string deploymentName = "gpt-4o/chat"; // 例如 "text-davinci-003"        

        public static async Task<string> GetOpenAIResponse(string prompt)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("api-key", apiKey);

                var requestBody = new
                {
                    prompt = prompt,
                    max_tokens = 100
                };

                var jsonContent = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                string requestUri = $"{endpoint}openai/deployments/{deploymentName}/completions?api-version=2024-08-01-preview";
                var response = await client.PostAsync(requestUri, content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                dynamic result = JsonConvert.DeserializeObject(responseBody);

                return result.choices[0].text.ToString();
            }
        }
    }
}
