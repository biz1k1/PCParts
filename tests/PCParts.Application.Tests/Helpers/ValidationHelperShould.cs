using FluentAssertions;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Enums;

namespace PCParts.Application.Tests.Helpers;

public class ValidationHelperShould
{
    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFalse_When_ValidationInvalid(CommandForTestValidationHelper command)
    {
        var result = ValidationHelper.IsValueValid(command.Type, command.Value);
        result.Should().BeFalse();
    }

    [Theory]
    [MemberData(nameof(GetValidCommands))]
    public async Task ReturnTrue_When_ValidationValid(CommandForTestValidationHelper command)
    {
        var result = ValidationHelper.IsValueValid(command.Type, command.Value);
        result.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new CommandForTestValidationHelper(SpecificationDataType.IntType, "text");
        yield return new object[] { validCommand };
        yield return new object[] { validCommand with { Type = SpecificationDataType.IntType, Value = "10,5" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.IntType, Value = "true" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.StringType, Value = "10" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.StringType, Value = "10,5" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.StringType, Value = "true" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.DoubleType, Value = "text" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.DoubleType, Value = "10" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.DoubleType, Value = "true" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BoolType, Value = "text" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BoolType, Value = "10,5" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BoolType, Value = "10" } };
    }

    public static IEnumerable<object[]> GetValidCommands()
    {
        var validCommand = new CommandForTestValidationHelper(SpecificationDataType.StringType, "text");

        yield return new object[] { validCommand };
        yield return new object[] { validCommand with { Type = SpecificationDataType.IntType, Value = "10" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BoolType, Value = "true" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BoolType, Value = "false" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.DoubleType, Value = "10,5" } };
    }
}

public record CommandForTestValidationHelper(SpecificationDataType Type, string Value);
