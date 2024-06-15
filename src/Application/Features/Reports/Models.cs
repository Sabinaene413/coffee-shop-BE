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
}
