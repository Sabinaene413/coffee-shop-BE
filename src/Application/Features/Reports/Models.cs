namespace MyCoffeeShop.Application.Features.Reports
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
        public int NoOfSalesCurrentMonth { get; set; }
        public int NoOfSelledItems { get; set; }
        public int NoOfSelledItemsCurrentMonth { get; set; }

    }
}
