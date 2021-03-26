using Services.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IItemService
    {
        Task<IList<ItemDTO>> GetAll();
        Task<ItemDTO> GetItemByName(string name);
        Task<ItemDTO> Get(int id);
        Task<ItemDTO> Create(ItemDTO itemDto);
        Task<ItemDTO> Update(ItemDTO itemDto);
        Task<ItemDTO> Delete(int id);
    }
}
