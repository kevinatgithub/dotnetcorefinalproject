using AutoMapper;
using FinalProject.ApiModels;
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
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, IMapper mapper, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAll();
            _logger.LogInformation("GetAll returned {n} result", orders.Count);
            return Ok(orders);
        }

        [HttpGet]
        [Route("{orderId}")]
        public async Task<IActionResult> Get(int orderId)
        {
            var order = await _orderService.Get(orderId);
            if (order == null)
            {
                _logger.LogWarning("Order with orderId ID = {orderId} not found!", orderId);
                return NotFound("Order not found!");
            }

            _logger.LogInformation("Order was found with Order ID = {orderId}", orderId);
            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderModel createOrderModel)
        {
            var orderDTO = _mapper.Map<CreateOrderModel, OrderDTO>(createOrderModel);
            _logger.LogInformation("Creating order with itemId = {itemId}, quantity = {quantity}", createOrderModel.ItemId, createOrderModel.Quantity);
            var record = await _orderService.Create(orderDTO);
            _logger.LogInformation("Order created succesfully with Order Id = {orderId}", record.Id);
            return Ok(record);
        }

        [HttpPut]
        [Route("{orderId}")]
        public async Task<IActionResult> Update(int orderId, [FromBody] UpdateOrderModel updateModel)
        {
            _logger.LogInformation("Updating Order with ID = {i} applying changes itemId={itemId}, quantity={q}", orderId, updateModel.ItemId, updateModel.Quantity);
            var item = await _orderService.Get(orderId);
            if (item == null)
            {
                _logger.LogWarning("Update Order failed!, Order with ID = {id} not found!", orderId);
                return NotFound("Order not found!");
            }

            var orderDto = _mapper.Map<UpdateOrderModel, OrderDTO>(updateModel);
            orderDto.Id = orderId;
            var record = await _orderService.Update(orderDto);
            if (record == null)
                return NotFound("Item not found!");

            _logger.LogInformation("Order with Id = {id} has been updated succesfully!", record.Id);
            return Ok(record);
        }

        [HttpDelete]
        [Route("{orderId}")]
        public async Task<IActionResult> Delete(int orderId)
        {
            var order = await _orderService.Delete(orderId);
            _logger.LogInformation("Order with Id = {id} has been deleted!", orderId);
            return Ok(order);
        }
    }
}
