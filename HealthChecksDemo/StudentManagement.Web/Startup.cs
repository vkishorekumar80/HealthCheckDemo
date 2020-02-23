using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Linq;
using StudentManagement.Web.Data;
using StudentManagement.Web.HealthChecks;

namespace StudentManagement.Web {
    public class Startup {
        public Startup(IConfiguration configuration) {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services) {
            var connectionString = Configuration.GetConnectionString("InhouseDB");
            services.AddControllersWithViews();
            services.AddDbContext<InhouseContext>(option => option.UseSqlServer(connectionString));
            ////services.AddJsonOptions(options =>
            // {
            //     options.SerializerSettings.ContractResolver
            //     = new CamelCasePropertyNamesContractResolver();

            // });

            var studentManagementUrl = Configuration["StudentManagementUrl"];
            var logFilePath = Configuration["LogFilePath"];

            services.AddHealthChecks()
                .AddSqlServer(connectionString, failureStatus: HealthStatus.Unhealthy, tags: new[] { "ready" })
                // return healthstatus as degraded, if the service does not impact the working of the application
                .AddUrlGroup(new Uri($"{studentManagementUrl}/api/Health"),
                    "Student Management Api Health Check",
                    HealthStatus.Degraded,
                    tags: new[] { "ready" },
                    timeout: new TimeSpan(0, 0, 5))
               //Custom Health Checkup 
               // .AddCheck("File Path Health Check", new FilePathWriteHealthCheck(logFilePath), HealthStatus.Unhealthy, tags: new[] { "ready" });
               .AddFilePathWrite(logFilePath, HealthStatus.Unhealthy, tags: new[] { "ready" });

            // Use Health Check UI
            services.AddHealthChecksUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if(env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            } else {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=InhouseStudents}/{action=Index}/{id?}");

                // Customization of the health results are done using HealthCheckOptions
                endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions() {
                    ResultStatusCodes = {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status500InternalServerError,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    },
                    ResponseWriter = WriteHealthCheckReadyResponse,
                    Predicate = (check)=> check.Tags.Contains("ready"),
                    AllowCachingResponses = false
                });

                endpoints.MapHealthChecks("/health/live", new HealthCheckOptions {
                    Predicate = (check) => !check.Tags.Contains("ready"),
                    ResponseWriter = WriteHealthCheckLiveResponse,
                    AllowCachingResponses = false
                });

                endpoints.MapHealthChecks("/healthui", new HealthCheckOptions {
                    Predicate= _ =>true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });
            });

            // Needs to add addition settings to configure the path in AppSettings
            app.UseHealthChecksUI();
        }

        private Task WriteHealthCheckLiveResponse(HttpContext httpContext, HealthReport result) {
            // set the response type as json to show json result as output
            httpContext.Response.ContentType = "applcation/json";

            var json = new JObject(
                      new JProperty("OverallStatus", result.Status.ToString()),
                      new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00"))
                  );
            return httpContext.Response.WriteAsync(json.ToString(Newtonsoft.Json.Formatting.Indented));
        }

            private Task WriteHealthCheckReadyResponse(HttpContext httpContext, HealthReport result) {
            // set the response type as json to show json result as output
            httpContext.Response.ContentType = "applcation/json";

            //var json = new JObject(
            //          new JProperty("OverallStatus", result.Status.ToString()),
            //          new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00"))
            //      );
            var json = new JObject(
                     new JProperty("OverallStatus", result.Status.ToString()),
                     new JProperty("TotalChecksDuration", result.TotalDuration.TotalSeconds.ToString("0:0.00")),
                     new JProperty("DependencyHealthChecks", new JObject(result.Entries.Select(dicItem =>
                         new JProperty(dicItem.Key, new JObject(
                                 new JProperty("Status", dicItem.Value.Status.ToString()),
                                 new JProperty("Duration", dicItem.Value.Duration.TotalSeconds.ToString("0:0.00"))
                                 //new JProperty("Exception", dicItem.Value.Exception?.Message),
                                 //new JProperty("Data", new JObject(dicItem.Value.Data.Select(dicData =>
                                 //    new JProperty(dicData.Key, dicData.Value))))
                             ))
                     )))
                 );
            return httpContext.Response.WriteAsync(json.ToString(Newtonsoft.Json.Formatting.Indented));
        }
    }
}
