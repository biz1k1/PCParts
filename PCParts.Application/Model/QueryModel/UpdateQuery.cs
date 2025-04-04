namespace PCParts.Application.Model.QueryModel;

public class UpdateQuery
{
    public Guid Id { get; set; }
    public object[] Parameters { get; set; } = [];
    public string Query { get; set; } = string.Empty;
}