using BlueLagoon.Shared.Infrastructure.Exceptions.Abstractions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlueLagoon.Shared.Infrastructure.Exceptions.Handlers;

internal sealed class AggregateExceptionHandler(IProblemDetailsService problemDetailsService,
                                         ILogger<AggregateExceptionHandler> logger,
                                         IWebHostEnvironment env)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not AggregateException aggregateException)
            return false;

        logger.LogError(exception, "Wystąpił więcej niż jeden wyjątek: {Message}", exception.Message);

        var errorDetails = aggregateException.InnerExceptions.Select(x => new
        {
            ErrorCode = x is BaseCoreException ce
                                ? ce.ErrorCode
                                : x is BaseApplicationException ae
                                                ? ae.ErrorCode
                                                : "AGGREGATED_ERROR",
            x.Message

        });

        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Detail = string.Join(Environment.NewLine, errorDetails.Select(x => x.Message)),
                Status = StatusCodes.Status409Conflict,
                Title = "Zbiorczy wyjątek",
                Extensions =
                {
                    ["errorCodes"] = errorDetails
                }
            }
        };

        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
