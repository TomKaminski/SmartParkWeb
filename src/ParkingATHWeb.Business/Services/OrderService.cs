﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Data.Entity;
using ParkingATHWeb.Business.Services.Base;
using ParkingATHWeb.Contracts.Common;
using ParkingATHWeb.Contracts.DTO.Order;
using ParkingATHWeb.Contracts.DTO.Payments;
using ParkingATHWeb.Contracts.Services;
using ParkingATHWeb.DataAccess.Common;
using ParkingATHWeb.DataAccess.Interfaces;
using ParkingATHWeb.Model.Concrete;
using ParkingATHWeb.Shared.Enums;
using ParkingATHWeb.Shared.Helpers;

namespace ParkingATHWeb.Business.Services
{
    public class OrderService : EntityService<OrderBaseDto, Order, long>, IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public OrderService(IUnitOfWork unitOfWork, IOrderRepository repository, IUserRepository userRepository, IMapper mapper)
            : base(repository, unitOfWork, mapper)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<ServiceResult<OrderBaseDto>> GetAsync(Guid externalorderId)
        {
            var obj = await _repository.FirstOrDefaultAsync(x => x.ExtOrderId == externalorderId);
            return ServiceResult<OrderBaseDto>.Success(_mapper.Map<Order, OrderBaseDto>(obj));
        }

        public async Task<ServiceResult<Guid>> GenerateExternalOrderIdAsync()
        {
            var guidsTaken = (await _repository.GetAllAsync()).Select(x => x.ExtOrderId).ToList();
            while (true)
            {
                var guid = Guid.NewGuid();
                if (!guidsTaken.Contains(guid))
                    return ServiceResult<Guid>.Success(guid);
            }
        }

        public ServiceResult<IEnumerable<OrderAdminDto>> GetAllAdmin()
        {
            return ServiceResult<IEnumerable<OrderAdminDto>>
                .Success(_repository.GetAll()
                .Select(_mapper.Map<OrderAdminDto>));
        }

        public ServiceResult<IEnumerable<OrderAdminDto>> GetAllAdmin(Expression<Func<OrderAdminDto, bool>> predicate)
        {
            var param = Expression.Parameter(typeof(Order));
            var result = new CustomExpressionVisitor<Order>(param).Visit(predicate.Body);
            var lambda = Expression.Lambda<Func<Order, bool>>(result, param);
            return ServiceResult<IEnumerable<OrderAdminDto>>
                .Success(_repository.Include(x => x.User).Where(lambda)
                .Select(_mapper.Map<OrderAdminDto>));
        }

        public async Task<ServiceResult<IEnumerable<OrderAdminDto>>> GetAllAdminAsync()
        {
            return ServiceResult<IEnumerable<OrderAdminDto>>
                .Success((await _repository.GetAllAsync()).Select(_mapper.Map<OrderAdminDto>));
        }

        public async Task<ServiceResult<IEnumerable<OrderAdminDto>>> GetAllAdminAsync(Expression<Func<OrderAdminDto, bool>> predicate)
        {
            var param = Expression.Parameter(typeof(Order));
            var result = new CustomExpressionVisitor<Order>(param).Visit(predicate.Body);
            var lambda = Expression.Lambda<Func<Order, bool>>(result, param);
            return ServiceResult<IEnumerable<OrderAdminDto>>
                .Success(
                    (await _repository.Include(x => x.User).Where(lambda).OrderByDescending(x => x.Date).ToListAsync()).Select(
                        _mapper.Map<OrderAdminDto>));
        }

        public async Task<ServiceResult<OrderStatus>> UpdateOrderState(PaymentNotification model)
        {
            var order = await _repository.FirstOrDefaultAsync(x => x.ExtOrderId == new Guid(model.order.extOrderId));
            if (order!= null && order.OrderState != OrderStatus.Canceled && order.OrderState != OrderStatus.Completed)
            {
                switch (model.order.status)
                {
                    case "COMPLETED":
                        {
                            order.OrderState = OrderStatus.Completed;
                            _repository.Edit(order);
                            var entity = await _userRepository.FirstOrDefaultAsync(x => x.Id == order.UserId);
                            entity.Charges += order.NumOfCharges;
                            _userRepository.Edit(entity);
                            await _unitOfWork.CommitAsync();
                            return ServiceResult<OrderStatus>.Success(OrderStatus.Completed);
                        }

                    case "CANCELED":
                        {
                            order.OrderState = OrderStatus.Canceled;
                            _repository.Edit(order);
                            await _unitOfWork.CommitAsync();
                            return ServiceResult<OrderStatus>.Success(OrderStatus.Canceled);
                        }
                    case "REJECTED":
                        {
                            order.OrderState = OrderStatus.Rejected;
                            _repository.Edit(order);
                            await _unitOfWork.CommitAsync();
                            return ServiceResult<OrderStatus>.Success(OrderStatus.Rejected);
                        }
                }
            }
               
            return ServiceResult<OrderStatus>.Success(OrderStatus.Pending);
        }
    }
}
