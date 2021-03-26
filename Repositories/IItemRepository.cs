using DomainModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IItemRepository
    {
        Task<IList<Item>> GetAll();

        Task<Item> GetByName(string name);

        Task<Item> Get(int id);

        Task<Item> Save(Item item);

        Task<Item> Delete(int id);
    }
}
