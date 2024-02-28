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
            while (running)
            {
                Console.Clear();
                Console.WriteLine("===========================================================");
                Console.WriteLine("===================     MENU      =========================");
                Console.WriteLine("===========================================================");
                Console.WriteLine("");
                Console.WriteLine("[F1]: login");
                Console.WriteLine("[F2]: buscar curso");
                Console.WriteLine("[F3]: ver cursos");
                Console.WriteLine("[F4]: completar curso");
                Console.Write("Selecciona una opcion: ");
                var input = Console.ReadKey();
                Console.WriteLine();
                Console.WriteLine();

                switch (input.Key)
                {
                    case ConsoleKey.F1:
                        await onLogin();
                        break;
                    case ConsoleKey.F2:
                        await onFindCurso();
                        break;
                    case ConsoleKey.F3:
                        await onSeeCurso();
                        break;
                    case ConsoleKey.F4:
                        await onCompletCurso();
                        break;
                    default:
                        Console.WriteLine("opcion invalida.");
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