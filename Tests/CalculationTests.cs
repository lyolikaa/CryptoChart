using API;
using Moq;
using WebClient;

namespace Tests;

public class CalculationTests
{
    private readonly IDataService _dataService;
    private readonly Mock<HttpClient> _httpClientMock =new();

    public static OrderLine BaseOrder = new (10, 2);
    public static OrderLine OrderBigPrice = new (15, 2);
    public static OrderLine OrderBigAmount = new (11, 5);

    public CalculationTests()
    {
        _dataService = new DataService(_httpClientMock.Object);
    }


    public static IEnumerable<object[]> Data =>
        new List<OrderLine[]>
        {
            new [] { new OrderLine(11,2), new OrderLine(12,1) },
        };
        
    [Theory]
    [TestCaseSource(nameof(Data))]
    public void CalculateCorrectly(OrderLine[] asks)
    {
        var calc = _dataService.GetTotalPurchase(BaseOrder.Amount, asks);
        Assert.AreEqual(calc, 22);
    }
    
   
    [Theory]
    [TestCaseSource(nameof(Data))]
    public void OrderAmountTooMuch(OrderLine[] asks)
    {
        var calc = _dataService.GetTotalPurchase(OrderBigAmount.Amount, asks);
        Assert.AreEqual(calc, 0);
    }}
