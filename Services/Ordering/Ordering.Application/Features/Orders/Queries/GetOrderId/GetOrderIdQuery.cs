using System.Collections.Generic;
using MediatR;
using Ordering.Application.Features.Orders.Queries.Dtos;

namespace Ordering.Application.Features.Orders.Queries.GetOrderId
{
    public class GetOrderIdQuery : IRequest<OrderToReturnDto>
    {
        public int Id { get; set; }
        public string BuyerEmail { get; set; }

        public GetOrderIdQuery(int id,string buyerEmail)
        {
            Id = id;
            BuyerEmail = buyerEmail;
        }
    }
}
