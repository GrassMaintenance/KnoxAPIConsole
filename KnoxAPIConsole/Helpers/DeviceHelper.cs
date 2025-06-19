using KnoxAPIConsole.Client;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.Helpers;

public static class DeviceHelper{
    private const string deviceIdEndPoint = "https://us01.manage.samsungknox.com/emm/oapi/device/selectDevicesByUser";

    public static async Task<string?> GetDeviceIDAsync(string tabletNumber) {
        FormUrlEncodedContent payload = new([
            new KeyValuePair<string, string>("userId", tabletNumber)
        ]);

        try {
            var response = await ClientManager.client.PostAsync(deviceIdEndPoint, payload);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            return json["resultValue"]?[0]?["deviceId"]?.ToString();
        } catch (Exception ex) {
            Console.WriteLine("Error retrieving device ID: " + ex.Message);
            return null;
        }
    }
}