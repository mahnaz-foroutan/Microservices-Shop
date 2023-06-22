using System.Collections.Generic;
using MediatR;
using Ordering.Application.Features.Orders.Queries.Dtos;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    public class GetOrdersListQuery : IRequest<IReadOnlyList<OrderToReturnDto>>
    {
        public string BuyerEmail { get; set; }

        public GetOrdersListQuery(string buyerEmail)
        {
            BuyerEmail = buyerEmail;
        }
    }
}
