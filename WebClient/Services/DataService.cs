using API;

namespace WebClient;

public interface IDataService
{
    Task<Snapshot> GetSnapshot(int limit);

    double GetTotalPurchase(OrderLine baseOrder, OrderLine[] asks);
    string TotalPurchaseString(double value);
}
public class DataService(HttpClient httpClient): IDataService
{
    public async Task<Snapshot> GetSnapshot(int limit)
    {
        return await httpClient.GetFromJsonAsync<Snapshot>($"orderbook?limit={limit}");
    }

    public double GetTotalPurchase(OrderLine baseOrder, OrderLine[] asks)
    {
        var i = 0;
        double totalAmount = 0;
        double totalCost = 0;
        var availableByPrice = asks
            .Where(o => o.Price >= baseOrder.Price)
            .OrderBy(o=>o.Price).ToArray();
        if (!availableByPrice.Any())
            return -1;
        do
        {
            totalAmount += availableByPrice[i].Amount;
            totalCost += CalcLineTotal(availableByPrice[i]);

            i++;
        } while (totalAmount < baseOrder.Amount && i < availableByPrice.Length);

        //#OA marker for UI - cannot reach desired amount with current snapshot
        if (totalAmount < baseOrder.Amount)
            totalCost = 0;

        return totalCost;

        double CalcLineTotal(OrderLine line) => line.Amount * line.Price;
    }

    public string TotalPurchaseString(double value)
    {
        switch (value)
        {
            case > 0:
                return value.ToString("0.#####");
            case 0:
                return "Not enough line to reach Amount";
            case -1:
                return "Order Price to low for available Asks";
            default: return value.ToString();
        }
    }
}