using KnoxAPIConsole.Client;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class ChangeOrganizationCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint;
    private PushProfileCommand pushProfileCommand;
    private readonly List<KeyValuePair<string, string>> organizationCodes = new() {
           new("HomeCityDeliveryFleet", "Operations"),
           new("HomeCityDeliveryFleetEnterprise", "HomeCityDeliveryFleetEnterprise"),
           new("Drivers (Private)", "Enterprise-Private-Drivers"),
           new("Drivers (Public)", "Enterprise-Public-Drivers"),
           new("Mechanics", "Enterprise-Private-Mechanic"),
           new("Productin (Legacy)", "Production"),
           new("Production (Enterprise)", "ProductionEnterprise"),
           new("Production (Private)", "Enterprise-Private-Production"),
           new("Production (Public)", "Enterprise-Public-Production"),
           new("Time Clock", "TimeClock")
    };
    
    public ChangeOrganizationCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
        endpoint = "https://us01.manage.samsungknox.com/emm/oapi/user/updateUser";
        pushProfileCommand = new(_tabletNumber);
    }

    public async Task<object?> ExecuteAsync() {
        string? orgCode = await GetOrgCodeSelection(organizationCodes);
        if(orgCode != null) await UpdateOrganzation(orgCode);
        await pushProfileCommand.ExecuteAsync();
        
        return null;
    }

    private async Task<string?> GetOrgCodeSelection(List<KeyValuePair<string, string>> orgCodes) {
        Console.Clear();
        
        for (int i = 0; i < orgCodes.Count; i++) {
            Console.WriteLine($"{i + 1}. {orgCodes[i].Key}");
        }

        while (true) {
            Console.Write("\nSelect a new organization for this tablet (or type 'exit' to cancel): ");
            string? input = Console.ReadLine();

            if(string.IsNullOrWhiteSpace(input)) {
                Console.WriteLine("Input cannot be empty. Please enter a number from the list.");
                continue;
            }

            if (input.Trim().ToLower() == "exit")  
                return null;
            if (int.TryParse(input, out int selection) && selection >= 1 && selection <= orgCodes.Count)
                return orgCodes[selection - 1].Value;
        }
    }

    private async Task<object?> UpdateOrganzation(string orgCode) {
        FormUrlEncodedContent payload = new([
            new KeyValuePair<string, string>("userId", _tabletNumber),
            new KeyValuePair<string, string>("userName", _tabletNumber),
            new KeyValuePair<string, string>("orgCode", orgCode)
        ]);

        try {
            var response = await ClientManager.client.PostAsync(endpoint, payload);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(content);

            return null;
        } catch (Exception ex) {
            Console.WriteLine("Error updating organization: " + ex.Message);
            return null;
        }
    }
}