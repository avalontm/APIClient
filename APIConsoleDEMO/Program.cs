using APIClient;


Console.WriteLine(await Client.client.GetAsync("users.json"));
Console.Read();

public static class Client
{
    static WebClientManager _client;
    public static WebClientManager client => _client ?? (_client = new WebClientManager(string.Format("{0}/api", "https://raw.githubusercontent.com/avalontm/APIClient/master")));
}
