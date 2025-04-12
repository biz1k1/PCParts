using FluentAssertions;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Enum;


namespace PCParts.Application.Tests.Helpers;

public class ValidationHelperShould
{
    [Fact]
    public async Task ReturnFalse_When_ValidationValid()
    {
        var result = ValidationHelper.IsValueValid(SpecificationDataType.INT, "text");
        result.Should().BeFalse();
    }

    [Fact]
    public async Task ReturnTrue_When_ValidationInvalid()
    {
        var result = ValidationHelper.IsValueValid(SpecificationDataType.STRING, "text");
        result.Should().BeTrue();
    }
}