using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UnenrollDeviceCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint;

    public UnenrollDeviceCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
        endpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForUnEnrollment";
    }

    public async Task<object?> ExecuteAsync() {
        return await Animator.PlayUntilComplete<object?>("Unenrolling device", async () => {
            string? deviceID = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
            if (deviceID == null) {
                Console.WriteLine("Device ID not found.");
                return null;
            }

            var payload = new[] {
            new KeyValuePair<string, string>("deviceId", deviceID)
        };

            JObject? json = await HttpHelper.PostFormAsync(endpoint, payload);

            Console.WriteLine(json != null ? "Success!" : "Failed to unenroll device.");
            return null;
        });
    }
}