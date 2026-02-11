using BlueLagoon.Shared.DevTools.Http;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;

namespace BlueLagoon.Shared.Infrastructure.Exceptions.Handlers;

internal sealed class DbOptimisticConcurrencyExceptionHandler(IProblemDetailsService problemDetailsService,
                                                              ILogger<GlobalExceptionHandler> logger,
                                                              IWebHostEnvironment env)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        if (exception is not DbUpdateConcurrencyException dbUpdateConcurrencyException)
            return false;

        logger.LogError(exception, "Wystąpił konflikt edycji danych {Message}", exception.Message);

        httpContext.Response.StatusCode = StatusCodes.Status409Conflict;

        var userId = httpContext.GetUserIdAsGuid();
        var userFullName = string.Concat(httpContext.GetClaimValueByName(OpenIddictConstants.Claims.GivenName), " ", httpContext.GetClaimValueByName(OpenIddictConstants.Claims.FamilyName));

        string detail = $"Dane zostały zmienione przez innego użytkownika ({userFullName}). Odśwież formularz i zedytuj dane ponownie.";
        var problemDetailsContext = new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Detail = detail,
                Status = StatusCodes.Status409Conflict,
                Title = "Konflikt edycji danych",
                Extensions =
                {
                    ["errorCode"] = "IAM_CONCURRENCY_CONFLICT"
                }
            }
        };

        if (env.IsDevelopment())
        {
            var entityNames =
                    dbUpdateConcurrencyException.Entries
                                .Select(entry => entry.Entity.GetType().Name)
                                .Distinct();

            detail = $"{detail} Encje: {string.Join(", ", entityNames)}";
            problemDetailsContext.ProblemDetails.Extensions.Add("affectedEntities", string.Join("; ", entityNames));
        }

        problemDetailsContext.ProblemDetails.Detail = detail;

        return await problemDetailsService.TryWriteAsync(problemDetailsContext);
    }
}
