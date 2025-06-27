using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UninstallAppCommand : IKnoxCommand{
    private readonly string _tabletNumber;
    private const string uninstallAppEndpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForUninstallApp";
    public bool UseAnimation => false;

    public UninstallAppCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task<object?> ExecuteAsync() {
        Console.Clear();

        JObject? selectedApp = await PromptUserToSelectAppAsync();
        if (selectedApp == null) return null;
        
        string? deviceId = await GetDeviceIDAsync();
        if (string.IsNullOrWhiteSpace(deviceId)) return null;

        JObject? result = await SendUninstallRequestAsync(deviceId, selectedApp);
        LogUpdateResult(result, selectedApp);
        return null;
    }

    private async Task<JObject?> PromptUserToSelectAppAsync() {
        JObject? selectedApp = await AppSelectionHelper.PromptUserToSelectAppAsync(_tabletNumber);
        if (selectedApp == null) {
            Console.WriteLine("App selection cancelled");
        }

        return selectedApp;
    }

    private async Task<string?> GetDeviceIDAsync() {
        string? deviceId = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (string.IsNullOrWhiteSpace(deviceId)) {
            Console.WriteLine("Device ID not found.");
        }

        return deviceId;
    }

    private async Task<JObject?> SendUninstallRequestAsync(string deviceId, JObject selectedApp) {
        string? packageName = selectedApp["packageName"]?.ToString();
        if (string.IsNullOrWhiteSpace(packageName)) {
            Console.WriteLine("Invalid package name");
            return null;
        }

        var payload = new[] {
            new KeyValuePair<string, string>("appPackage", packageName),
            new KeyValuePair<string, string>("deviceId", deviceId)
        };

        return await HttpHelper.PostFormAsync(uninstallAppEndpoint, payload);
    }

    private void LogUpdateResult(JObject? result, JObject selectedApp) {
        string? packageName = selectedApp["packageName"]?.ToString();
        string displayName = AppFilterHelper.GetDisplayName(packageName ?? "(unknown)");

        if (result?["resultCode"]?.ToString() == "0") {
            Console.WriteLine($"\nSuccessfully uninstalled {displayName}.");
        } else {
            Console.WriteLine($"\nFailed to uninstall {displayName}.");
            Console.WriteLine(result?["resultMessage"]?.ToString() ?? "No response");
        }
    }
}

