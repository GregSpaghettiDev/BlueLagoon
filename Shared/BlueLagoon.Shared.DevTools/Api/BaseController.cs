using BlueLagoon.Shared.DevTools.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Mime;
using System.Text.Json;

namespace BlueLagoon.Shared.DevTools.Api;

[Route("[controller]")]
[ApiController]
public abstract class BaseController : ControllerBase
{
    protected JsonSerializerOptions _jsonSerializerOptions;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public BaseController(IHttpContextAccessor httpContextAccessor)
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
        _httpContextAccessor = httpContextAccessor;
    }

    protected ContentResult ContentResult<TDto>(TDto content, HttpStatusCode statusCode) where TDto : class
        =>  new()
            {
                ContentType = MediaTypeNames.Application.Json,
                StatusCode = (int)statusCode,
                Content = content is { } ? JsonSerializer.Serialize(content, _jsonSerializerOptions) : JsonSerializer.Serialize(new Empty(), _jsonSerializerOptions)
            };

    protected ContentResult CreatedContentResult()
    {
        var result = new ContentResult()
        {
            ContentType = MediaTypeNames.Application.Json,
            StatusCode = (int)HttpStatusCode.Created,
        };

        if (_httpContextAccessor?.HttpContext?.Items?.TryGetValue(nameof(CreatedResource.CreatedResourceId), out var resId) ?? false)
            if (Guid.TryParse(resId as string, out Guid createdResourceId))
                result.Content = JsonSerializer.Serialize(new CreatedResource() { CreatedResourceId = createdResourceId });

        return result;
    }

    protected ContentResult NoContentResult()
    {
        var result = new ContentResult()
        {
            StatusCode = (int)HttpStatusCode.NoContent,
        };

        return result;
    }
}

public class Empty { }