using KnoxAPIConsole.Client;
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
        if (deviceID != null) { await UnenrollDevice(deviceID); }
    }

    private async Task UnenrollDevice(string deviceID) {
        endpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForUnEnrollment";

        FormUrlEncodedContent payload = new([
            new KeyValuePair<string, string>("deviceId", deviceID)
        ]);

        try {
            var response = await ClientManager.client.PostAsync(endpoint, payload);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            Console.WriteLine("Success!");
        } catch (Exception ex) {
            Console.WriteLine("Error unenrolling device: " + ex.Message);
        }
    }
}