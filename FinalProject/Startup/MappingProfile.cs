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

            CreateMap<ItemDTO, ItemModel>();
            CreateMap<ItemModel, ItemDTO>();
        }
    }
}
