﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;
using ParkingATHWeb.ApiModels.Base;
using ParkingATHWeb.Areas.Portal.Controllers.Base;
using ParkingATHWeb.Areas.Portal.ViewModels.PriceTreshold;
using ParkingATHWeb.Areas.Portal.ViewModels.User;
using ParkingATHWeb.Contracts.Services;
using ParkingATHWeb.Infrastructure.Attributes;

namespace ParkingATHWeb.Areas.Portal.Controllers
{
    [Area("Portal")]
    [Route("[area]/Shop")]
    [Authorize]
    public class ShopController : BaseController
    {
        private readonly IPriceTresholdService _priceTresholdService;
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;

        public ShopController(IPriceTresholdService priceTresholdService, IMapper mapper, IOrderService orderService)
        {
            _priceTresholdService = priceTresholdService;
            _mapper = mapper;
            _orderService = orderService;
        }

        [Route("")]
        public IActionResult Index()
        {
            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryTokenFromHeader]
        [Route("[action]")]
        public async Task<IActionResult> GetPrices()
        {
            var currentPricesResult = await _priceTresholdService.GetAllAsync();
            if (currentPricesResult.IsValid)
            {
                var pricesViewModels = currentPricesResult.Result.Select(_mapper.Map<PriceTresholdShopItemViewModel>).ToList();
                var defaultPrice = pricesViewModels.First();
                defaultPrice.IsDeafult = true;

                for (var i = 1; i < pricesViewModels.Count; i++)
                {
                    var price = pricesViewModels[i];
                    price.PercentDiscount = 100 - Convert.ToInt32((price.PricePerCharge * 100) / defaultPrice.PricePerCharge);
                }

                return Json(SmartJsonResult<IEnumerable<PriceTresholdShopItemViewModel>>.Success(pricesViewModels));
            }
            return Json(SmartJsonResult<IEnumerable<PriceTresholdShopItemViewModel>>.Failure(currentPricesResult.ValidationErrors));
        }

        [HttpPost]
        [ValidateAntiForgeryTokenFromHeader]
        [Route("[action]")]
        public async Task<IActionResult> GetUserOrders()
        {;
            var userId = CurrentUser.UserId.Value;
            var userOrdersResult = await _orderService.GetAllAsync(x => x.UserId == userId);
            if (userOrdersResult.IsValid)
            {
                var viewModel = userOrdersResult.Result.Take(5).Select(_mapper.Map<ShopOrderItemViewModel>);
                return Json(SmartJsonResult<IEnumerable<ShopOrderItemViewModel>>.Success(viewModel));
            }
            return Json(SmartJsonResult<IEnumerable<ShopOrderItemViewModel>>.Failure(userOrdersResult.ValidationErrors));
        }
    }
}
