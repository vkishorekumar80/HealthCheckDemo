using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace StudentManagement.Web.HealthChecks {
    public class FilePathWriteHealthCheck : IHealthCheck {
        private readonly string filePath;
        private IReadOnlyDictionary<string, object> healthCheckData;

        public FilePathWriteHealthCheck(string filePath) {
            this.filePath = filePath;
            healthCheckData = new Dictionary<string, object> {
                {"filePath", filePath }
            };
        }
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
            try {
                var testFile = $"{filePath}\\test.txt";
                var fs = File.Create(testFile);
                fs.Close();
                File.Delete(testFile);

                return Task.FromResult(HealthCheckResult.Healthy());
            } catch(Exception exception) {
                switch(context.Registration.FailureStatus) {
                    case HealthStatus.Degraded:
                        return Task.FromResult(HealthCheckResult.Degraded($"Issues writing to file path", exception, healthCheckData));
                    case HealthStatus.Healthy:
                        return Task.FromResult(HealthCheckResult.Healthy($"Issues writing to file path", healthCheckData));
                    default:
                        return Task.FromResult(HealthCheckResult.Unhealthy($"Issues writing to file path", exception, healthCheckData));
                }
            }
        }
    }
}
