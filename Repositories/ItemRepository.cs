using DomainModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Repositories.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ItemRepository> _logger;

        public ItemRepository(ApplicationDbContext dbContext, ILogger<ItemRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
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
            var record = await _dbContext.Items.FindAsync(id);
            if (record == null)
                _logger.LogWarning("Item with ID = {itemId} not found!", id);

            return record;
        }

        public async Task<IList<Item>> GetAll()
        {
            return await _dbContext.Items.ToListAsync();
        }

        public async Task<Item> GetByName(string name)
        {
            return await _dbContext.Items.FirstOrDefaultAsync(i => i.Name.ToUpper().Equals(name.ToUpper()));
        }

        public async Task<Item> Save(Item item)
        {
            if (item == null)
                return null;

            if (item.Id != null)
            {
                var toUpdate = await Get((int)item.Id);
                if (toUpdate == null)
                    return null;

                var entry = _dbContext.Entry(toUpdate);
                entry.CurrentValues.SetValues(item);
                await _dbContext.SaveChangesAsync();
                return entry.Entity;
            }

            var record = _dbContext.Items.Add(item);
            await _dbContext.SaveChangesAsync();
            
            return record.Entity;
        }
    }
}
