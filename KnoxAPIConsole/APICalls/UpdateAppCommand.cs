using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UpdateAppCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private const string appListEndpoint = "https://us01.manage.samsungknox.com/emm/oapi/device/selectDeviceAppList";

    public UpdateAppCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("Retrieving app list...");

        string? deviceId = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (deviceId == null) {
            Console.WriteLine("Device ID not found.");
            return;
        }

        string? orgCode = await OrganizationHelper.GetOrganizationCodeAsync(_tabletNumber);
        if (orgCode == null) {
            Console.WriteLine("Organization code not found.");
            return;
        }

        var payload = new[] {
            new KeyValuePair<string, string>("deviceId", deviceId)
        };

        JObject? response = await HttpHelper.PostFormAsync(appListEndpoint, payload);
        if (response == null) {
            Console.WriteLine("Failed to fetch app list.");
            return;
        }

        var appList = response["resultValue"]?["appList"] as JArray;
        if (appList == null || appList.Count == 0) {
            Console.WriteLine("No apps found.");
            return;
        }

        var filteredApps = AppFilterHelper.FilterAppsByOrg(orgCode, appList.Cast<JObject>());
        Console.WriteLine($"\nFiltered apps for org '{orgCode}':\n");
        foreach (var app in filteredApps) {
            Console.WriteLine(app["packageName"]?.ToString() ?? "(Unnamed App)");
        }

        Console.Read();
    }
}
