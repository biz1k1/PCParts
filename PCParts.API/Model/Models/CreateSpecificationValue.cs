namespace PCParts.API.Model.Models;

public class CreateSpecificationValue
{
    public Guid ComponentId { get; set; }
    public Guid SpeicificationId { get; set; }
    public string Value { get; set; }
}