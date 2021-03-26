using DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ApplicationDbContext dbContext, ILogger<OrderRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<Order> Delete(int id)
        {
            var toDelete = await _dbContext.Orders.FindAsync(id);
            if (toDelete != null)
                _dbContext.Orders.Remove(toDelete);
            await _dbContext.SaveChangesAsync();
            return toDelete;
        }

        public async Task<Order> Get(int id)
        {
            var record = await _dbContext.Orders.FindAsync(id);
            if (record == null)
                _logger.LogWarning("Order with ID = {id} not found!", id);
            return record;
        }

        public async Task<IList<Order>> GetAll()
        {
            return await _dbContext.Orders.ToListAsync();
        }

        public async Task<IList<Order>> GetAllForUser(string userId)
        {
            return await _dbContext.Orders.Where(o => o.CreatedBy.Equals(userId)).ToListAsync();
        }

        public async Task<Order> Save(Order order)
        {
            if (order == null)
                return null;

            if (order.Id != null)
            {
                var toUpdate = await _dbContext.Orders.FindAsync(order.Id);
                if (toUpdate == null)
                    return null;

                var entry = _dbContext.Entry(toUpdate);
                entry.CurrentValues.SetValues(order);
                await _dbContext.SaveChangesAsync();
                return entry.Entity;
            }

            var record = _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();
            
            return record.Entity;
        }
    }
}
