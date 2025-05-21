using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ekart.Middleware
{
    // <summary>
    // Middleware to handle unhandled exceptions globally in the API pipeline.
    // Converts exceptions into ProblemDetails responses with status code 500.
    // </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        // <summary>
        // Constructor accepting the next middleware in the pipeline.
        // </summary>
        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        // <summary>
        // Middleware entry point. Executes the next middleware, and catches any unhandled exceptions.
        // </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Continue down the middleware pipeline
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex); // Catch and handle unhandled exception
            }
        }

        // <summary>
        // Formats the exception into a standardized HTTP 500 response using ProblemDetails.
        // </summary>
        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
{
    var statusCode = exception switch
    {
        ArgumentException => (int)HttpStatusCode.BadRequest,
        UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
        InvalidOperationException => (int)HttpStatusCode.Conflict,
        _ => (int)HttpStatusCode.InternalServerError
    };

    var problemDetails = new ProblemDetails
    {
        Status = statusCode,
        Title = "An error occurred",
        Detail = exception.Message
    };

    context.Response.ContentType = "application/json";
    context.Response.StatusCode = problemDetails.Status.Value;
    return context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
}

    }
}
