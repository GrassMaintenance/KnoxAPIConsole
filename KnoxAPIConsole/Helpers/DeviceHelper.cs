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

        JObject? json = await HttpHelper.PostFormAsync(deviceIdEndPoint, payload);
        var deviceArray = json?["resultValue"] as JArray;
        var device = deviceArray?.FirstOrDefault() as JObject;
        if (device == null) {
            Console.WriteLine("No device found for the given tablet number.");
            return null;
        }

        return device["deviceId"]?.ToString();
    }

    public static async Task<JObject?> GetDeviceMetadataAsync(string deviceId) {        
        if (string.IsNullOrWhiteSpace(deviceId)) { return null; }

        if (_deviceMetadataCache.TryGetValue(deviceId, out var cached)) {
            return cached;
        }

        var payload = new[] {
            new KeyValuePair<string, string>("deviceId", deviceId)
        };

        JObject? json = await HttpHelper.PostFormAsync(deviceMetadataEndPoint, payload);
        JObject? device = json?["resultValue"] as JObject;

        if (device != null) {
            _deviceMetadataCache[deviceId] = device;
        } else {
            Console.WriteLine("Device metadata was not found in the response");
        }

        return device;
    }

    public static async Task<JObject?> GetDeviceMetadataAsyncByTabletNumber(string tabletNumber) {
        string? deviceId = await GetDeviceIDAsync(tabletNumber);
        return string.IsNullOrWhiteSpace(deviceId) ? null : await GetDeviceMetadataAsync(deviceId);
    }
}
