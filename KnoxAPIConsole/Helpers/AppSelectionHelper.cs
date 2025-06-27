using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.Helpers;

public static class AppSelectionHelper {
    public static async Task<JObject?> PromptUserToSelectAppAsync(string tabletNumber) {
        List<JObject>? filteredApps = await Animator.PlayUntilComplete(
            "Gathering app list",
            async () => {
                string? deviceId = await GetDeviceIDAsync(tabletNumber);
                if (string.IsNullOrWhiteSpace(deviceId)) return new List<JObject>();

                JObject? metadata = await GetDeviceMetadata(deviceId);
                if (metadata == null) return new List<JObject>();

                string? orgCode = GetOrgCode(metadata);
                if (string.IsNullOrWhiteSpace(orgCode)) return new List<JObject>();

                return await GetFilteredAppList(deviceId, orgCode);
            }
        );

        if (filteredApps == null || filteredApps.Count == 0) return null;

        return PromptUserToChooseApp(filteredApps);
    }

    private static async Task<string?> GetDeviceIDAsync(string tabletNumber) {
        string? deviceId = await DeviceHelper.GetDeviceIDAsync(tabletNumber);
        if (deviceId == null) {
            Console.WriteLine("Device ID not found.");
            return null;
        }
         
        return deviceId;
    }


    private static async Task<JObject?> GetDeviceMetadata(string deviceId) {
        JObject? metadata = await DeviceHelper.GetDeviceMetadataAsync(deviceId);
        if (metadata == null) {
            Console.WriteLine("Failed to retrieve device metadata");
            return null;
        }

        return metadata;
    }

    private static string? GetOrgCode(JObject? metadata) {
        string? orgCode = metadata?["orgCode"]?.ToString();
        if (string.IsNullOrWhiteSpace(orgCode)) {
            Console.WriteLine("Organization code not found.");
            return null;
        }

        return orgCode;
    }

    private static async Task<List<JObject>?> GetFilteredAppList(string deviceId, string orgCode) {            
        JObject? appListJson = await HttpHelper.PostFormAsync(
            "https://us01.manage.samsungknox.com/emm/oapi/device/selectDeviceAppList",
             new [] { new KeyValuePair<string, string>("deviceId", deviceId) });

        JArray ? apps = appListJson?["resultValue"]?["appList"] as JArray;
        if (apps == null || apps.Count == 0) {
            Console.WriteLine("No apps found.");
            return null;
        }

        var filtered = AppFilterHelper.FilterAppsByOrg(orgCode, apps.Cast<JObject>()).ToList();
        if (filtered.Count == 0) {
            Console.WriteLine("No apps matched the filter");
            return null;
        }

        return filtered;
    }

    private static JObject? PromptUserToChooseApp(List<JObject> apps) {
        Console.Clear();
        Console.WriteLine("Select an app:\n");
        for (int i = 0; i < apps.Count; i++) {
            var pkg = apps[i]["packageName"]?.ToString() ?? "(unknown)";
            Console.WriteLine($"{i + 1}. {AppFilterHelper.GetDisplayName(pkg)}");
        }

        Console.WriteLine("0. Cancel");

        int selection = -1;
        while (true) {
            Console.Write("\nEnter your choice: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out selection) && selection >= 0 && selection <= apps.Count) {
                break;
            } else {
                Console.WriteLine("Invalid input, please enter a number between 0 and " + apps.Count);
            }
        }

        return selection == 0 ? null : apps[selection - 1];
    }
}
