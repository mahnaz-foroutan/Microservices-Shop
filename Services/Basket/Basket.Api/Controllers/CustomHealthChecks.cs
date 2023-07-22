using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;

namespace Basket.Api.Controllers
{
    public class CustomHealthChecks : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var catUrl = "http://localhost:8001/api/v1/Basket";

            var client = new HttpClient();

            client.BaseAddress = new Uri(catUrl);

            HttpResponseMessage response = await client.GetAsync("");

            return response.StatusCode == HttpStatusCode.OK ?
                await Task.FromResult(new HealthCheckResult(
                      status: HealthStatus.Healthy,
                      description: "The API is healthy （。＾▽＾）")) :
                await Task.FromResult(new HealthCheckResult(
                      status: HealthStatus.Unhealthy,
                      description: "The API is sick (‘﹏*๑)"));
        }
    }
}
