using KnoxAPIConsole.Client;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.Helpers;

public static class DeviceHelper {
    private const string deviceIdEndPoint = "https://us01.manage.samsungknox.com/emm/oapi/device/selectDevicesByUser";
    private const string deviceMetadataEndPoint = "https://us01.manage.samsungknox.com/emm/oapi/device/selectDeviceInfo";
    private static readonly Dictionary<string, JObject> _deviceMetadataCache = new();

    public static async Task<string?> GetDeviceIDAsync(string tabletNumber) {
        var payload = new[] {
            new KeyValuePair<string, string>("userId", tabletNumber)
        };

        try {
            var response = await ClientManager.client.PostAsync(deviceIdEndPoint, new FormUrlEncodedContent(payload));
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            var deviceArray = json["resultValue"] as JArray;
            var device = deviceArray?.FirstOrDefault() as JObject;
            return device?["deviceId"]?.ToString();
        } catch (Exception ex) {
            Console.WriteLine("Error retrieving device ID: " + ex.Message);
            return null;
        }
    }

    public static async Task<JObject?> GetDeviceMetadataAsync(string deviceId) {        
        if (string.IsNullOrWhiteSpace(deviceId)) { return null; }

        if (_deviceMetadataCache.TryGetValue(deviceId, out var cached)) {
            return cached;
        }

        var payload = new[] {
            new KeyValuePair<string, string>("deviceId", deviceId)
        };

        try {
            var response = await ClientManager.client.PostAsync(deviceMetadataEndPoint, new FormUrlEncodedContent(payload));
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            JObject? device = json["resultValue"] as JObject;

            if (device != null)
                _deviceMetadataCache[deviceId] = device;

            return device;
        } catch (Exception ex) {
            Console.WriteLine("Error retrieving device metadata: " + ex.Message);
            return null;
        }
    }
}
