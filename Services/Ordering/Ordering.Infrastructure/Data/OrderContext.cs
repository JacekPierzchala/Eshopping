﻿using Microsoft.EntityFrameworkCore;
using Ordering.Core.Common;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data;
public class OrderContext:DbContext
{
    public OrderContext(DbContextOptions<OrderContext> options)
        :base(options)
    {
        
    }

    public DbSet<Order> Orders { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<EntityBase>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedDate = DateTime.Now;
                    entry.Entity.CreatedBy = "rahul"; //TODO: This will be replaced Identity Server
                    break;
                case EntityState.Modified:
                    entry.Entity.LastModifiedDate = DateTime.Now;
                    entry.Entity.LastModifiedBy = "rahul"; //TODO: This will be replaced Identity Server
                    break;
            }
        }
        return await base.SaveChangesAsync(cancellationToken);
    }
}
