using API;

namespace WebClient;

public interface IDataService
{
    Task<Snapshot> GetSnapshot();
}
public class DataService(HttpClient httpClient): IDataService
{
    public async Task<Snapshot> GetSnapshot()
    {
        return await httpClient.GetFromJsonAsync<Snapshot>("orderbook");
    }
}