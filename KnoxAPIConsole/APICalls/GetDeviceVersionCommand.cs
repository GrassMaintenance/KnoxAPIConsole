using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class GetDeviceVersionCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    public bool UseAnimation => true;

    public GetDeviceVersionCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task<object?> ExecuteAsync() {
        JObject? metadata = await DeviceHelper.GetDeviceMetadataAsyncByTabletNumber(_tabletNumber);
        if(metadata == null) {
            Console.WriteLine("Could not retrieve device metadata.");
            return null;
        }

        string? version = metadata["deviceVersionName"]?.ToString();
        if (!string.IsNullOrWhiteSpace(version)) {
            Console.WriteLine("\n" + version);
            return null;
        } else {
            Console.WriteLine("Version information not found in metadata");
            return null;
        }
    }
}