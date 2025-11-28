using AutoMapper;
using ECommerce.OrderManagement.Application.DTOs;
using ECommerce.OrderManagement.Domain.Entities;
using ECommerce.OrderManagement.Domain.ValueObjects;

namespace ECommerce.OrderManagement.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>().ReverseMap();
            CreateMap<Order, OrderDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<OrderItem, OrderItemDTO>().ReverseMap();
            CreateMap<Address, AddressDTO>().ReverseMap();
            CreateMap<Customer, CustomerDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value));

            CreateMap<CustomerDTO, Customer>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => new Email(src.Email)));

            CreateMap<Order, OrderDTO>()
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => src.SubTotal.Value));

            CreateMap<OrderDTO, Order>()
                .ForMember(dest => dest.SubTotal, opt => opt.MapFrom(src => new Money(src.SubTotal)));
        }
    }
}
