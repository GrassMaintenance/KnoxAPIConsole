using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.Helpers;

public static class OrganizationHelper {
    public static async Task<string?> GetOrganizationCodeAsync(string tabletNumber) {
        string? deviceId = await DeviceHelper.GetDeviceIDAsync(tabletNumber);
        if (deviceId == null) { return null; }

        JObject? device = await DeviceHelper.GetDeviceMetadataAsync(deviceId);
        return device?["orgCode"]?.ToString()
            ?? device?["orgName"]?.ToString();
    }

    public static string ParseOrganizationCodeFromTabletNumber(string tabletNumber) {
        return tabletNumber.Length >= 4 && tabletNumber.StartsWith("11")
            ? tabletNumber.Substring(2, 2).ToUpper()
            : "UNKNOWN";
    }
}