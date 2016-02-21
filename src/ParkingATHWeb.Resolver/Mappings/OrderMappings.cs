﻿using AutoMapper;
using ParkingATHWeb.Contracts.DTO.Order;
using ParkingATHWeb.Model.Concrete;
using ParkingATHWeb.Shared.Enums;
using ParkingATHWeb.Shared.Helpers;

namespace ParkingATHWeb.Resolver.Mappings
{
    public class OrderBackendMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<OrderBaseDto, Order>()
             .ForMember(x => x.OrderState, opt => opt.UseValue(OrderStatus.Pending))
             .IgnoreNotExistingProperties();

            CreateMap<Order, OrderAdminDto>()
                .ForMember(x => x.LastName, opt => opt.MapFrom(k => k.User.LastName))
                .ForMember(x => x.Name, opt => opt.MapFrom(k => k.User.Name))
                .IgnoreNotExistingProperties();

            CreateMap<Order, OrderBaseDto>()
                .IgnoreNotExistingProperties();
        }
    }
}
