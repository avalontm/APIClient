using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace PluginAPI
{
    public class WebClientManager
    {
        readonly TimeSpan _timeout;
        HttpClient _httpClient;
        HttpClientHandler _httpClientHandler;
        CookieContainer _cookieContainer;
        readonly string _baseUrl;
        const string ClientUserAgent = "WebClientManager-v1";
        const string MediaTypeJson = "application/json";

        public WebClientManager(string baseUrl, TimeSpan? timeout = null)
        {
            _baseUrl = NormalizeBaseUrl(baseUrl);
            _timeout = timeout ?? TimeSpan.FromSeconds(90);

            EnsureHttpClientCreated();
        }

        public async Task<string> PostAsync(string url, HttpContent input)
        {
            EnsureHttpClientCreated();

            using (var response = await _httpClient.PostAsync(url, input))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<TResult> PostAsync<TResult>(string url, object input) where TResult : class, new()
        {
            var strResponse = await PostAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, MediaTypeJson));

            return JsonConvert.DeserializeObject<TResult>(strResponse, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Error = new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(onError)
            });
        }

        public async Task<TResult> GetAsync<TResult>(string url) where TResult : class, new()
        {
            var strResponse = await GetAsync(url);

            return JsonConvert.DeserializeObject<TResult>(strResponse, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Error = new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(onError)
            });
        }

        public async Task<TResult> PathAsync<TResult>(string url, object input) where TResult : class, new()
        {
            var strResponse = await PathAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, MediaTypeJson));

            return JsonConvert.DeserializeObject<TResult>(strResponse, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Error = new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(onError)
            });
        }

        public async Task<string> GetAsync(string url)
        {
            EnsureHttpClientCreated();

            using (var response = await _httpClient.GetAsync(url))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> PathAsync(string url, HttpContent content)
        {
            EnsureHttpClientCreated();

            using (var response = await _httpClient.PatchAsync(url, content))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<string> PutAsync(string url, HttpContent content)
        {
            EnsureHttpClientCreated();

            using (var response = await _httpClient.PutAsync(url, content))
            {
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
        }

        public async Task<TResult> PutAsync<TResult>(string url, object input) where TResult : class, new()
        {
            string strResponse = await PutAsync(url, new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, MediaTypeJson));

            return JsonConvert.DeserializeObject<TResult>(strResponse, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Error = new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(onError)
            });
        }

        public async Task<TResult> DeleteAsync<TResult>(string url) where TResult : class, new()
        {
            EnsureHttpClientCreated();

            using (var response = await _httpClient.DeleteAsync(url))
            {
                response.EnsureSuccessStatusCode();
                string strResponse = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResult>(strResponse, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Error = new EventHandler<Newtonsoft.Json.Serialization.ErrorEventArgs>(onError)
                });
            }
        }

        public void Dispose()
        {
            _httpClientHandler?.Dispose();
            _httpClient?.Dispose();
        }

        void CreateHttpClient()
        {
            _cookieContainer = new CookieContainer();

            _httpClientHandler = new HttpClientHandler
            {
                AllowAutoRedirect = true,
                AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
                UseCookies = true,
                CookieContainer = _cookieContainer,

            };

            _httpClientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            _httpClient = new HttpClient(_httpClientHandler, false)
            {
                Timeout = _timeout
            };

            _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(ClientUserAgent);

            if (!string.IsNullOrWhiteSpace(_baseUrl))
            {
                _httpClient.BaseAddress = new Uri(_baseUrl);
            }

        }

        public void CreateToken(string accessToken)
        {
            EnsureHttpClientCreated();

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }

        void EnsureHttpClientCreated()
        {
            if (_httpClient == null)
            {
                CreateHttpClient();
            }
        }

        static string ConvertToJsonString(object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            return JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
        }

        static string NormalizeBaseUrl(string url)
        {
            return url.EndsWith("/") ? url : url + "/";
        }

        void onError(object sender, Newtonsoft.Json.Serialization.ErrorEventArgs e)
        {
            e.ErrorContext.Handled = true;
        }

    }
}