using Common.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Shopping.Aggregator.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Shopping.Aggregator", Version = "v1" });
});
//by type
builder.Services.AddHttpClient<ICatalogService, CatalogService>(c =>
{
    c.BaseAddress = new Uri(builder.Configuration["ApiSettings:CatalogUrl"]);
});

builder.Services.AddHttpClient<IBasketService, BasketService>(b =>
{
    b.BaseAddress = new Uri(builder.Configuration["ApiSettings:BasketUrl"]);
});

//builder.Services.AddHttpClient<IOrderService, OrderService>(o =>
//{
//    o.BaseAddress = new Uri(builder.Configuration["ApiSettings:OrderingUrl"]);
//});
builder.Host.UseSerilog(SeriLogger.Configure);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Shopping.Aggregator v1"));
}

app.UseAuthorization();

app.MapControllers();

app.Run();
