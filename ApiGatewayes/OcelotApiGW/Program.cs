using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Cache.CacheManager;
using Ocelot.Values;
using Common.Logging;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);
//var identityUrl = _cfg.GetValue<string>("IdentityUrl");
//var authenticationProviderKey = "IdentityApiKey";
////…
//services.AddAuthentication()
//    .AddJwtBearer(authenticationProviderKey, x =>
//    {
//        x.Authority = identityUrl;
//        x.RequireHttpsMetadata = false;
//        x.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
//        {
//            ValidAudiences = new[] { "orders", "basket", "locations", "marketing", "mobileshoppingagg", "webshoppingagg" }
//        };
//    });
builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = "GatewayAuthenticationKey";
})
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer("GatewayAuthenticationKey", option =>
    {
        // option.Authority = "https://localhost:8004"; // IDENTITY SERVER URL
        option.RequireHttpsMetadata = false;
        
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Token:Issuer"],
            ValidAudience = builder.Configuration["Token:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:Key"]))
        };
    });

builder.Services.AddAuthorization();
builder.Services.AddOcelot().AddCacheManager(x =>
{
    x.WithDictionaryHandle();
});


builder.Host.ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true);
    })
    .ConfigureLogging((hostingContext, loggingBuilder) =>
    {
        loggingBuilder.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
        loggingBuilder.AddConsole();
        loggingBuilder.AddDebug();
    });

builder.Host.UseSerilog(SeriLogger.Configure);
builder.Services.AddCors();
var app = builder.Build();
app.UseCors(options =>
   options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.MapGet("/", () => "Hello World!");
await app.UseOcelot();
app.UseStaticFiles();
app.UseAuthentication(); 
app.UseAuthorization();
app.Run();
