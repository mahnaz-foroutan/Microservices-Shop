using AutoMapper;
using Basket.Api.GrpcServices;
using Basket.Api.Mapper;
using Basket.Api.Repositories;
using Common.Logging;
using Discount.Grpc.Protos;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using StackExchange.Redis;
using System.Linq;
using System;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Builder;
using Basket.Api.Controllers;

var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        //builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
        //{
        //    var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("ConnectionString"), true);
        //    return ConnectionMultiplexer.Connect(configuration);
        //});
        builder.Services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = builder.Configuration.GetValue<string>("CacheSettings:ConnectionString");
        });
        builder.Services.AddScoped<IDistributedCacheWrapper, DistributedCacheWrapper>();
        builder.Services.AddScoped<IBasketRepository, BasketRepository>();

        builder.Services.AddGrpcClient<DiscountProtoService.DiscountProtoServiceClient>
        (options =>
        {
            options.Address = new Uri(builder.Configuration["GrpcSettings:DiscountUrl"]);
        });
        builder.Services.AddGrpc();
        builder.Services.AddScoped<DiscountGrpcService>();
        builder.Services.AddMassTransit(config =>
        {
            config.UsingRabbitMq((ctx, conf) =>
            {
                conf.Host(builder.Configuration.GetValue<string>("EventBusSettings:HostAddress"));
            });
        });
        builder.Services.AddMassTransitHostedService();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Basket.Api", Version = "v1" });
        });
        //var mapper = BasketProfile.CreateMapper();
        //services.AddSingleton(mapper);
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer("GatewayAuthenticationKey", option =>
            {
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
        builder.Host.UseSerilog(SeriLogger.Configure);
builder.Services.AddHealthChecks()
 .AddRedis(builder.Configuration["CacheSettings:ConnectionString"], failureStatus: HealthStatus.Unhealthy)
  //.AddCheck<CustomHealthChecks>("Custom Health Checks"); //Here is the custom class dependency injection
 .AddUrlGroup(new Uri($"http://localhost:8001/api/v1/Basket"),
           "Basket Health Check", HealthStatus.Degraded, timeout: new TimeSpan(0, 0, 5));//tags: new[] { "ready" }
//builder.Services.AddHealthChecksUI(options =>
//{
//    options.SetEvaluationTimeInSeconds(5); //Sets the time interval in which HealthCheck will be triggered
//    options.MaximumHistoryEntriesPerEndpoint(10); //Sets the maximum number of records displayed in history
//    options.AddHealthCheckEndpoint("Basket Checks API", "/Basket"); //Sets the Health Check endpoint
//}).AddInMemoryStorage(); //Here is the memory bank configuration

var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            //app.UseSwaggerUI();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Basket.Api v1"));

        }

app.UseRouting();

app.UseAuthentication(); // 👈 new code
        app.UseAuthorization();

        app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapHealthChecks("/health", new HealthCheckOptions()
    {
        ResultStatusCodes = {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                        [HealthStatus.Unhealthy] =StatusCodes.Status503ServiceUnavailable
                            },
        ResponseWriter = WriteHealthCheckReadyResponse,
      //  Predicate = (check) => check.Tags.Contains("ready"),
        AllowCachingResponses = false
    });

    //endpoints.MapHealthChecks("/health/live", new HealthCheckOptions()
    //{
    //    Predicate = (check) => !check.Tags.Contains("ready"),
    //    ResponseWriter = WriteHealthCheckLiveResponse,
    //    AllowCachingResponses = false
    //});
});



Task WriteHealthCheckReadyResponse(HttpContext context, HealthReport healthReport)
{
    context.Response.ContentType = "application/json; charset=utf-8";

    var options = new JsonWriterOptions { Indented = true };

    using var memoryStream = new MemoryStream();
    using (var jsonWriter = new Utf8JsonWriter(memoryStream, options))
    {
        jsonWriter.WriteStartObject();
        jsonWriter.WriteString("status", healthReport.Status.ToString());
        jsonWriter.WriteStartObject("results");

        foreach (var healthReportEntry in healthReport.Entries)
        {
            jsonWriter.WriteStartObject(healthReportEntry.Key);
            jsonWriter.WriteString("status",
                healthReportEntry.Value.Status.ToString());
            jsonWriter.WriteString("description",
                healthReportEntry.Value.Description);
            jsonWriter.WriteStartObject("data");

            foreach (var item in healthReportEntry.Value.Data)
            {
                jsonWriter.WritePropertyName(item.Key);

                System.Text.Json.JsonSerializer.Serialize(jsonWriter, item.Value,
                    item.Value?.GetType() ?? typeof(object));
            }

            jsonWriter.WriteEndObject();
            jsonWriter.WriteEndObject();
        }

        jsonWriter.WriteEndObject();
        jsonWriter.WriteEndObject();
    }

    return context.Response.WriteAsync(
        Encoding.UTF8.GetString(memoryStream.ToArray()));
}
Task WriteHealthCheckLiveResponse(HttpContext httpContext, HealthReport result)
{
    httpContext.Response.ContentType = "application/json";

    var json = new JObject(
            new JProperty("OverallStatus", result.Status.ToString()),
            new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00"))
        );

    return httpContext.Response.WriteAsync(json.ToString(Formatting.Indented));
}

app.Run();


public partial class Program
{
   
}
