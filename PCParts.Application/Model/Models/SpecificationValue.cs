﻿namespace PCParts.Application.Model.Models;

public record SpecificationValue
{
    public Guid Id { get; init; }
    public string SpecificationName { get; init; }
    public object Value { get; init; }
    public Specification Specification { get; init; }
}