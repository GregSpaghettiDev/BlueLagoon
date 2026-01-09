using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BlueLagoon.Modules.SalesContracting.Api.Controllers;

[ApiController]
[Route("[controller]")]
public sealed class TestSalesContractingController : ControllerBase
{
    [HttpGet("test")]
    [SwaggerOperation(OperationId = nameof(TestSalesContracting))]
    public IActionResult TestSalesContracting()
    {
        return Ok("Sales and contracting module is working!");
    }
}
