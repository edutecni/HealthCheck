using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace HealthCheck.Services
{
    // Essa classe é um Serviço também conhecido com Middleware
    public class ICMPHealthCheck : IHealthCheck
    {
        private readonly string Host = "www.does-not-exist.com";
        private readonly int HealthyRoundtripTime = 30;

        public async Task<HealthCheckResult> CheckHealthAsync(
                HealthCheckContext context,
                CancellationToken cancellationToken =  default
            )
        {
            try
            {
                using var ping = new Ping();
                var reply = await ping.SendPingAsync(Host);

                switch (reply.Status)
                {
                    case IPStatus.Success:
                        return (reply.RoundtripTime > HealthyRoundtripTime) ? HealthCheckResult.Degraded() : HealthCheckResult.Healthy();                    
                    default:
                        return HealthCheckResult.Unhealthy();
                }
            }
            catch (Exception)
            {

                return HealthCheckResult.Unhealthy();
            }
        }
    }
}
