using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Queries.Dtos;
using Ordering.Domain.Entities;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, IReadOnlyList<OrderToReturnDto>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<IReadOnlyList<OrderToReturnDto>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var ordersList  = await _orderRepository.GetOrdersForUserAsync(request.BuyerEmail);
            return _mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderToReturnDto>>(ordersList);
        }

    }
}
