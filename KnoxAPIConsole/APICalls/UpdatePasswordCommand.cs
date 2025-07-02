using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class UpdatePasswordCommand : IKnoxCommand{
    private readonly string _tabletNumber;
    private const string endpoint = "https://us01.manage.samsungknox.com/emm/oapi/user/updatePassword";

    public UpdatePasswordCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task<object?> ExecuteAsync() {
        return await Animator.PlayUntilComplete<object?>("Updating password", async () => {

            string last4 = _tabletNumber.Length >= 4 ? _tabletNumber[^4..] : _tabletNumber;
            string password = $"HCI@@11{last4}";

            var payload = new[] {
                new KeyValuePair<string, string>("userId", _tabletNumber),
                new KeyValuePair<string, string>("userPassword", password)
            };

            JObject? json = await HttpHelper.PostFormAsync(endpoint, payload);
            Console.WriteLine(json != null ? "\nSuccess!" : "\nFailed to update password.");

            return null;
        });
    }
}
