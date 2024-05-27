using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.TransactionTypes;

public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.HasData(new List<TransactionType>() { new TransactionType() { Id = (long)TransactionTypeEnum.CUMPARARE, Name = TransactionTypeEnum.CUMPARARE.ToString() }
            , new TransactionType() { Id = (long)TransactionTypeEnum.VANZARE, Name = TransactionTypeEnum.VANZARE.ToString() } , new TransactionType() { Id = (long)TransactionTypeEnum.PLATA, Name = TransactionTypeEnum.PLATA.ToString() } });
    }
}
