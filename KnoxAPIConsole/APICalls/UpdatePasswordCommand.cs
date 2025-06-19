using KnoxAPIConsole.Client;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UpdatePasswordCommand : IKnoxCommand{
    private readonly string _tabletNumber;

    public UpdatePasswordCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("\nUpdating Password...");
        string endpoint = "https://us01.manage.samsungknox.com/emm/oapi/user/updatePassword";

        FormUrlEncodedContent payload = new([
            new KeyValuePair<string, string>("userId", _tabletNumber),
            new KeyValuePair<string, string>("userPassword", $"HCI@@11{_tabletNumber[^4..]}")
        ]);

        try {
            var response = await ClientManager.client.PostAsync(endpoint, payload);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            Console.WriteLine("Successfully updated password!");
        } catch (Exception ex) {
            Console.WriteLine("Error updating password" + ex);
            Console.Read();
        }
    }
}
