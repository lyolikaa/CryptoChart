using System.Text.Json;
using AutoMapper;
using DB;
using Microsoft.EntityFrameworkCore;

namespace API;

public interface IDataProvider
{
    Task<Snapshot> GetSnapshot();
    Task<int> StoreSnapshot(Snapshot snapshot);

    Task<bool> FindSnapshot(int id);
}

public class BinanceDataProvider(CryptoContext context, HttpClient httpClient, IMapper mapper) : IDataProvider
{
    private CryptoContext _context = context;
    private readonly string _apiUri = "depth?symbol=BTCEUR";
    private readonly IMapper _mapper = mapper;
    
    public async Task<Snapshot> GetSnapshot()
    {
        var data = await httpClient.GetFromJsonAsync<OrderBook>(_apiUri);
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
        
        return _mapper.Map<Snapshot>(data);
    }

    public async Task<int> StoreSnapshot(Snapshot snapshot)
    {
        var sn = new DB.Snapshot
        {
            SnapshotId = snapshot.Id,
            Date = snapshot.Date,
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