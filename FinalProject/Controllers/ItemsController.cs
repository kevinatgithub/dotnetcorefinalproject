using AutoMapper;
using FinalProject.ApiModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.DTO;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly IItemService _itemService;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(IItemService itemService, IMapper mapper, ILogger<ItemsController> logger)
        {
            _itemService = itemService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAll();
            _logger.LogInformation("GetAll returned {n} result",items.Count);
            return Ok(items);
        }

        [HttpGet]
        [Route("{itemId}")]
        public async Task<IActionResult> Get(int itemId)
        {
            var item = await _itemService.Get(itemId);
            if (item == null)
            {
                _logger.LogWarning("Item with Item ID = {itemId} no found!", itemId);
                return NotFound("Item not found!");
            }

            _logger.LogInformation("Item was found with Item ID = {itemId}", itemId);
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemModel createItemModel)
        {
            var itemDto = _mapper.Map<ItemModel, ItemDTO>(createItemModel);
            _logger.LogInformation("Creating Item with name = {n}, stocks = {s}, unit price = {p}", itemDto.Name, itemDto.Stocks, itemDto.UnitPrice);
            var record = await _itemService.Create(itemDto);
            _logger.LogInformation("Item created succesfully with Item Id = {itemId}", record.Id);
            return Ok(record);
        }

        [HttpPut]
        [Route("{itemId}")]
        public async Task<IActionResult> Update(int itemId, [FromBody] ItemModel updateItemModel)
        {
            _logger.LogInformation("Updating Item with Item ID = {i} applying changes name = {n}, stocks = {s}, unit price = {p}", itemId, updateItemModel.Name, updateItemModel.Stocks, updateItemModel.UnitPrice);
            var item = await _itemService.Get(itemId);
            if (item == null)
            {
                _logger.LogWarning("Update item failed!, Item with Item ID = {itemId} not found!", itemId);
                return NotFound("Item not found!");
            }

            var itemDto = _mapper.Map<ItemModel, ItemDTO>(updateItemModel);
            itemDto.Id = itemId;
            var record = await _itemService.Update(itemDto);
            _logger.LogInformation("Item with Item Id = {itemId} has been updated succesfully!", record.Id);
            return Ok(record);
        }

        [HttpDelete]
        [Route("{itemId}")]
        public async Task<IActionResult> Delete(int itemId)
        {
            var item = await _itemService.Delete(itemId);
            _logger.LogInformation("Item with Item Id = {itemId} has been deleted!", itemId);
            return Ok(item);
        }
    }
}
