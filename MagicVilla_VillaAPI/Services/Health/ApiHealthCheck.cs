using Azure.Core;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RestSharp;

namespace Students_API.Services.Health
{
    public class ApiHealthCheck : IHealthCheck
    {
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            var url = "https://localhost:7156/api/v1/studentAPI/apihealthf";

            var client = new RestClient();

            // making request
            var request = new RestRequest(url , Method.Get);

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }
            else
                return Task.FromResult(HealthCheckResult.Unhealthy());
        }
    }
}
