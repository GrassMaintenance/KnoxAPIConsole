using KnoxAPIConsole.Helpers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class PowerOffCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private const string powerOffEndPoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForPowerOff";

    public PowerOffCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task<object?> ExecuteAsync() {
        Console.Clear();

        try {
            string? deviceId = await GetDeviceIdAsync();
            if (string.IsNullOrWhiteSpace(deviceId))
                Console.WriteLine("Cannot find Device ID.");

            var result = await SendPowerOffRequestAsync(deviceId);
        } catch (Exception ex) {
            Console.WriteLine(ex);
            Console.Read();
        }
        
        return null;
    }


    private async Task<JObject?> SendPowerOffRequestAsync(string? deviceId) {

        var deviceIdArray = new[] { deviceId };
        var payload = new[] {
            new KeyValuePair<string, string>("deviceIds", JsonConvert.SerializeObject(deviceIdArray))
        };

        return await HttpHelper.PostFormAsync(powerOffEndPoint, payload);
    }

    private async Task<string?> GetDeviceIdAsync() {
        string? deviceId = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (string.IsNullOrWhiteSpace(deviceId)) {
            Console.WriteLine("Device ID not found.");
        }

        return deviceId;
    }
}
