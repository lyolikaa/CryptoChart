using API;

namespace WebClient;

public interface IDataService
{
    Task<Snapshot> GetSnapshot(int limit);

    double GetTotalPurchase(double amount, OrderLine[] asks);
    string TotalPurchaseString(double value);
}
public class DataService(HttpClient httpClient): IDataService
{
    public async Task<Snapshot> GetSnapshot(int limit = 0)
    {
        return await httpClient.GetFromJsonAsync<Snapshot>($"orderbook?limit={limit}");
    }

    public double GetTotalPurchase(double amount, OrderLine[] asks)
    {
        var i = 0;
        double totalAmount = 0;
        double totalCost = 0;
        var sortedByPrice = asks
            .OrderBy(o=>o.Price).ToArray();

        do
        {
            totalAmount += sortedByPrice[i].Amount;
            totalCost += CalcLineTotal(sortedByPrice[i]);

            i++;
        } while (totalAmount < amount && i < sortedByPrice.Length);

        //#OA cannot reach desired amount with current snapshot
        if (totalAmount < amount)
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
            default: return value.ToString();
        }
    }
}