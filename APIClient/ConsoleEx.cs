using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace PluginAPI
{
    public static class ConsoleEx
    {
        static bool isWait { get; set; } = false;
        static ConsoleSpinner spinner;

        public static string ReadPassword()
        {
            // Leer la contraseña sin mostrarla en la consola
            var password = "";
            while (true)
            {
                var tecla = Console.ReadKey(true);
                if (tecla.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (tecla.Key == ConsoleKey.Backspace)
                {
                    if (password.Length > 0)
                    {
                        password = password.Substring(0, password.Length - 1);
                        Console.CursorLeft--;
                        Console.Write(" ");
                        Console.CursorLeft--;
                    }
                }
                else
                {
                    password += tecla.KeyChar;
                    Console.Write("*");
                }
            }

            Console.WriteLine();
            return password;
        }


        public static async Task WaitEnd()
        {
            if (spinner != null)
            {
                isWait = false;
            }

            await Task.Delay(100);
        }

        public static async Task WaitStart(string message = "Espere", int sequence = 5)
        {
            Task.Run(() =>
            {
                isWait = true;
                spinner = new ConsoleSpinner();
                spinner.Delay = 300;
                while (isWait)
                {
                    spinner.Turn(displayMsg: message, sequenceCode: sequence);
                }

                spinner.End();
            });

            await Task.Delay(1000);
        }


        public static void CreateTable(List<JObject> datos)
        {
            if (datos == null || datos.Count == 0)
            {
                Console.WriteLine("No se proporcionan datos. No se puede generar la tabla.");
                return;
            }

            Console.WriteLine();

            List<string> headers = new List<string>();
            List<List<JToken>> items = new List<List<JToken>>();

            //Obtengemos las llaves
            var keys = datos[0].Properties().Select(p => p.Name).ToList();

            foreach (JToken key in keys)
            {
                headers.Add(key.ToString());
            }

            foreach (JObject dato in datos)
            {
                List<JToken> list = new List<JToken>();
                foreach (var value in dato)
                {
                    list.Add(value.Value);
                }

                items.Add(list);
            }

            // Calcular el ancho máximo para cada columna.
            int[] columnWidths = new int[headers.Count];

            for (int i = 0; i < headers.Count; i++)
            {
                for (int j = 0; j < items[0].Count; j++)
                {
                    columnWidths[i] = Math.Max(columnWidths[i], items[0][i].ToString().Length);
                }
            }

            // Titulos con separadores y relleno
            for (int i = 0; i < headers.Count; i++)
            {
                Console.Write(headers[i].PadRight(columnWidths[i] + 2));
            }

            Console.WriteLine();

            // Línea de separación
            for (int i = 0; i < headers.Count; i++)
            {
                Console.Write(new string('-', columnWidths[i] + 2));
            }

            Console.WriteLine();

            // Filas de datos
            for (int i = 0; i < datos.Count; i++)
            {
                for (int j = 0; j < headers.Count; j++)
                {
                    JToken token = items[i][j];

                    string value = token.ToString();

                    if (token.Type == JTokenType.Boolean)
                    {
                        if (token.Value<bool>())
                        {
                            value = "SI";
                        }
                        else
                        {
                            value = "NO";
                        }
                    }

                    Console.Write(value.PadRight(columnWidths[j] + 2));
                }

                Console.WriteLine();
             }

            headers.Clear();
            items.Clear();
            Console.WriteLine();
        }
    }
}
