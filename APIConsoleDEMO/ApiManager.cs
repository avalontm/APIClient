using PluginAPI;

namespace APIConsoleDEMO
{
    public static class ApiManager
    {
        //Servidor de LAN
        static string host = "https://la1.api.riotgames.com";
        static string api = "RGAPI-f0e4eff0-fb17-4cbf-89e6-d39a50907794";
        static WebClientManager client { set; get; } = new WebClientManager(host);

        public static void Int()
        {
            client.CreateKey("X-Riot-Token" , api);
        }

        public static async Task<string> GetSummoner(string name)
        {
            try
            {
                return await client.GetAsync($"/lol/summoner/v4/summoners/by-name/{name}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }

        }


        public static async Task<string> GetFreeRotation()
        {
            try
            {
                return await client.GetAsync($"/lol/platform/v3/champion-rotations");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
