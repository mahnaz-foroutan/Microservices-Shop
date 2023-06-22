using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Ordering.Domain.Entities.Identity;
using Ordering.Infrastructure.Persistence.Identity;

namespace Ordering.Api.Extensions
{
    public static class IdentityServiceExtensions
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, 
            IConfiguration config)
        {
            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseSqlServer(config.GetConnectionString("IdentityConnection"));
            });
            //services.AddIdentity<AppIdentityDbContext, IdentityRole>();
            //        services.AddIdentity<AppUser, IdentityRole>(options =>
            //        {
            //            options.User.RequireUniqueEmail = false;
            //        })
            //.AddEntityFrameworkStores<AppIdentityDbContext>()
            //.AddDefaultTokenProviders();
            services.AddIdentity<AppUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>().AddSignInManager<SignInManager<AppUser>>();



            //services.AddIdentityCore<AppUser>(opt => 
            //{
            //    // add identity options here
            //})
            //.AddEntityFrameworkStores<AppIdentityDbContext>()
            //.AddSignInManager<SignInManager<AppUser>>();
            services.AddAuthentication(options =>
            {
               //options= new OpenIdConnectEvents()
               //{
               //    OnAuthorizationCodeReceived = context =>
               //    {
               //        context.TokenEndpointRequest.SetParameter("id_token_key_type", "JWK");
               //        return Task.CompletedTask;
               //    }
               //};
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                    ValidIssuer = config["Token:Issuer"],
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = config["Token:Audience"],

                };
            });


            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(options => 
            //    {
            //        options.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey  = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
            //            ValidIssuer = config["Token:Issuer"],
            //            ValidateIssuer = true,
            //            ValidateAudience = false
            //        };
            //    });


            services.AddAuthorization();

            return services;
        }
    }
}