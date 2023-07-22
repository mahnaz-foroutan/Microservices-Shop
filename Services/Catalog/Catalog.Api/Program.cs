using Catalog.Api.Errors;
using Catalog.Core.Interfaces;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Common.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
            {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Catalog.Api", Version = "v1" });
});

builder.Services.Configure<ProductDatabaseSettings>(
               builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddSingleton<IProductDatabaseSettings>(sp =>
            sp.GetRequiredService<IOptions<ProductDatabaseSettings>>().Value);
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = actionContext =>
    {
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();

        var errorResponse = new ApiValidationErrorResponse
        {
            Errors = errors
        };

        return new BadRequestObjectResult(errorResponse);
    };
});
builder.Services.AddDirectoryBrowser();

builder.Host.UseSerilog(SeriLogger.Configure);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Catalog.Api v1"));
}
app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.UseFileServer();
app.UseStaticFiles();
app.UseDefaultFiles();

//app.UseFileServer(new FileServerOptions
//{
//    FileProvider = new PhysicalFileProvider(
//           Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
//    RequestPath = "/wwwroot",
//    EnableDirectoryBrowsing = true
//});
app.MapDefaultControllerRoute();
app.MapControllers();

app.Run();
public partial class Program { }