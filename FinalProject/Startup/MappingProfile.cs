using AutoMapper;
using DomainModels;
using FinalProject.ApiModels;
using Services.DTO;

namespace FinalProject
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Item, ItemDTO>();
            CreateMap<ItemDTO, Item>();

            CreateMap<ItemModel, ItemDTO>();

            CreateMap<Order, OrderDTO>();
            CreateMap<OrderDTO, Order>();

            CreateMap<CreateOrderModel, OrderDTO>();
            CreateMap<UpdateOrderModel, OrderDTO>();
        }
    }
}
