using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlueLagoon.Modules.Notifications.Api.Controllers;

[ApiController]
[Route("[controller]")]
internal class TestNotificationController : ControllerBase
{
    [HttpGet("test")]
    [SwaggerOperation(OperationId = nameof(TestNotification))]
    public IActionResult TestNotification()
    {
        return Ok("Notifications module is working!");
    }
}