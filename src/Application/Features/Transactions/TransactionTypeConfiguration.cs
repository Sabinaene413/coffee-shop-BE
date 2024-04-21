using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.TransactionTypes;

public class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.HasData(new List<TransactionType>() { new TransactionType() { Id = (long)TranscationTypeEnum.IN, Name = TranscationTypeEnum.IN.ToString() }
            , new TransactionType() { Id = (long)TranscationTypeEnum.OUT, Name = TranscationTypeEnum.OUT.ToString() } });
    }
}
