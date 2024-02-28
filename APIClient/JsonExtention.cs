using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PluginAPI
{
    public static class JsonExtention
    {
        /// <summary>
        /// Convertir formato JSON a un objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T? ToConvert<T>(this object obj)
        {
            if(obj == null)
            {
                return default(T);
            }
            return JsonConvert.DeserializeObject<T>(obj.ToString());
        }

        
        /// <summary>
        /// Convertir un objecto a formato JSON
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string FromConvert(this object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            return JsonConvert.SerializeObject(obj);
        }


        /// <summary>
        /// Obtener un valor en una KEY en formato JSON
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? GetValue<T>(this object obj, string name, object value = null)
        {
            try
            {
                string json = obj?.ToString();

                if (string.IsNullOrEmpty(json))
                {
                    // Handle null or empty JSON string
                    Debug.WriteLine("JSON string is null or empty.");
                    return default(T);
                }

                JObject o = JsonConvert.DeserializeObject<JObject>(json);

                if (o == null)
                {
                    // Handle case where JSON couldn't be deserialized
                    Debug.WriteLine("Failed to deserialize JSON.");
                    return default(T);
                }

                if (o.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out var token))
                {
                    return token.ToObject<T>();
                }
                else
                {
                    Debug.WriteLine($"Key '{name}' not found in JSON.");

                    if(value !=  null)
                    {
                        return (T)Convert.ChangeType(value, value.GetType());
                    }
                    return default(T);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return default(T);
            }
        }

    }
}
