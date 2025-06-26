using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class PushProfileCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForUpdateProfile";

    public PushProfileCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("\nUpdating Profile");

        string? deviceID = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (deviceID == null) {
            Console.WriteLine("Device ID not found.");
            return;
        }

        var payload = new[] {
            new KeyValuePair<string, string>("deviceId", deviceID)
        };

        JObject json = await HttpHelper.PostFormAsync(endpoint, payload);

        Console.WriteLine(json != null ? "Success!" : "Failed to push profile to device.");
    }
}