using KnoxAPIConsole.Client;
using Newtonsoft.Json.Linq;


namespace KnoxAPIConsole.APICalls;

public class ChangeOrganizationCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint;
    
    public ChangeOrganizationCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("\nUpdating organization...");
        endpoint = "https://us01.manage.samsungknox.com/emm/oapi/user/updateUser";

        FormUrlEncodedContent payload = new([
            new KeyValuePair<string, string>("userId", _tabletNumber),
            new KeyValuePair<string, string>("orgCode", "Enterprise-Public-Production")
        ]);

        try {
            var response = await ClientManager.client.PostAsync(endpoint, payload);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            using PushProfileCommand {

            }

            Console.WriteLine("Success!");
        } catch (Exception ex) {
            Console.WriteLine("Error updating organization: " + ex.Message);
        }
    }
}