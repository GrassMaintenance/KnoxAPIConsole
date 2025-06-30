using Newtonsoft.Json.Linq;
using KnoxAPIConsole.Helpers;

namespace KnoxAPIConsole.APICalls;

public class FactoryResetCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private const string endpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForFactoryReset";

    public FactoryResetCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task<object?> ExecuteAsync() {
        return await Animator.PlayUntilComplete<object?>("Resetting tablet", async () => {
            string? deviceID = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
            if (deviceID == null) {
               Console.WriteLine("Device ID not found.");
               return null;
            }

            var payload = new[] {
                new KeyValuePair<string, string>("deviceId", deviceID)
            };

            JObject? json = await HttpHelper.PostFormAsync(endpoint, payload);
            Console.WriteLine(json != null ? "Success!" : "Failed to factory reset the device.");
            return null; 
        });
    }

}