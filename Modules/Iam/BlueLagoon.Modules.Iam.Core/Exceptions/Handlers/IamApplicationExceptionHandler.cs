using BlueLagoon.Modules.Iam.Core.Exceptions.Abstractions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BlueLagoon.Modules.Iam.Core.Exceptions.Handlers;

internal sealed class IamApplicationExceptionHandler(IProblemDetailsService ProblemDetailsService, ILogger<IamCoreExceptionHandler> Logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not BaseIamApplicationException applicationException)
            return false;

        Logger.LogError(exception, "Wystąpił wyjątek aplikacyjny ({Code}) modułu Iam: {Message}", applicationException.ErrorCode, applicationException.Message);

        return await ProblemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Detail = applicationException.Message,
                Status = (int)applicationException.StatusCode,
                Title = "Naruszenie reguły aplikacyjnej w module IAM",
                Extensions = { ["errorCode"] = applicationException.ErrorCode }
            }
        });
    }
}