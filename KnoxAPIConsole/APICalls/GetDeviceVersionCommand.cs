using KnoxAPIConsole.Client;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class GetDeviceVersionCommand : IKnoxCommand {
    private readonly string _tabletNumber;

    public GetDeviceVersionCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("\nFetching device version...");
        string endpoint = "https://us01.manage.samsungknox.com/emm/oapi/device/selectDevicesByUser";

        FormUrlEncodedContent payload = new([
             new KeyValuePair<string, string>("userId", _tabletNumber)
        ]);

        try {
            var response = await ClientManager.client.PostAsync(endpoint, payload);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            File.WriteAllText("C:\\Users\\bdevine\\Desktop\\version.txt", json["resultValue"]?[0]?["deviceVersionName"]?.ToString());
            Console.WriteLine("Success!");
        } catch (Exception ex) {
            Console.WriteLine("Error fetching device: " +  ex.Message);
            Console.Read();
        }
    }
}