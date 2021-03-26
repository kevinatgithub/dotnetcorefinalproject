using AutoMapper;
using FinalProject.ApiModels;
using FinalProject.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.DTO;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
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

        /// <summary>
        /// Get all items from the database.
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Succeful request, returns the items from the database as a collection</response>
        /// <response code="401">Request unauthorized</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var items = await _itemService.GetAll();
            _logger.LogInformation("GetAll returned {n} result",items.Count);
            return Ok(items);
        }

        /// <summary>
        /// Get an specific item using Item ID
        /// </summary>
        /// <param name="itemId">The Item's ID</param>
        /// <returns></returns>
        /// <response code="200">Item was found!</response>
        /// <response code="401">Request unauthorized</response>
        /// <response code="404">Item does not exist!</response>
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

        /// <summary>
        /// For creating a new item record
        /// </summary>
        /// <param name="createItemModel">The Item details</param>
        /// <returns></returns>
        /// <response code="200">The request was succeful, returns the item created with the ID</response>
        /// <response code="400">The request failed due to the item name already existing or invalid request payload.</response>
        /// <response code="401">Request unauthorized</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ItemModel createItemModel)
        {
            var itemDto = _mapper.Map<ItemModel, ItemDTO>(createItemModel);
            _logger.LogInformation("Creating Item with name = {n}, stocks = {s}, unit price = {p}", itemDto.Name, itemDto.Stocks, itemDto.UnitPrice);
            var record = await _itemService.Create(itemDto);
            if (record == null)
            {
                _logger.LogWarning("Failed to create item, item name already exist!");
                return BadRequest("Failed to create item, item name already exist!");
            }

            _logger.LogInformation("Item created succesfully with Item Id = {itemId}", record.Id);
            return Ok(record);
        }

        /// <summary>
        /// For updating an existing item
        /// </summary>
        /// <param name="itemId">The ID of the item to be updated, [ItemExistActionFilter]</param>
        /// <param name="updateItemModel">contains update for Item Name, Stocks, and Unit Price</param>
        /// <returns></returns>
        /// <response code="200">Item updated succesfully</response>
        /// <response code="400">Failed to update item</response>
        /// <response code="401">Request unauthorized</response>
        /// <response code="404">Item with the provided ID does not exist</response>
        [HttpPut]
        [Route("{itemId}")]
        [ItemExistActionFilter]
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

        /// <summary>
        /// Delete an Item by ID
        /// </summary>
        /// <param name="itemId">The ID of the Item to be deleted, [ItemExistActionFilter]</param>
        /// <returns></returns>
        /// <response code="200">Item have been deleted succefully</response>
        /// <response code="401">Request unauthorized</response>
        [HttpDelete]
        [Route("{itemId}")]
        [ItemExistActionFilter]
        public async Task<IActionResult> Delete(int itemId)
        {
            var item = await _itemService.Delete(itemId);
            _logger.LogInformation("Item with Item Id = {itemId} has been deleted!", itemId);
            return Ok(item);
        }
    }
}
