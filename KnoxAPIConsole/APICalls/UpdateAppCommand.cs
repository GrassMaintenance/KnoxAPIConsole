using KnoxAPIConsole.Client;
using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UpdateAppCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint;

    public UpdateAppCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        string? deviceID = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (deviceID != null) { GetAppList(deviceID); } 
    }

    private async Task GetAppList(string deviceID) {
        endpoint = "https://us01.manage.samsungknox.com/emm/oapi/device/selectDeviceAppList";

        FormUrlEncodedContent payload = new([
            new KeyValuePair<string, string>("deviceId", deviceID)
        ]);

        try {
            var response = await ClientManager.client.PostAsync(endpoint, payload);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            foreach (JObject app in json["resultValue"]?["appList"] ?? new JArray()) {
                Console.WriteLine(app["packageName"]);
            }
        } catch (Exception ex) {
            Console.WriteLine("Error fetching app list: " + ex.Message);
        }
    }
}
