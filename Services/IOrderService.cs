﻿using Services.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public interface IOrderService
    {
        Task<IList<OrderDTO>> GetAll();
        Task<IList<OrderDTO>> GetAllForUser(string userId);
        Task<OrderDTO> Get(int id);
        Task<OrderDTO> Create(OrderDTO orderDTO);
        Task<OrderDTO> Update(OrderDTO orderDTO);
        Task<OrderDTO> Delete(int id);
        Task<ItemDTO> GetItemOfOrder(OrderDTO orderDTO);
        Task<decimal> CalculateTotalPrice(OrderDTO orderDTO);
    }
}
