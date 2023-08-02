using Common.Logging;
using EventBus.Messages.Common;
using GreenPipes.Agents;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Ordering.Api.EventBusConsumer;
using Ordering.Api.Extensions;
using Ordering.Application;
using Ordering.Domain.Entities.Identity;
using Ordering.Infrastructure;
using Ordering.Infrastructure.Persistence;
using Ordering.Infrastructure.Persistence.Identity;
using Ordering.Infrastructure.Repositories;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAuthentication();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddScoped<BasketCheckoutConsumer>();
builder.Services.AddIdentityServices(builder.Configuration);
builder.Services.AddSwaggerDocumentation();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddMassTransit(config =>
{
    config.AddConsumer<BasketCheckoutConsumer>();
    config.UsingRabbitMq((ctx, conf) =>
    {
        conf.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
        conf.ReceiveEndpoint(EventBusConstants.BasketCheckoutQueue, c =>
        {
          
            c.ConfigureConsumer<BasketCheckoutConsumer>(ctx);
        });
    });
});
builder.Services.AddMassTransitHostedService();
builder.Services.AddControllers();
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy",
//        builder => builder.AllowAnyOrigin()
//        .AllowAnyMethod()
//        .AllowAnyHeader()
//        .AllowCredentials()
//    .Build());
//});
//builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//    .AddCookie(options =>
//    {
//        options.Events.OnRedirectToLogin = (context) =>
//        {
//            context.Response.StatusCode = 401;
//            return Task.CompletedTask;
//        };
//    });
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Host.UseSerilog(SeriLogger.Configure);
var app = builder.Build();

//app.MigrateDatabase<OrderContext>((context, services) =>
//    {
//        var logger = services.GetService<ILogger<OrderContextSeed>>();
//        OrderContextSeed.SeedAsync(context, logger).Wait();
//    });
app.UseSwaggerDocumentation();
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");
app.UseCookiePolicy();
app.UseStaticFiles();

app.UseCors("CorsPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapDefaultControllerRoute();
//app.MapFallbackToController("Index", "Fallback");
app.UseHttpsRedirection();
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<OrderContext>();
var identityContext = services.GetRequiredService<AppIdentityDbContext>();
var userManager = services.GetRequiredService<UserManager<AppUser>>();
var logger = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
   await identityContext.Database.MigrateAsync();
    await OrderContextSeed.SeedAsync(context);
    await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
}
catch (Exception ex)
{
    logger.LogError(ex, "An error occured during migration");
}
app.Run();

