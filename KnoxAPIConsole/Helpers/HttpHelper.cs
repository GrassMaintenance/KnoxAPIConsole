using KnoxAPIConsole.Client;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.Helpers;

public static class HttpHelper {
    public static async Task<JObject?> PostFormAsync(string url, IEnumerable<KeyValuePair<string, string>> formData) {
        try {
            using var content = new FormUrlEncodedContent(formData);
            var response = await ClientManager.client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();

            string responseBody = await response.Content.ReadAsStringAsync();
            return JObject.Parse(responseBody);
        } catch (Exception ex) {
            Console.WriteLine($"HTTP POST failed: {ex.Message}");
            return null;
        }
    }
}