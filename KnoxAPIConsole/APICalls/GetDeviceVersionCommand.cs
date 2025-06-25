using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class GetDeviceVersionCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private const string endpoint = "https://us01.manage.samsungknox.com/emm/oapi/device/selectDevicesByUser";

    public GetDeviceVersionCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("\nFetching device version...");
        
        var payload = new[] {
             new KeyValuePair<string, string>("userId", _tabletNumber)
        };

        JObject? json = await HttpHelper.PostFormAsync(endpoint, payload);
        string? version = json?["resultValue"]?[0]?["deviceVersionName"]?.ToString();

        if (!string.IsNullOrWhiteSpace(version)) {
            File.WriteAllText("C:\\Users\\bdevine\\Desktop\\version.txt", version);
            Console.WriteLine("Success!");
        } else {
            Console.WriteLine("Failed to retrive version info.");
        }
    }
}