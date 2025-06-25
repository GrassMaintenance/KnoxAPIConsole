using KnoxAPIConsole.Client;
using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class PushProfileCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint;
    
    public PushProfileCommand(string tabletNumber) { 
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("\nUpdating Profile...");
        string? deviceID = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (deviceID != null) { await UpdateProfile(deviceID); }
    }

    private async Task UpdateProfile(string deviceID) {
        endpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForUpdateProfile";

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
            Console.WriteLine("Error pushing profile: " + ex.Message);
        }
    }
}