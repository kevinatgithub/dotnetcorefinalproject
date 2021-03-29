using AutoMapper;
using DomainModels;
using Microsoft.Extensions.Logging;
using Repositories;
using Services.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IItemRepository _itemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, IItemRepository itemRepository, IMapper mapper, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _itemRepository = itemRepository ?? throw new ArgumentNullException(nameof(itemRepository)); ;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper)); ;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger)); ;
        }

        public async Task<decimal> CalculateTotalPrice(OrderDTO orderDTO)
        {
            var item = await _itemRepository.Get(orderDTO.ItemId);
            if (item == null)
                return 0;

            return item.UnitPrice * orderDTO.Quantity;
        }

        private async Task AdjustItemStocks(int itemId, int quantity)
        {
            var item = await _itemRepository.Get(itemId);
            if (item == null)
                return;

            item.Stocks -= quantity;

            await _itemRepository.Save(item);
            _logger.LogInformation("Item with ID = {id} stocks set to = {s}", itemId, quantity);
        }

        public async Task<OrderDTO> Create(OrderDTO orderDTO)
        {
            var order = _mapper.Map<OrderDTO, Order>(orderDTO);
            order.Id = null;
            order.CreatedOn = DateTime.UtcNow;
            order.TotalPrice = await CalculateTotalPrice(orderDTO);
            order.Status = OrderStatus.PLACED.ToString();
            var created = _mapper.Map<Order, OrderDTO>(await _orderRepository.Save(order));
            await AdjustItemStocks(order.ItemId, order.Quantity);
            return created;
        }

        public async Task<OrderDTO> Delete(int id)
        {
            return _mapper.Map<Order, OrderDTO>(await _orderRepository.Delete(id));
        }

        public async Task<OrderDTO> Get(int id)
        {
            return _mapper.Map<Order, OrderDTO>(await _orderRepository.Get(id));
        }

        public async Task<IList<OrderDTO>> GetAll()
        {
            return _mapper.Map<IList<Order>, IList<OrderDTO>>(await _orderRepository.GetAll());
        }

        public async Task<ItemDTO> GetItemOfOrder(OrderDTO orderDTO)
        {
            return _mapper.Map<Item, ItemDTO>(await _itemRepository.Get(orderDTO.ItemId));
        }

        public async Task<OrderDTO> Update(OrderDTO orderDTO)
        {
            var oldRecord = await Get((int)orderDTO.Id);
            if (oldRecord == null)
                return null;

            orderDTO.CreatedBy = oldRecord.CreatedBy;
            orderDTO.CreatedOn = oldRecord.CreatedOn;

            var item = await _itemRepository.Get(orderDTO.ItemId);
            if (orderDTO.ItemId != oldRecord.ItemId || orderDTO.Quantity != oldRecord.Quantity)
            {
                if (item == null)
                    return null;
                orderDTO.TotalPrice = item.UnitPrice * orderDTO.Quantity;
            }

            var order = _mapper.Map<OrderDTO, Order>(orderDTO);
            var updated = _mapper.Map<Order, OrderDTO>(await _orderRepository.Save(order));

            //var item2 = await _itemRepository.Get(updated.ItemId);
            if (orderDTO.ItemId == oldRecord.ItemId && orderDTO.Quantity != oldRecord.Quantity)
            {
                _logger.LogInformation("Order Quantity changed, adjusting item stocks!");
                await AdjustItemStocks(orderDTO.ItemId, orderDTO.Quantity - oldRecord.Quantity);
            }
            else if (orderDTO.ItemId != oldRecord.ItemId)
            {
                _logger.LogInformation("Order Item ID changed, adjusting item stocks!");
                await AdjustItemStocks(oldRecord.ItemId, oldRecord.Quantity * -1);
                await AdjustItemStocks(orderDTO.ItemId, orderDTO.Quantity);
            }

            return updated;
        }

        public async Task<IList<OrderDTO>> GetAllForUser(string userId)
        {
            var orders = await _orderRepository.GetAllForUser(userId);

            return _mapper.Map<IList<Order>, IList<OrderDTO>>(orders);
        }
    }
}
