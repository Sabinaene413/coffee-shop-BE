﻿using MyCoffeeShop.Application.Common.Abstracts;
using MyCoffeeShop.Application.Common.Mappings;

namespace MyCoffeeShop.Application.ShopOrders;

public class ShopOrder : BaseEntity
{
    public ShopOrder()
    {
    }
    public string Supplier { get; set; }
    public decimal Cost { get; set; }
    public DateTime? OrderDate { get; set; }
    public DateTime? ArrivalDate { get; set; }
    public bool Received { get; set; }
    public virtual List<ShopProductOrder> ShopOrderProducts { get; set; }
}

public class ShopOrderDto : IMapFrom<ShopOrder>
{
    public long Id { get; set; }
    public string Supplier { get; set; }
    public decimal Cost { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime ArrivalDate { get; set; }
    public bool Received { get; set; }

    public ShopProductOrderDto ShopOrderProducts { get; set; }

}