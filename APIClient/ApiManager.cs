using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace PluginAPI
{
    public static class ApiManager
    {
        static string? host { set; get; }
        static WebClientManager? client { set; get; }

        /// <summary>
        /// Establecemos la url base del dominio.
        /// (Ejemplo: https://dominio.com)
        /// </summary>
        /// <param name="url"></param>
        public static void Create(string url)
        {
            host = url;
            client = new WebClientManager(host);
        }


        /// <summary>
        /// Creamos Autorizacion en Headers
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void CreateKey(string key,  string value)
        {
            client.CreateKey(key, value);
        }

        /// <summary>
        /// Limpiamos la Autorizacion en Headers
        /// </summary>
        public static void ClearKeys()
        {
            client.ClearKeys();
        }

        /// <summary>
        /// Creamos Autorizacion Token para Bearer
        /// </summary>
        /// <param name="accessToken"></param>
        public static void CreateToken(string accessToken)
        {
            client.CreateToken(accessToken);
        }

        /// <summary>
        /// Metodo GET
        /// </summary>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Metodo POST
        /// </summary>
        /// <param name="input"></param>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
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


        /// <summary>
        /// Metodo PUT
        /// </summary>
        /// <param name="input"></param>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<string> Put(object input, string path, params string[] args)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

                string _path = string.Empty;

                foreach (string arg in args)
                {
                    _path += "/" + arg;
                }

                return await client.PutAsync($"{path}{_path}", content);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }


        /// <summary>
        /// Metodo PATH
        /// </summary>
        /// <param name="input"></param>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<string> Path(object input, string path, params string[] args)
        {
            try
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(input), Encoding.UTF8, "application/json");

                string _path = string.Empty;

                foreach (string arg in args)
                {
                    _path += "/" + arg;
                }

                return await client.PathAsync($"{path}{_path}", content);
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }


        /// <summary>
        /// Metodo DELETE
        /// </summary>
        /// <param name="path"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static async Task<string> Delete(string path, params string[] args)
        {
            try
            {
                string _path = string.Empty;

                foreach (string arg in args)
                {
                    _path += "/" + arg;
                }

                return await client.DeleteAsync($"{path}{_path}");
            }
            catch (HttpRequestException ex)
            {
                Debug.WriteLine(ex);
                return null;
            }
        }
    }
}
