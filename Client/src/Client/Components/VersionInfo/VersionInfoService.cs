using System.Net.Http.Json;

namespace Client.Components.VersionInfo;

public class VersionInfoService(HttpClient httpClient)
{
    public async Task<VersionInfo?> GetVersionInfo()
    {
        return await httpClient.GetFromJsonAsync<VersionInfo>("version.json");
    }
}
