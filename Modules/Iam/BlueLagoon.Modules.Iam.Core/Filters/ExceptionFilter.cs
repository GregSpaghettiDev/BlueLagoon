using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace BlueLagoon.Modules.Iam.Core.Filters;

internal sealed class ExceptionFilter : IAsyncExceptionFilter
{
    private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

    public ExceptionFilter()
    {
        _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
        {
            { typeof(DbUpdateConcurrencyException), HandleOptimisticConcurrencyException },
            { typeof(HttpRequestException), HandleHttpRequestException },
            { typeof(BaseCoreException), HandleCoreException }
        };
    }

    public Task OnExceptionAsync(ExceptionContext context)
    {
        Type type = context.Exception.GetType();

        if (_exceptionHandlers.ContainsKey(type))
        {
            _exceptionHandlers[type].Invoke(context);

            return Task.CompletedTask;
        }

        HandleUnknownException(context);
        return Task.CompletedTask;
    }

    private void HandleCoreException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "Wystąpił błąd",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            Detail = context.Exception.Message
        };

        context.Result = new BadRequestObjectResult(details);

        context.ExceptionHandled = true;
    }

    private void HandleUnknownException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Wystąpił nieznany błąd podczas przetwarzania żądania",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status500InternalServerError
        };

        context.ExceptionHandled = true;
    }

    private void HandleOptimisticConcurrencyException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {

        };
        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status409Conflict
        };
        context.ExceptionHandled = true;
    }

    private void HandleHttpRequestException(ExceptionContext context)
    {
        var details = new ProblemDetails
        {
        };

        context.Result = new ObjectResult(details)
        {
            StatusCode = StatusCodes.Status503ServiceUnavailable
        };
        context.ExceptionHandled = true;
    }
}