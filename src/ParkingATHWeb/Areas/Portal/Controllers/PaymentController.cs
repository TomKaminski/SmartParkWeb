﻿using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Http.Features;
using Microsoft.AspNet.Mvc;
using ParkingATHWeb.ApiModels.Base;
using ParkingATHWeb.Areas.Portal.Controllers.Base;
using ParkingATHWeb.Areas.Portal.ViewModels.Payment;
using ParkingATHWeb.Contracts.DTO.Payments;
using ParkingATHWeb.Contracts.Services;
using ParkingATHWeb.Contracts.Services.Payments;
using ParkingATHWeb.Infrastructure.Attributes;
using ParkingATHWeb.Shared.Enums;

namespace ParkingATHWeb.Areas.Portal.Controllers
{
    [Area("Portal")]
    [Route("[area]/Payment")]
    [Authorize]
    public class PaymentController : BaseController
    {
        private readonly IPayuService _payuService;
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        public PaymentController(IPayuService payuService, IMapper mapper, IOrderService orderService)
        {
            _payuService = payuService;
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        [ValidateAntiForgeryTokenFromHeader]
        [Route("[action]")]
        public async Task<IActionResult> ProcessPayment([FromBody]PaymentRequestViewModel model)
        {
            model.UserId = CurrentUser.UserId.Value;

            //var connection = HttpContext.Features.Get<IHttpConnectionFeature>();
            //model.CustomerIP = connection?.RemoteIpAddress?.ToString();

            model.CustomerIP = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1";
            model.UserEmail = CurrentUser.Email;
            model.UserName = CurrentUser.Name;
            model.UserLastName = CurrentUser.LastName;

            var request = _mapper.Map<PaymentRequest>(model);
            request.notifyUrl = Url.Action("Notify", "Payment" , new { area = "Portal" }, "http");
            request.continueUrl = Url.Action("ShopContinue", "Home", new { area = "Portal" }, "http");
            var payuServiceResult = await _payuService.ProcessPaymentAsync(request, model.UserId, OrderPlace.Website);

            if (payuServiceResult.IsValid)
            {
                return
                    Json(
                        SmartJsonResult<PaymentResponseViewModel>.Success(
                            _mapper.Map<PaymentResponseViewModel>(payuServiceResult.Result)));
            }
            return Json(SmartJsonResult<PaymentResponseViewModel>.Failure(payuServiceResult.ValidationErrors));
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("[action]")]
        public async Task<IActionResult> Notify([FromBody]PayuNotificationModel model)
        {
            await _orderService.UpdateOrderState(_mapper.Map<PaymentNotification>(model));
            return new EmptyResult();
        }
    }
}

