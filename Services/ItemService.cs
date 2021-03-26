using AutoMapper;
using DomainModels;
using Repositories;
using Services.DTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class ItemService : IItemService
    {
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;

        public ItemService(IItemRepository itemRepository, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _mapper = mapper;
        }

        public async Task<ItemDTO> Create(ItemDTO itemDto)
        {
            var item = _mapper.Map<ItemDTO, Item>(itemDto);
            item.Id = null;
            var exist = await _itemRepository.GetByName(item.Name);
            if (exist != null)
                return null;
            return _mapper.Map<Item, ItemDTO>(await _itemRepository.Save(item));
        }

        public async Task<ItemDTO> Delete(int id)
        {
            return _mapper.Map<Item, ItemDTO>(await _itemRepository.Delete(id));
        }

        public async Task<ItemDTO> Get(int id)
        {
            return _mapper.Map<Item, ItemDTO>(await _itemRepository.Get(id));
        }

        public async Task<IList<ItemDTO>> GetAll()
        {
            return _mapper.Map<IList<Item>, IList<ItemDTO>>(await _itemRepository.GetAll());
        }

        public async Task<ItemDTO> GetItemByName(string name)
        {
            return _mapper.Map<Item, ItemDTO>(await _itemRepository.GetByName(name));
        }

        public async Task<ItemDTO> Update(ItemDTO itemDto)
        {
            var item = _mapper.Map<ItemDTO, Item>(itemDto);
            return _mapper.Map<Item, ItemDTO>(await _itemRepository.Save(item));
        }
    }
}
