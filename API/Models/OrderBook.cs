namespace API;

/// <summary>
/// Use for retrieve data from Binance API
/// </summary>
public class OrderBookBinance
{
    public ulong LastUpdateId { get; set; }
    public DateTime Date { get; set; }
    public string[][] Bids { get; set; }
    public string[][] Asks { get; set; }
}

/// <summary>
/// DTO model, use on API and Client
/// </summary>
public class Snapshot
{
    public ulong Id { get; set; }
    public DateTime Date { get; set; }
    public OrderLine[] Bids { get; set; }
    public OrderLine[] Asks { get; set; }
}

public class OrderLine(double price, double amount)
{
    public OrderLine() : this(0, 0)
    {
        
    }
    public double Price { get; set; } = price;
    public double Amount { get; set; } = amount;
}