using System.Text.Json;
using DevConnect.Application.Contracts.Models;
using DevConnect.Exceptions;
using FluentValidation;
using UnauthorizedAccessException = DevConnect.Exceptions.UnauthorizedAccessException;

namespace DevConnect.WebApi.Middlewares;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        var statusCode = GetStatusCode(ex);
        var message = GetErrorMessage(ex);
        
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;
        
        var response = ApiResponseModel<string>.Fail(message, statusCode);

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }

    private static int GetStatusCode(Exception ex) => ex switch
        {
            ArgumentNullException => StatusCodes.Status400BadRequest,
            EntityNotFoundException => StatusCodes.Status404NotFound,
            DomainValidationException => StatusCodes.Status422UnprocessableEntity,
            DuplicateEntityException => StatusCodes.Status409Conflict,
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ForbiddenOperationException => StatusCodes.Status403Forbidden,
            InvalidCredentialsException => StatusCodes.Status400BadRequest,
            ConcurrencyException => StatusCodes.Status409Conflict,
            TokenExpirationException => StatusCodes.Status401Unauthorized,
            TokenValidationException => StatusCodes.Status400BadRequest,
            ValidationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError,
        };

    private static string GetErrorMessage(Exception ex)
    {
        if (ex is ValidationException ve && ve.Errors?.Any() is true)
            return string.Join("; ", ve.Errors);

        return ex.Message;
    }
}