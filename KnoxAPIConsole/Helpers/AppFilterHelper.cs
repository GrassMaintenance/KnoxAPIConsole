using Newtonsoft.Json.Linq;

namespace KnoxAPIConsole.Helpers;

public static class AppFilterHelper {
    private static readonly Dictionary<string, HashSet<string>> allowedApps = new() {
        {"Enterprise-Public-Drivers", new() {
            "com.sds.emm.cloud.knox.samsung",
            "com.homecityice.icedelivery",
            "com.keeptruckin.android",
            "com.android.chrome",
            "com.squareup",
            "com.usbank.oed" }
        },

        {"Enterprise-Private-Drivers", new() {
            "com.sds.emm.cloud.knox.samsung",
            "com.homecityice.icedelivery",
            "com.keeptruckin.android",
            "com.android.chrome",
            "com.squareup",
            "com.usbank.oed" }
        },

        {"Operations", new() {
            "com.sds.emm.cloud.knox.samsung",
            "com.homecityice.icedelivery",
            "com.android.chrome",
            "com.usbank.oed"} 
        },

        {"HomeCityDeliveryFleetEnterprise", new() {
            "com.sds.emm.cloud.knox.samsung",
            "com.homecityice.icedelivery",
            "com.android.chrome",
            "com.usbank.oed"}
        },

        {"Enterprise-Public-Production", new() {
            "com.sds.emm.cloud.knox.samsung",
            "com.android.chrome",
            "com.hha" }
        },

        {"Enterprise-Private-Mechanic", new() { 
            "com.acumatica.androidapp",
            "com.snapinspect.snapinspect3",
            "com.android.chrome" } 
        },

        {"TimeClock", new() {
            "com.sds.emm.cloud.knox.samsung",
            "com.android.chrome" } 
        }
    };

    public static IEnumerable<JObject> FilterAppsByOrg(string orgCode, IEnumerable<JObject> apps) {
        if(!allowedApps.TryGetValue(orgCode, out var allowedSet)) {
            return apps;
        }

        return apps.Where(app => {
            string? packageName = app["packageName"]?.ToString();
            return packageName != null && allowedSet.Contains(packageName);
        });
    }
}