using Discount.Grpc.Protos;
using System.Threading.Tasks;

namespace Basket.Api.GrpcServices
{
    public class DiscountGrpcService
    {
        #region constructor

        private readonly DiscountProtoService.DiscountProtoServiceClient _discountProtoService;
        
        public DiscountGrpcService(DiscountProtoService.DiscountProtoServiceClient discountProtoService)
        {
            _discountProtoService = discountProtoService;
        }

        #endregion

        #region get discount

        public async Task<CouponModel> GetDiscount(string productId)
        {
            var discountRequest = new GetDiscountRequest { ProductId = productId };

            return await _discountProtoService.GetDiscountAsync(discountRequest);
        }

        #endregion
    }
}
