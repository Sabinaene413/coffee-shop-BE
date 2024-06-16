namespace MyCoffeeShop.Application.Reports
{

    public class ShopSalesDto
    {
        public DateTime? SaleDate { get; set; }
        public decimal Cost { get; set; }
        public int NoOfSales { get; set; }
        public int NoOfItemsSold { get; set; }
        public string Details { get; set; }

    }
    public class ShopOrdersDto
    {
        public DateTime? SaleDate { get; set; }
        public decimal Cost { get; set; }
        public int NoOfSales { get; set; }
        public int NoOfItemsSold { get; set; }
        public string Details { get; set; }

    }
    public class DashboardDto
    {
        public decimal Budget { get; set; }
        public int NoOfSales { get; set; }
        public int NoOfSalesLastMonth { get; set; }
        public int NoOfSalesCurrentMonth { get; set; }
        public double IncreasePercentageCurrentMonthSales { get; set; }
        public int NoOfSelledItems { get; set; }
        public int NoOfSelledItemsCurrentMonth { get; set; }

    }

    public class ProfitSixMonthsDto
    {
        public decimal Profit { get; set; }
        public decimal ProfitRate { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
        public List<ProfitGraphDto> ProfitGraphDtos { get; set; }
    }

    public class ProfitGraphDto
    {
        public DateTime Date { get; set; }
        public decimal Income { get; set; }
        public decimal Expenses { get; set; }
    }

    public class TopSalesDto
    {
        public string ProductName { get; set; }
        public int TotalSales { get; set; }
    }
}
