using Microsoft.EntityFrameworkCore;
using Ordering.Core.Entities;
using Ordering.Core.Repositories;
using Ordering.Infrastructure.Data;

namespace Ordering.Infrastructure.Repositories;
internal class OrderRepository : RepositoryBase<Order>, IOrderRepository
{
    public OrderRepository(OrderContext orderContext) : base(orderContext)
    {}

    public async Task<IEnumerable<Order>> GetOrdersByUserName(string userName)
    {
        var orderList = await _orderContext.Orders.Where(c => c.UserName == userName).ToListAsync();
        return orderList;
    }
}
