namespace API;

public interface IDataProvider
{
    Snapshot GetSnapshot();
    bool StoreSnapshot(Snapshot snapshot);
}

public class BinanceDataProvider: IDataProvider
{
    public Snapshot GetSnapshot()
    {
        throw new NotImplementedException();
    }

    public bool StoreSnapshot(Snapshot snapshot)
    {
        throw new NotImplementedException();
    }
}

public class FakeDataProvider : IDataProvider
{
    public Snapshot GetSnapshot()
    {
        return new Snapshot
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
    }

    public bool StoreSnapshot(Snapshot snapshot)
    {
        throw new NotImplementedException();
    }
}