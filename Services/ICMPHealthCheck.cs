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
        private readonly string Host;
        private readonly int HealthyRoundtripTime;

        public ICMPHealthCheck(string host, int healpthyRoundTripTime)
        {
            Host = host;
            HealthyRoundtripTime = healpthyRoundTripTime;
        }

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
                        var msg = $"ICMP to {Host} took {reply.RoundtripTime} ms.";

                        return (reply.RoundtripTime > HealthyRoundtripTime) ? HealthCheckResult.Degraded(msg) : HealthCheckResult.Healthy(msg);                    
                    default:
                        var err = $"ICMP to {Host} failed: {reply.Status}";

                        return HealthCheckResult.Unhealthy(err);
                }
            }
            catch (Exception ex)
            {
                var err = $"ICMP to {Host} failed: {ex.Message}";
                return HealthCheckResult.Unhealthy(err);
            }
        }
    }
}
