using System.Text.Json;
using DB;
using Microsoft.EntityFrameworkCore;

namespace API;

public interface IDataProvider
{
    /// <summary>
    /// Retrieve data specifically from each provider
    /// </summary>
    /// <returns></returns>
    Task<Snapshot> GetSnapshot(int limit);
    /// <summary>
    /// Save to DB
    /// </summary>
    /// <param name="snapshot"></param>
    /// <returns></returns>
    Task<int> StoreSnapshot(Snapshot snapshot);
    /// <summary>
    /// Check for existing snapshot
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> FindSnapshot(ulong id);
}

//TODO possibly to use generics for DTO models, API path etc. 
public class BinanceDataProvider(CryptoContext context, HttpClient httpClient) : IDataProvider
{
    private readonly CryptoContext _context = context;
    private readonly string _apiUri = "depth?symbol=BTCEUR";
    
    public async Task<Snapshot> GetSnapshot(int limit = 0)
    {
        var limitStr = limit <= 0 ? "" : $"&limit={limit}";
        var data = await httpClient.GetFromJsonAsync<OrderBookBinance>(_apiUri + limitStr);
        //TODO possibly to use Automapper
        var snapshot = new Snapshot
        {
            Date = DateTime.Now,
            Bids = data.Bids.Select(b => 
                new OrderLine( double.Parse(b[0]), double.Parse(b[1]))).ToArray(),
            Asks = data.Asks.Select(b => 
                new OrderLine(double.Parse(b[0]), double.Parse(b[1]))).ToArray(),
            Id = data.LastUpdateId
        };
        if (!await FindSnapshot(data.LastUpdateId))
        {
            var res = await StoreSnapshot(snapshot);
        }

        return snapshot;
    }

    public async Task<int> StoreSnapshot(Snapshot snapshot)
    {
        var sn = new DB.Snapshot
        {
            SnapshotId = snapshot.Id,
            Date = snapshot.Date,
            //#OA decided to store as string:
            //- not need to search by particular item
            //- easy to retrieve from archive and deserialize
            Bids = JsonSerializer.Serialize(snapshot.Bids),
            Asks = JsonSerializer.Serialize(snapshot.Asks),
        };
        _context.Snapshots.Add(sn); 
        return await _context.SaveChangesAsync();
    }

    public async Task<bool> FindSnapshot(ulong id)
    {
        return await _context.Snapshots.FirstOrDefaultAsync(s => s.SnapshotId == id) != null;
    }
}
