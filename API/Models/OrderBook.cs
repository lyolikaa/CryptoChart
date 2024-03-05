namespace API;

public class OrderBook
{
    public int LastUpdateId { get; set; }
    public DateTime Date { get; set; }
    public string[][] Bids { get; set; }
    public string[][] Asks { get; set; }
}

public class Snapshot()
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public OrderLine[] Bids { get; set; }
    public OrderLine[] Asks { get; set; }
}

public class OrderLine
{
    public double Price { get; set; }
    public double Amount { get; set; }
}