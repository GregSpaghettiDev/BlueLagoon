namespace BlueLagoon.Modules.Iam.Core.Services.Dto;

public class EndpointDefinitionDto
{
    public string ModuleCode { get; set; }

    public string HttpMethod { get; set; }

    public string Path { get; set; }

    public string OperationId { get; set; }

    public string Summary { get; set; }

    public IEnumerable<string> Tags { get; set; }
}
