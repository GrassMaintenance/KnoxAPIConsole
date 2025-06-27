using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class GetDeviceVersionCommand : IKnoxCommand {
    private readonly string _tabletNumber;

    public GetDeviceVersionCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        JObject? metadata = await DeviceHelper.GetDeviceMetadataAsyncByTabletNumber(_tabletNumber);
        if(metadata == null) {
            Console.WriteLine("Could not retrieve device metadata.");
            return;
        }

        string? version = metadata["deviceVersionName"]?.ToString();
        if (!string.IsNullOrWhiteSpace(version)) {
            Console.WriteLine("\n" + version);
        } else {
            Console.WriteLine("Version information not found in metadata");
        }
    }
}