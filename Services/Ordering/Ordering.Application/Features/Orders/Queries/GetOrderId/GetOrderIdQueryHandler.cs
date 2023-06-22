using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Queries.Dtos;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Queries.GetOrderId
{
    public class GetOrderIdQueryHandler : IRequestHandler<GetOrderIdQuery, OrderToReturnDto>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrderIdQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<OrderToReturnDto> Handle(GetOrderIdQuery request, CancellationToken cancellationToken)
        {
            var ordersList = await _orderRepository.GetOrderByIdAsync(request.Id, request.BuyerEmail);

            return _mapper.Map<OrderToReturnDto>(ordersList);
        }

    }
}
