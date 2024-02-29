using Newtonsoft.Json.Linq;
using PluginAPI;

namespace APIConsoleDEMO
{
    internal class Program
    {
        static bool running = true;
        static string? apikey = string.Empty;

        static void Main(string[] args)
        {
            ApiManager.Create("https://curso-dev-bmem.1.us-1.fl0.io");

            onMenu();

            while (running)
            {
               
            }
        }

        static async Task onMenu()
        {
            //titulo
            string titulo = "Mini Curso";

            // Opciones del menú
            string[] opciones = { "login", "buscar curso", "ver cursos", "completar curso", "Salir" };

            // Posición actual del cursor
            int cursorPos = 0;
            // Posición actual de la barra
            int barraPos = 0;

            // Mostrar el menú con marco
            const char EsquinaSuperiorIzquierda = '╔';
            const char EsquinaSuperiorDerecha = '╗';
            const char EsquinaInferiorIzquierda = '╚';
            const char EsquinaInferiorDerecha = '╝';
            const char LineaHorizontal = '═';
            const char LineaVertical = '║';

            int anchoConsola = Console.WindowWidth;
            int anchoMarco = anchoConsola - 10; 

            while (running)
            {
                // Limpiar la pantalla
                Console.Clear();

                // Calcular la posición central del título
                int anchoTitulo = titulo.Length;
                int centroTitulo = (anchoConsola - anchoTitulo) / 2;

                // Imprimir el título antes del marco
                Console.SetCursorPosition(centroTitulo, Console.CursorTop);

                Console.WriteLine(titulo);

                // Imprimir el marco con separación
                Console.Write("    ");
                Console.Write(EsquinaSuperiorIzquierda);
                Console.Write(new string(LineaHorizontal, anchoMarco));
                Console.WriteLine(EsquinaSuperiorDerecha);

                // Mostrar el menú
                for (int i = 0; i < opciones.Length; i++)
                {
                    Console.Write("    ");

                    Console.Write(LineaVertical);
                    Console.BackgroundColor = (i == cursorPos) ? ConsoleColor.White : ConsoleColor.Black;
                    Console.ForegroundColor = (i == cursorPos) ? ConsoleColor.Black : ConsoleColor.White;
                    Console.Write("  {0,-" + (anchoMarco - 6) + "}    ", i + 1 + ". " + opciones[i]);
                    Console.ResetColor();
                    Console.Write(LineaVertical);
                    Console.WriteLine();

                    // Mostrar la barra solo en la opción actual
                    if (i == barraPos)
                    {
                        Console.SetCursorPosition(1, Console.CursorTop - 1);
                        Console.SetCursorPosition(Console.CursorLeft + (anchoMarco - 2), Console.CursorTop);
                        Console.WriteLine();
                        Console.ResetColor();
                    }
                }

                Console.Write("    ");
                Console.Write(EsquinaInferiorIzquierda);
                Console.Write(new string(LineaHorizontal, anchoMarco));
                Console.WriteLine(EsquinaInferiorDerecha);

                // Leer la tecla pulsada
                ConsoleKeyInfo keyInfo = Console.ReadKey(true);

                // Navegar por el menú
                switch (keyInfo.Key)
                {
                    case ConsoleKey.UpArrow:
                        cursorPos = (cursorPos - 1 + opciones.Length) % opciones.Length;
                        barraPos = cursorPos;
                        break;
                    case ConsoleKey.DownArrow:
                        cursorPos = (cursorPos + 1) % opciones.Length;
                        barraPos = cursorPos;
                        break;
                    case ConsoleKey.Enter:
                        // Seleccionar la opción actual
                        int opcionSeleccionada = cursorPos + 1;

                        // Agregar lógica específica para cada opción
                        switch (opcionSeleccionada)
                        {
                            case 1:
                                await onLogin();
                                break;
                            case 2:
                                await onFindCurso();
                                break;
                            case 3:
                                await onSeeCurso();
                                break;
                            case 4:
                                await onCompletCurso();
                                break;
                            case 5:
                                running = false;
                                break;
                        }

                        break;
                }
            }
        }


        static async Task onLogin()
        {
            // Crear un diccionario vacío
            var cuenta = new Dictionary<string, string>();

            Console.WriteLine("Escribe tus credenciales.");
            Console.Write($"Email: ");
            cuenta.Add("email", Console.ReadLine());

            Console.Write($"Contraseña: ");
            cuenta.Add("contrasena", ConsoleEx.ReadPassword());

            await ConsoleEx.WaitStart("iniciando sesion");
            string result = await ApiManager.Post(cuenta, "/api/login");
            await ConsoleEx.WaitEnd();

            if (!result.GetValue<bool>("status", true))
            {
                Console.WriteLine($"Error: {result.GetValue<string>("message")}");
                Continue();
                return;
            }

            apikey = result.GetValue<string>("api_key");

            Console.WriteLine($"apikey: {apikey}");

            if (!string.IsNullOrEmpty(apikey))
            {
                //Colocamos nuestra apikey que obtuvimos.
                ApiManager.CreateKey("api-key", apikey);
            }

            //esperamos que se precione cualquier tecla.
            Continue();
        }

        static async Task onFindCurso()
        {

            Console.WriteLine("Escribe el id del curso.");
            Console.Write($"Id: ");
            string curso_id = Console.ReadLine();


            await ConsoleEx.WaitStart("obtiniendo informacion del curso");
            string result = await ApiManager.Get($"/api/cursos/{curso_id}");
            await ConsoleEx.WaitEnd();

            List<JObject> cursos = result.GetValue<List<JObject>>("cursos");

            ConsoleEx.CreateTable(cursos);

            //esperamos que se precione cualquier tecla.
            Continue();
        }

        private static async Task onSeeCurso()
        {
            await ConsoleEx.WaitStart("obteniendo mis cursos");
            string result = await ApiManager.Get("/api/cursos");
            await ConsoleEx.WaitEnd();

            List<JObject> cursos = result.GetValue<List<JObject>>("cursos");

            ConsoleEx.CreateTable(cursos);

            //esperamos que se precione cualquier tecla.
            Continue();
        }


        static async Task onCompletCurso()
        {
            var curso = new Dictionary<string, string>();

            Console.WriteLine("Escribe el id del curso.");
            Console.Write($"Id: ");
            curso.Add("curso_id", Console.ReadLine());

            Console.WriteLine("Escribe el token del curso.");
            Console.Write($"Token: ");
            curso.Add("token", Console.ReadLine());

            await ConsoleEx.WaitStart("completando curso");
            string result = await ApiManager.Post(curso, "/api/cursos/completar");
            await ConsoleEx.WaitEnd();

            if (!result.GetValue<bool>("status", true))
            {
                Console.WriteLine($"Error: {result.GetValue<string>("message")}");
                Continue();
                return;
            }

            Console.WriteLine($"{result.GetValue<string>("message")}");

            //esperamos que se precione cualquier tecla.
            Continue();
        }

        static void Continue()
        {
            //esperamos que se precione cualquier tecla.
            Console.WriteLine("Preciona una tecla para continuar...");
            Console.ReadKey();
        }
    }
}