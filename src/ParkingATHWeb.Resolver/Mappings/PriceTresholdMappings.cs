﻿using AutoMapper;
using ParkingATHWeb.Contracts.DTO.PriceTreshold;
using ParkingATHWeb.Model.Concrete;
using ParkingATHWeb.Shared.Helpers;

namespace ParkingATHWeb.Resolver.Mappings
{
    public class PriceTresholdBackendMappings : Profile
    {
        protected override void Configure()
        {
            CreateMap<PriceTreshold, PriceTresholdBaseDto>().IgnoreNotExistingProperties();

            CreateMap<PriceTreshold, PriceTresholdAdminDto>()
                .ForMember(x => x.NumOfOrders, opt => opt.MapFrom(k => k.Orders.Count))
                .IgnoreNotExistingProperties();

            CreateMap<PriceTresholdBaseDto, PriceTreshold>().IgnoreNotExistingProperties();
        }
    }
}
