using FluentAssertions;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Enum;


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
        var validCommand = new CommandForTestValidationHelper(SpecificationDataType.INT, "text");
        yield return new object[] { validCommand};
        yield return new object[] { validCommand with { Type = SpecificationDataType.INT, Value = "10,5" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.INT, Value = "true" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.STRING, Value = "10" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.STRING, Value = "10,5" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.STRING, Value = "true" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.DOUBLE, Value = "text" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.DOUBLE, Value = "10" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.DOUBLE, Value = "true" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BOOL, Value = "text" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BOOL, Value = "10,5" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BOOL, Value = "10" } };
    }

    public static IEnumerable<object[]> GetValidCommands()
    {
        var validCommand = new CommandForTestValidationHelper(SpecificationDataType.STRING, "text");

        yield return new object[] { validCommand};
        yield return new object[] { validCommand with { Type = SpecificationDataType.INT, Value = "10" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BOOL, Value = "true" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.BOOL, Value = "false" } };
        yield return new object[] { validCommand with { Type = SpecificationDataType.DOUBLE, Value = "10,5" } };
    }
}

public record CommandForTestValidationHelper(SpecificationDataType Type, string Value);