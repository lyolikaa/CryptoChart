namespace API;

public class OrderBook
{
    
}

public class Snapshot()
{
    public DateTime Date { get; set; }
    public OrderLine[] Bids { get; set; }
    public OrderLine[] Asks { get; set; }
}

public class OrderLine
{
    public double Price { get; set; }
    public double Amount { get; set; }
}