using DomainModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public interface IOrderRepository
    {
        Task<IList<Order>> GetAll();

        Task<IList<Order>> GetAllForUser(string userId);

        Task<Order> Get(int id);

        Task<Order> Save(Order order);

        Task<Order> Delete(int id);
    }
}
