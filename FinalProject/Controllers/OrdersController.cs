using AutoMapper;
using FinalProject.ApiModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services;
using Services.DTO;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FinalProject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly UserManager<IdentityUser> _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, UserManager<IdentityUser> userService, IMapper mapper, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// Get All Orders
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all orders from the database</response>
        /// <response code="401">Request unauthorized</response>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var orders = await _orderService.GetAll();
            _logger.LogInformation("GetAll returned {n} result", orders.Count);
            return Ok(orders);
        }

        /// <summary>
        /// Get All Orders for the current User
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns all orders created by the current user</response>
        /// <response code="401">Request unauthorized</response>
        [HttpGet]
        [Route("self")]
        public async Task<IActionResult> GetAllForUser()
        {
            var email = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email);
            var user = await _userService.FindByNameAsync(email.Value);
            var orders = await _orderService.GetAllForUser(user.Id);
            _logger.LogInformation("GetAllForUser returned {n} result", orders.Count);
            return Ok(orders);
        }

        /// <summary>
        /// Get an Specific Order by ID
        /// </summary>
        /// <param name="orderId">ID of the Order</param>
        /// <returns></returns>
        /// <response code="200">Returns the specific Order record</response>
        /// <response code="401">Request unauthorized</response>
        /// <response code="404">Order record not found</response>
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

        /// <summary>
        /// Endpoint for creating Order
        /// </summary>
        /// <param name="createOrderModel">Contains the Item ID and Quantity</param>
        /// <returns></returns>
        /// <response code="200">Order creatd succefully</response>
        /// <response code="404">Item with the specified ID was not found!</response>
        /// <response code="400">Invalid request payload</response>
        /// <response code="401">Request unauthorized</response>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrderModel createOrderModel)
        {
            var orderDTO = _mapper.Map<CreateOrderModel, OrderDTO>(createOrderModel);
            var email = ((ClaimsIdentity)User.Identity).FindFirst(ClaimTypes.Email);
            var user = await _userService.FindByNameAsync(email.Value);
            orderDTO.CreatedBy = Guid.Parse(user.Id).ToString();
            _logger.LogInformation("Creating order with itemId = {itemId}, quantity = {quantity}", createOrderModel.ItemId, createOrderModel.Quantity);
            var record = await _orderService.Create(orderDTO);
            _logger.LogInformation("Order created succesfully with Order Id = {orderId}", record.Id);
            return Ok(record);
        }

        /// <summary>
        /// For updating an order record
        /// </summary>
        /// <param name="orderId">The ID of the Order to be updated</param>
        /// <param name="updateModel">contains new Item ID or new Quantity or new Status of the order</param>
        /// <returns></returns>
        /// <response code="200">Order was succefully updated</response>
        /// <response code="401">Request unauthorized</response>
        /// <response code="404">Item with the specified ID was not found!</response>
        /// <response code="400">Invalid request payload</response>
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

        /// <summary>
        /// For deleting an order
        /// </summary>
        /// <param name="orderId">The ID of the order to be deleted</param>
        /// <returns></returns>
        /// <response code="200">Order has been deleted succefully</response>
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
