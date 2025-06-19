using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace KnoxAPIConsole.Client;

public class ClientManager {
    public static HttpClient client;
    public static string? token;

    public static async Task SetupClient() {
        client ??= new HttpClient();
        token = await GetAccessToken();
        SetHeaders();
    }

    private static async Task<string> GetAccessToken() {
        string url = string.Format("https://us01.manage.samsungknox.com/emm/oauth/token");

        List<KeyValuePair<string, string>> postData = new() {
            new ("grant_type", "client_credentials"),
            new ("client_id", "knox_manage_user@homecityice.com"),
            new ("client_secret", "xCnVZ8ZoTbV7^bzJSNq&hfLw")
        };

        FormUrlEncodedContent content = new(postData);
        content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

        HttpResponseMessage response = await client.PostAsync(url, content);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var tokenObj = JsonConvert.DeserializeObject<AccessToken>(json);
        return tokenObj.access_token;
    }

    private static void SetHeaders() {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}

