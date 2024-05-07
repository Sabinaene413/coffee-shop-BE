using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyCoffeeShop.Application.Transactions;

public class TransactionDetailConfiguration : IEntityTypeConfiguration<TransactionDetail>
{
    public void Configure(EntityTypeBuilder<TransactionDetail> builder)
    {
        builder.HasOne(x => x.Transaction)
            .WithMany(x => x.TransactionDetails)
            .HasForeignKey(x => x.TransactionId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
