using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using System;
using Serilog.Exceptions;

namespace Common.Logging
{
    public static class SeriLogger
    {
        public static Action<HostBuilderContext, LoggerConfiguration> Configure =>
           (context, configuration) =>
           {
               #region Enriching Logger Context
               var env = context.HostingEnvironment;
               configuration.Enrich.FromLogContext()
                   .Enrich.WithProperty("ApplicationName", env.ApplicationName)
                   .Enrich.WithProperty("Environment", env.EnvironmentName)
                   .Enrich.WithExceptionDetails()
                 //.Enrich.WithProcessId()
                 //  .Enrich.WithProcessName();
                   .ReadFrom.Configuration(context.Configuration);
               #endregion
               configuration.WriteTo.Console().MinimumLevel.Information();

               #region ElasticSearch Configuration.
               var elasticUrl = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");
               if (!string.IsNullOrEmpty(elasticUrl))
               {
                   configuration.Enrich.FromLogContext()
                       .Enrich.WithMachineName()
                       .WriteTo.Debug()
                       .WriteTo.Console()
                       .WriteTo.Elasticsearch(
                       new ElasticsearchSinkOptions(new Uri(elasticUrl))
                       {
                           AutoRegisterTemplate = true,
                           AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                           IndexFormat = $"applogs-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
                           MinimumLogEventLevel = LogEventLevel.Debug
                       });
               }
               #endregion

               //var elasticUri = context.Configuration.GetValue<string>("ElasticConfiguration:Uri");

               //configuration
               //     .Enrich.FromLogContext()
               //     .Enrich.WithMachineName()
               //     .WriteTo.Debug()
               //     .WriteTo.Console()
               //     .WriteTo.Elasticsearch(
               //         new ElasticsearchSinkOptions(new Uri(elasticUri))
               //         {
               //             IndexFormat = $"applogs-{context.HostingEnvironment.ApplicationName?.ToLower().Replace(".", "-")}-{context.HostingEnvironment.EnvironmentName?.ToLower().Replace(".", "-")}-{DateTime.UtcNow:yyyy-MM}",
               //             AutoRegisterTemplate = true,
               //             NumberOfShards = 2,
               //             NumberOfReplicas = 1
               //         })
               //     .Enrich.WithProperty("Environment", context.HostingEnvironment.EnvironmentName)
               //     .Enrich.WithProperty("Application", context.HostingEnvironment.ApplicationName)
               //     .ReadFrom.Configuration(context.Configuration);
           };
    }
}
