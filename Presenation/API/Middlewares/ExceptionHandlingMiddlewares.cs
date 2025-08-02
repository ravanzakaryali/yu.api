using FluentValidation;

namespace Yu.API.Middlewares;

public class ExceptionHandling
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private ICurrentUserService? _currentUserService;
    public ExceptionHandling(RequestDelegate next, ILogger<ExceptionHandling> logger)
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvokeAsync(HttpContext httpContext, ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
        try
        {
            await _next(httpContext);
        }
        catch (NotFoundException ex)
        {
            ErrorResponseDto error = await HandleExceptionAsync(httpContext, ex, ex.HttpStatusCode);
        }
        catch (AlreadyExistsException ex)
        {
            ErrorResponseDto error = await HandleExceptionAsync(httpContext, ex, ex.HttpStatusCode);
        }
        catch (UnauthorizedAccessException ex)
        {
            ErrorResponseDto error = await HandleExceptionAsync(httpContext, ex, HttpStatusCode.Unauthorized);
        }
        catch (ValidationException ex)
        {
            string validationErrors = string.Join(", ", ex.Errors.Select(e => e.ErrorMessage));
            ErrorResponseDto error = await HandleExceptionAsync(httpContext, ex, HttpStatusCode.BadRequest, validationErrors: validationErrors);
        }
        catch (Exception ex)
        {
            ErrorResponseDto error = await HandleExceptionAsync(httpContext, ex);

            _logger.LogError(ex, $"Request {httpContext.Request?.Method}: {httpContext.Request?.Path.Value} failed Error: {@error}", error);
        }
    }
    private async Task<ErrorResponseDto> HandleExceptionAsync(HttpContext context, Exception exception, HttpStatusCode statusCode = HttpStatusCode.InternalServerError, string? message = null, string? validationErrors = null)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;
        ErrorResponseDto response = new()
        {
            Message = message ?? exception.Message,
            StatusCode = (int)statusCode,
            Errors = validationErrors
        };
        string json = JsonConvert.SerializeObject(response, new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
        await context.Response.WriteAsync(json);

        // telegram bot send message 
        return response;
    }
}
public static class ExceptionHandlerMiddelwareExtensions
{
    public static IApplicationBuilder UseExceptionHandling(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionHandling>();
    }
}

