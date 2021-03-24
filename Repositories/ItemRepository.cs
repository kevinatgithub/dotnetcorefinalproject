using DomainModels;
using Microsoft.EntityFrameworkCore;
using Repositories.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ItemRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Item> Delete(int id)
        {
            var toDelete = await _dbContext.Items.FindAsync(id);
            if (toDelete != null)
                _dbContext.Items.Remove(toDelete);
            await _dbContext.SaveChangesAsync();
            return toDelete;
        }

        public async Task<Item> Get(int id)
        {
            return await _dbContext.Items.FindAsync(id);
        }

        public async Task<IList<Item>> GetAll()
        {
            return await _dbContext.Items.ToListAsync();
        }

        public async Task<Item> Save(Item item)
        {
            if (item == null)
                return null;

            if (item.Id != null)
            {
                var toUpdate = await _dbContext.Items.FindAsync(item.Id);
                if (toUpdate == null)
                    return null;

                toUpdate.Name = item.Name;
                toUpdate.Stocks = item.Stocks;
                toUpdate.UnitPrice = item.UnitPrice;
                var updated = _dbContext.Items.Update(toUpdate);
                await _dbContext.SaveChangesAsync();
                return updated.Entity;
            }

            var record = _dbContext.Items.Add(item);
            await _dbContext.SaveChangesAsync();
            return record.Entity;
        }
    }
}
