using AutoMapper;
using Week01_EFCore.DTOs;
using Week01_EFCore.Entities;

namespace Week01_EFCore.Configs
{
    public class MappingConfigs : Profile
    {
        public MappingConfigs()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
        }
    }
}
