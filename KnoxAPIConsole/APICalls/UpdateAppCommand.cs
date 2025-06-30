using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UpdateAppCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private const string installAppEndpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForInstallApp";

    public UpdateAppCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task<object?> ExecuteAsync() {
        Console.Clear();

        JObject? selectedApp = await PromptUserToSelectAppAsync();
        if (selectedApp == null) return null;

        string? deviceId = await GetDeviceIdAsync();
        if (string.IsNullOrWhiteSpace(deviceId)) return null;

        JObject? result = await SendUpdateRequestAsync(deviceId, selectedApp);
        LogUpdateResult(result, selectedApp);
        return null;
    }

    private async Task<JObject?> PromptUserToSelectAppAsync() {
        JObject? selectedApp = await AppSelectionHelper.PromptUserToSelectAppAsync( _tabletNumber);
        if(selectedApp == null) {
            Console.WriteLine("App selection cancelled.");
        }

        return selectedApp;
    }

    private async Task<string?> GetDeviceIdAsync() {
        string? deviceId = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (string.IsNullOrWhiteSpace(deviceId)) {
            Console.WriteLine("Device ID not found.");
        }

        return deviceId;
    }

    private async Task<JObject?> SendUpdateRequestAsync(string deviceId, JObject selectedApp) {
        string? packageName = selectedApp["packageName"]?.ToString();
        if (string.IsNullOrWhiteSpace(packageName)) {
            Console.WriteLine("Invalid package name.");
            return null;
        }

        var payload = new[] {
            new KeyValuePair<string, string>("deviceId", deviceId),
            new KeyValuePair<string, string>("packageName", packageName),
            new KeyValuePair<string, string>("appInstallType", "FORCE"),
            new KeyValuePair<string, string>("isLatest", "true")
        };

        return await HttpHelper.PostFormAsync(installAppEndpoint, payload);
    }

    private void LogUpdateResult(JObject? result, JObject selectedApp) {
        string? packageName = selectedApp["packageName"]?.ToString();
        string displayName = AppFilterHelper.GetDisplayName(packageName ?? "(unknown)");

        if (result?["result"]?.ToString() == "success") {
            Console.WriteLine($"\nSuccessfully triggered update to {displayName}.");
        } else {
            Console.WriteLine($"\nFailed to trigger update for {displayName}.");
            Console.WriteLine(result?.ToString() ?? "No response.");
        }
    }
}
