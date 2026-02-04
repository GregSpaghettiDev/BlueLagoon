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
        if (exception is not BaseIamApplicationException coreException)
            return false;

        Logger.LogError(exception, "Wystąpił wyjątek aplikacyjny ({Code}) modułu Iam: {Message}", coreException.ErrorCode, coreException.Message);

        return await ProblemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Detail = coreException.Message,
                Status = (int)coreException.StatusCode,
                Title = "Naruszenie reguły aplikacyjnej w module IAM",
                Instance = httpContext.Request.Path
            }
        });
    }
}