using KnoxAPIConsole.Client;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UpdateAppCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint;

    public UpdateAppCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        string? deviceID = await GetDeviceID();
        if (deviceID != null) { GetAppList(deviceID); } 
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
