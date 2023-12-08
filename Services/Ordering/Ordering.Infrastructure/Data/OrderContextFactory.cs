using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Ordering.Infrastructure.Data;
internal class OrderContextFactory : IDesignTimeDbContextFactory<OrderContext>
{
    public OrderContext CreateDbContext(string[] args)
    {
        var optionbuilder= new DbContextOptionsBuilder<OrderContext>();
        optionbuilder.UseSqlServer("Data Source=OrderDb");
        return new OrderContext(optionbuilder.Options);
    }
}
