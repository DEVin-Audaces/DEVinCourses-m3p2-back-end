using System.Net;
using Newtonsoft.Json;

namespace DEVCoursesAPI.Middleware;

public class MiddlewareError
{
    private readonly RequestDelegate _requestDelegate;

    public MiddlewareError(RequestDelegate requestDelegate)
    {
        this._requestDelegate = requestDelegate;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _requestDelegate(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var code = HttpStatusCode.InternalServerError; 

        if      (exception is Exception)     code = HttpStatusCode.NotFound;

        var resultError = JsonConvert.SerializeObject(new { error = exception.Message });
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)code;
        return context.Response.WriteAsync(resultError);
    }
    
}