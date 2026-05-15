namespace ApiDocGen.Models.Responses;

public class RouteInfo
{
    public string HttpMethod { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public string ControllerName { get; set; } = string.Empty;
    public string ActionName { get; set; } = string.Empty;
    public string? Summary { get; set; }
    public List<ParameterInfo> Parameters { get; set; } = new();
    public RequestBodyInfo? RequestBody { get; set; }
    public List<ResponseInfo> Responses { get; set; } = new();
    public List<string> Attributes { get; set; } = new();
}

public class ParameterInfo
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
}

public class RequestBodyInfo
{
    public string TypeName { get; set; } = string.Empty;
    public List<PropertyInfo> Properties { get; set; } = new();
}

public class PropertyInfo
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public string? Summary { get; set; }
}

public class ResponseInfo
{
    public int StatusCode { get; set; }
    public string TypeName { get; set; } = string.Empty;
    public string? Description { get; set; }
}