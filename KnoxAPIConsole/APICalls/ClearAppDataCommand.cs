using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class ClearAppDataCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private const string clearAppDataEndpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForDeleteAppData";
    
    public ClearAppDataCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task<object?> ExecuteAsync() {
        JObject? selectedApp = await PromptUserToSelectAppAsync();
        if(selectedApp == null) return null;

        string? deviceId = await GetDeviceIDAsync();
        if(deviceId == null) return null;

        JObject? result = await Animator.PlayUntilComplete(
            "Clearning app data",
            () => SendClearAppDataRquestAsync(deviceId, selectedApp));
        
        LogUpdateResult(result, selectedApp);
        return null;
    }

    private async Task<JObject?> PromptUserToSelectAppAsync() {
        JObject? selectedApp = await AppSelectionHelper.PromptUserToSelectAppAsync(_tabletNumber);
        if (selectedApp == null) {
            Console.WriteLine("App selection cancelled.");
        }

        return selectedApp;
    }

    private async Task<string?> GetDeviceIDAsync() {
        string? deviceId = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (string.IsNullOrWhiteSpace(deviceId)) {
            Console.WriteLine("DeviceID not found");
        }

        return deviceId;
    }

    private async Task<JObject?> SendClearAppDataRquestAsync(string deviceId, JObject selectedApp) {
        string? packageName = selectedApp["packageName"]?.ToString();
        if (string.IsNullOrWhiteSpace(packageName)) {
            Console.WriteLine("Invalid package name.");
            return null;
        }

        var payload = new[] {
            new KeyValuePair<string, string>("appPackage", packageName),
            new KeyValuePair<string, string>("deviceId", deviceId)
        };

        return await HttpHelper.PostFormAsync(clearAppDataEndpoint, payload);
    }

    private void LogUpdateResult(JObject? result, JObject selectedApp) {
        string? packageName = selectedApp["packageName"]?.ToString();
        string displayName = AppFilterHelper.GetDisplayName(packageName ?? "(unknown)");

        if (result?["resultCode"]?.ToString() == "0") {
            Console.WriteLine($"\nSuccessfully cleared app data for {displayName}");
        } else {
            Console.WriteLine($"\nFailed to clear app data for {displayName}");
            Console.WriteLine(result?.ToString() ?? "No response");
        }
    }
}
