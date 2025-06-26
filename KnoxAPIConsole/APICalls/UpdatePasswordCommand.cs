using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UpdatePasswordCommand : IKnoxCommand{
    private readonly string _tabletNumber;
    private const string endpoint = "https://us01.manage.samsungknox.com/emm/oapi/user/updatePassword";

    public UpdatePasswordCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task ExecuteAsync() {
        Console.WriteLine("\nUpdating Password...");

        string last4 = _tabletNumber.Length >= 4 ? _tabletNumber[^4..] : _tabletNumber;
        string password = $"HCI@@11{last4}";

        var payload = new[] {
            new KeyValuePair<string, string>("userId", _tabletNumber),
            new KeyValuePair<string, string>("userPassword", password)
        };

        JObject? json = await HttpHelper.PostFormAsync(endpoint, payload);
        Console.WriteLine(json != null ? "Success!" : "Failed to update password.");
    }
}
