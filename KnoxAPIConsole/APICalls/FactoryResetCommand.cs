using KnoxAPIConsole.Client;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class FactoryResetCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint;

    public FactoryResetCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("\nResetting Tablet");
        string? deviceID = await GetDeviceID();
        if(deviceID != null) await FactoryReset(deviceID);
    }

    private async Task<string?> GetDeviceID() {
        endpoint = "https://us01.manage.samsungknox.com/emm/oapi/device/selectDevicesByUser";

        FormUrlEncodedContent payload = new([
            new KeyValuePair<string, string>("userId", _tabletNumber)
        ]);

        try {
            var response = await ClientManager.client.PostAsync(endpoint, payload);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);
            return json["resultValue"]?[0]?["deviceId"].ToString();
        } catch (Exception ex) {
            Console.WriteLine("Error trying to factory reset: " + ex);
            return null;
        }
    }

    private async Task FactoryReset(string deviceID) {
        endpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForFactoryReset";

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
            Console.WriteLine("Error factory resetting device: " + ex);
        }
    }
}