using PluginAPI;

namespace APIConsoleDEMO
{
    internal class Program
    {
        static bool running = true;
        static string summoner_name = "konejita";

        static void Main(string[] args)
        {
            onLoop();

            while (running)
            {

            }
        }

        static async void onLoop()
        {
            ApiManager.Int();

            string rotacion = await ApiManager.GetFreeRotation();

            int[] champions_ids = rotacion.GetValue<int[]>("freeChampionIds");

            for (int i = 0; i < champions_ids.Length; i++)
            {
                Console.WriteLine($"champion_id: {champions_ids[i]}");
            }
        }
    }
}