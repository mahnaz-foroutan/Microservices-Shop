using System.Collections.Generic;
using Catalog.Core.Entities;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data
{
    public class CatalogContextSeed
    {
      
        public static void SeedData(IMongoCollection<ProductBrand> productBrandCollection, IMongoCollection<ProductType> productTypeCollection, IMongoCollection<Product> productCollection)
        {
            bool existProductBrand = productBrandCollection.Find(p => true).Any();

            if (!existProductBrand)
            {
                productBrandCollection.InsertManyAsync(GetSeedProductBrand());
            }
            bool existProductType = productTypeCollection.Find(p => true).Any();

            if (!existProductType)
            {
                productTypeCollection.InsertManyAsync(GetSeedProductType());
            }
            //var filter = Builders<Product>.Filter.BitsAllSet(p=> true);
            //Task<DeleteResult> task = productCollection.deleteMany({ })
           
            bool existProduct = productCollection.Find(p => true).Any();

            if (!existProduct)
            {
                productCollection.InsertManyAsync(GetSeedProduct());
            }
        }
        private static IEnumerable<ProductBrand> GetSeedProductBrand()
        {
            return new List<ProductBrand>()
            {
                new ProductBrand()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "Apple"
                },
                new ProductBrand()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Samsung"
                },
                new ProductBrand()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "Huawei"
                },
                new ProductBrand()
                {
                    Id = "602d2149e773f2a3990b47f8",
                    Name = "Xiaomi"
                },
                new ProductBrand()
                {
                    Id = "602d2149e773f2a3990b47f9",
                    Name = "HTC"
                },
                new ProductBrand()
                {
                    Id = "602d2149e773f2a3990b47fa",
                    Name = "LG"
                }
            };
        }
        private static IEnumerable<ProductType> GetSeedProductType()
        {
            return new List<ProductType>()
            {
                new ProductType()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "Smart Phone"
                },
                new ProductType()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "TV"
                },
                new ProductType()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "LapTop"
                },
                new ProductType()
                {
                    Id = "602d2149e773f2a3990b47f8",
                    Name = "Tablet"
                },
                new ProductType()
                {
                    Id = "602d2149e773f2a3990b47f9",
                    Name = "Appliance"
                },
                new ProductType()
                {
                    Id = "602d2149e773f2a3990b47fa",
                    Name = "Gadget"
                }
            };
        }
        private static IEnumerable<Product> GetSeedProduct()
        {
            return new List<Product>()
            {
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f5",
                    Name = "iPhone 13 Pro ZAA",
                   Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "iPhone13ProZAA.jpg",
                    Price = 1950.00M,
                    ProductTypeId = "602d2149e773f2a3990b47f5",
                    ProductBrandId="602d2149e773f2a3990b47f5"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f6",
                    Name = "Galaxy S23 Ultra",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "GalaxyS23Ultra.jpg",
                    Price = 1840.00M,
                    ProductTypeId = "602d2149e773f2a3990b47f5",
                    ProductBrandId="602d2149e773f2a3990b47f6"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f7",
                    Name = "Huawei Y9a FRL-L22",
                   Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "Y9aFRL-L22.jpg",
                    Price = 650.00M,
                    ProductTypeId = "602d2149e773f2a3990b47f5",
                    ProductBrandId="602d2149e773f2a3990b47f7"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f8",
                    Name = "Redmi Note 10 pro",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "RedmiNote10pro.jpg",
                    Price = 470.00M,
                    ProductTypeId = "602d2149e773f2a3990b47f5",
                    ProductBrandId="602d2149e773f2a3990b47f8"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f9",
                    Name = "HTC U11+ Plus",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "boot-ang1.png",
                    Price = 380.00M,
                    ProductTypeId = "602d2149e773f2a3990b47f5",
                    ProductBrandId="602d2149e773f2a3990b47f9"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f1",
                    Name = "LG G2",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "boot-ang1.png",
                    Price = 440.00M,
                    ProductTypeId = "602d2149e773f2a3990b47f5",
                    ProductBrandId="602d2149e773f2a3990b47fa"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f2",
                    Name = "PrintProof® Stainless Steel Smart Counter-Depth Max French Door Refrigerator",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "StainlessSteel.jpg",
                    Price = 2240.00M,
                    ProductTypeId = "602d2149e773f2a3990b47f9",
                    ProductBrandId="602d2149e773f2a3990b47fa"
                },
                new Product()
                {
                    Id = "602d2149e773f2a3990b47f3",
                    Name = "sumsunge ref side",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "boot-ang1.png",
                    Price = 2240.00M,
                    ProductTypeId = "602d2149e773f2a3990b47f9",
                    ProductBrandId="602d2149e773f2a3990b47f6"
                },
                  new Product()
                {
                    Id = "602d2149e773f2a3990b47f4",
                    Name = "power Bank Redmi 200000",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "power200redmi.jpg",
                    Price = 240.00M,
                    ProductTypeId = "602d2149e773f2a3990b47fa",
                    ProductBrandId="602d2149e773f2a3990b47f8"
                },
                     new Product()
                {
                    Id = "622d2149e773f2a3990b47f0",
                    Name = "Galaxy Watch4 44mm",
                    Description = "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus. Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.",
                    PictureUrl = "GalaxyWatch444mm.jpg",
                    Price = 540.00M,
                    ProductTypeId = "602d2149e773f2a3990b47fa",
                    ProductBrandId="602d2149e773f2a3990b47f6"
                }

            };
        }
    }
}
