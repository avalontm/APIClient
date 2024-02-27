using Newtonsoft.Json;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace PluginAPI
{
    public static class ApiManager
    {
        static string? host { set; get; }
        static WebClientManager? client { set; get; }

        public static void Create(string url)
        {
            host = url;
            client = new WebClientManager(host);
        }

        public static void CreateKey(string key,  string value)
        {
            client.CreateKey(key, value);
        }

        public static void CreateToken(string accessToken)
        {
            client.CreateToken(accessToken);
        }

        public static async Task<string> Get(string path, params string[] args)
        {
            try
            {
                string _path = string.Empty;

                foreach (string arg in args)
                {
                    _path += "/" + arg;
                }

                return await client.GetAsync($"{path}{_path}");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }

        public static async Task<string> Post(object input, string path, params string[] args)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

                string _path = string.Empty;

                foreach (string arg in args)
                {
                    _path += "/" + arg;
                }

                return await client.PostAsync($"{path}{_path}", content);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
