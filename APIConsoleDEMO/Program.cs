using APIClient;


Console.WriteLine(Client.client.GetAsync(""));


public static class Client
{
    static WebClientManager _client;
    public static WebClientManager client => _client ?? (_client = new WebClientManager(string.Format("{0}/api", "")));
}
