using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.Helpers;

public static class AppSelectionHelper {
    public static async Task<JObject?> PromptUserToSelectAppAsync(string tabletNumber) {
        string? deviceId = await DeviceHelper.GetDeviceIDAsync(tabletNumber);
        if (deviceId == null) {
            Console.WriteLine("Device ID not found.");
            return null;
        }

        JObject? metadata = await DeviceHelper.GetDeviceMetadataAsync(deviceId);
        if (metadata == null) {
            Console.WriteLine("Failed to retrieve device metadata.");
            return null;
        }

        string? orgCode = metadata["orgCode"]?.ToString();
        if(string.IsNullOrWhiteSpace(orgCode)) {
            Console.WriteLine("Organization code not found.");
            return null;
        }

        JObject? appListJson = await HttpHelper.PostFormAsync(
            "https://us01.manage.samsungknox.com/emm/oapi/device/selectDeviceAppList",
            new[] {new KeyValuePair<string, string>("deviceId", deviceId)});

        JArray? apps = appListJson?["resultValue"]?["appList"] as JArray;
        if(apps == null || apps.Count == 0) {
            Console.WriteLine("No apps found.");
            return null;
        }

        var filtered = AppFilterHelper.FilterAppsByOrg(orgCode, apps.Cast<JObject>()).ToList();
        if (filtered.Count == 0) {
            Console.WriteLine("No apps matched the filter");
            return null;
        }

        Console.WriteLine("\nSelect an app:\n");
        for(int i = 0; i < filtered.Count; i++) {
            var pkg = filtered[i]["packageName"]?.ToString() ?? "(unknown)";
            Console.WriteLine($"{i + 1}. {AppFilterHelper.GetDisplayName(pkg)}");
        }

        Console.WriteLine("0. Cancel");

        int selection = -1;
        while (true) {
            Console.Write("\nEnter your choice: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out selection) && selection >= 0 && selection <= filtered.Count) {
                break; 
            } else {
                Console.WriteLine("Invalid input, please enter a number between 0 and " + filtered.Count);
            }
        }

        return selection == 0 ? null : filtered[selection - 1];
    }
}
