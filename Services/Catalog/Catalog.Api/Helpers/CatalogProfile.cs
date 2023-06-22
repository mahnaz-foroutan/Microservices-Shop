using AutoMapper;
using Catalog.Api.Dtos;
using Catalog.Core.Entities;

namespace Catalog.Api.Helpers
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.ProductBrand, o => o.MapFrom(s => s.ProductBrandId))
                .ForMember(d => d.ProductType, o => o.MapFrom(s => s.ProductTypeId))
                .ForMember(d => d.PictureUrl, o => o.MapFrom<ProductUrlResolver>());

        }
    }
}
