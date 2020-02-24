# HealthCheckDemo
Health checks in ASP.NET Core

By Using Asp.net core Health check

 1. Liveness health check - the liveness health check is the simplest type of health check because it only has the ability to report that     your application is running
 
 2. Readiness health check - the readiness health checks are great because they allow you to check the health of any additional       
    functionality that your application needs in order to function.
    
So it's the combination of liveness health checks and readiness health checks that makes ASP.NET Core health checks such an interesting feature within your application in terms of reporting the health of your application.

In order to run these health checks, all we have to do is call a HTTP endpoint on our application, and in return, we get a HTTP response, which is basically a health response, which includes a health status of Healthy, Degraded, or Unhealthy, and each one of these health statuses maps to the HTTP status that's returned as part of the HTTP response. The other excellent thing about ASP.NET Core health checks is it allows you to feature multiple health check endpoints within your application, and each one can execute different sets of health checks, and for each health check endpoint, you can have a custom health check response. 

