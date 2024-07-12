using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace ThomasWright.ServiceTemplate.Handlers;

public class GlobalExceptionHandler(IHostEnvironment environment, ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (environment.IsDevelopment())
        {
            return HandleDevelopmentExceptionAsync(httpContext, exception, cancellationToken);
        }
        
        return HandleProductionExceptionAsync(httpContext, exception, cancellationToken);
    }
    
    private async ValueTask<bool> HandleDevelopmentExceptionAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Detail = exception.StackTrace
        };
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";
        
        logger.LogError(exception, exception.Message);
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
    
    private async ValueTask<bool> HandleProductionExceptionAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Title = "An unexpected error occurred.",
            Detail = "An unexpected error occurred. Please try again later."
        };
        
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
        httpContext.Response.ContentType = "application/problem+json";
        
        logger.LogError(exception, "An unexpected error occurred.");
        
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
        
        return true;
    }
}