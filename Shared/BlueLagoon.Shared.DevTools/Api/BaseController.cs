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

    public BaseController()
    {
        _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };
    }

    protected ContentResult ContentResult<TDto>(TDto content, HttpStatusCode statusCode) where TDto : class
        =>  new()
            {
                ContentType = MediaTypeNames.Application.Json,
                StatusCode = (int)statusCode,
                Content = content is { } ? JsonSerializer.Serialize(content, _jsonSerializerOptions) : JsonSerializer.Serialize(new Empty(), _jsonSerializerOptions)
            };
}

public class Empty { }