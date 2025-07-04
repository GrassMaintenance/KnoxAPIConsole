﻿using KnoxAPIConsole.Helpers;
using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.APICalls;

public class PushProfileCommand : IKnoxCommand {
    private readonly string _tabletNumber;
    private string endpoint = "https://us01.manage.samsungknox.com/emm/oapi/mdm/commonOTCServiceWrapper/sendDeviceControlForUpdateProfile";
    public bool UseAnimation => true;

    public PushProfileCommand(string tabletNumber) {
        _tabletNumber = tabletNumber;
    }

    public async Task<object?> ExecuteAsync() {
        Console.WriteLine("\nUpdating Profile");

        string? deviceID = await DeviceHelper.GetDeviceIDAsync(_tabletNumber);
        if (deviceID == null) {
            Console.WriteLine("\nDevice ID not found.");
            return null;
        }

        var payload = new[] {
            new KeyValuePair<string, string>("deviceId", deviceID)
        };

        JObject? json = await HttpHelper.PostFormAsync(endpoint, payload);

        Console.WriteLine(json != null ? "\nSuccess!" : "\nFailed to push profile to device.");
        return null;
    }
}