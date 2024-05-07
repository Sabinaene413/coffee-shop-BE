using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.TransactionTypes;

public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.HasData(new List<TransactionType>() { new TransactionType() { Id = (long)TransactionTypeEnum.IN, Name = TransactionTypeEnum.IN.ToString() }
            , new TransactionType() { Id = (long)TransactionTypeEnum.OUT, Name = TransactionTypeEnum.OUT.ToString() } });
    }
}
