namespace PCParts.API.Model
{
    public record SpecificationValue
    {
        public Guid Id { get; init; }
        public string SpecificationName { get; init; }
        public object Value { get; init; }
    }
}
