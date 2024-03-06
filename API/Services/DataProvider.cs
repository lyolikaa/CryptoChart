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
    Task<Snapshot> GetSnapshot();
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
    Task<bool> FindSnapshot(int id);
}

//TODO possibly to use generics for DTO models, API path etc. 
public class BinanceDataProvider(CryptoContext context, HttpClient httpClient) : IDataProvider
{
    private readonly CryptoContext _context = context;
    private readonly string _apiUri = "depth?symbol=BTCEUR";
    
    public async Task<Snapshot> GetSnapshot()
    {
        var data = await httpClient.GetFromJsonAsync<OrderBookBinance>(_apiUri);
        //TODO possibly to use Automapper
        var snapshot = new Snapshot
        {
            Date = DateTime.Now,
            Bids = data.Bids.Select(b => 
                new OrderLine{ Price = double.Parse(b[0]), Amount = double.Parse(b[1])}).ToArray(),
            Asks = data.Asks.Select(b => 
                new OrderLine{ Price = double.Parse(b[0]), Amount = double.Parse(b[1])}).ToArray(),
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
            //- easy to retrieve from archive and seserialize
            Bids = JsonSerializer.Serialize(snapshot.Bids),
            Asks = JsonSerializer.Serialize(snapshot.Asks),
        };
        _context.Snapshots.Add(sn); 
        return await _context.SaveChangesAsync();
    }

    public async Task<bool> FindSnapshot(int id)
    {
        return await _context.Snapshots.FirstOrDefaultAsync(s => s.SnapshotId == id) != null;
    }
}

public class FakeDataProvider : IDataProvider
{
    public Task<Snapshot> GetSnapshot()
    {
        var data = new Snapshot
        {
            Date = DateTime.Now,
            Bids = new[]
            {
                new OrderLine { Price = 60228.46000000, Amount = 0.01527000 },
                new OrderLine { Price = 5555.46000000, Amount = 0.2 },
                new OrderLine { Price = 60213.03000000, Amount = 0.00733000 }
            },
            Asks = new[]
            {
                new OrderLine { Price = 60228.47000000, Amount = 0.10388000 },
                new OrderLine { Price = 4444.46000000, Amount = 0.3 },
                new OrderLine { Price = 60231.03000000, Amount = 0.09653000 }
            },
        };

        return Task.FromResult(data);
    }

    Task<int> IDataProvider.StoreSnapshot(Snapshot snapshot)
    {
        throw new NotImplementedException();
    }

    public Task<bool> FindSnapshot(int id)
    {
        throw new NotImplementedException();
    }
}