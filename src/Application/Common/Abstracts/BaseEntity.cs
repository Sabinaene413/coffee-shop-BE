﻿namespace MyCoffeeShop.Application.Common.Abstracts;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public bool Active { get; set; } = true;
    public DateTime CreatedAt { get; set; }
    public long? CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public long? UpdatedBy { get; set; }
}