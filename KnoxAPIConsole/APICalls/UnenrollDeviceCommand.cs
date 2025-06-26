using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UnenrollDeviceCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint;

    public UnenrollDeviceCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("\nUnenrolling Device");
        string? deviceID = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (deviceID == null) {
            Console.WriteLine("Device ID not found.");
            return;
        }

        var payload = new[] {
            new KeyValuePair<string, string>("deviceId", deviceID)
        };

        JObject? json = await HttpHelper.PostFormAsync(endpoint, payload);

        Console.WriteLine(json != null ? "Success!" : "Failed to unenroll device.");
    }
}