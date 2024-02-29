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

            List<ConsoleMenuItem> opciones = new List<ConsoleMenuItem>();
            opciones.Add(new ConsoleMenuItem() { Title = "Login", Action = onLogin });
            opciones.Add(new ConsoleMenuItem() { Title = "Buscar Curso", Action = onFindCurso });
            opciones.Add(new ConsoleMenuItem() { Title = "Ver Cursos", Action = onSeeCursos });
            opciones.Add(new ConsoleMenuItem() { Title = "Completar Curso", Action = onCompletCurso });
            opciones.Add(new ConsoleMenuItem() { Title = "Salir", Action = onExit });
            
            ConsoleEx.Menu("Mini Curso", opciones);

            while (running)
            {
               
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

        private static async Task onSeeCursos()
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

        static async Task onExit()
        {
            running = false;
            Environment.Exit(0);
        }

        static void Continue()
        {
            //esperamos que se precione cualquier tecla.
            Console.WriteLine("Preciona una tecla para continuar...");
            Console.ReadKey();
        }
    }
}