using API;

namespace WebClient;

public interface IDataService
{
    Task<Snapshot> GetSnapshot(int limit);
}
public class DataService(HttpClient httpClient): IDataService
{
    public async Task<Snapshot> GetSnapshot(int limit)
    {
        return await httpClient.GetFromJsonAsync<Snapshot>($"orderbook?limit={limit}");
    }
}