using Core.Models.Order;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShipToAddress, a =>
            {
                a.WithOwner();
            });
            builder.Navigation(a => a.ShipToAddress).IsRequired();
            builder.Property(status => status.Status)
                .HasConversion(
                    os => os.ToString(),
                    os => (OrderStatus)Enum.Parse(typeof(OrderStatus), os));

            builder.HasMany(o => o.OrderProducts).WithOne().OnDelete(DeleteBehavior.Cascade);
        }
    }
}
