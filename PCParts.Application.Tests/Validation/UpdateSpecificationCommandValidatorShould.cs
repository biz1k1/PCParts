using FluentAssertions;
using FluentValidation.TestHelper;
using PCParts.Application.Model.Command;
using PCParts.Application.Validation;
using PCParts.Domain.Enum;

namespace PCParts.Application.Tests.Validation;

public class UpdateSpecificationCommandValidatorShould
{
    private readonly UpdateSpecificationCommandValidator _sut;

    public UpdateSpecificationCommandValidatorShould()
    {
        _sut = new UpdateSpecificationCommandValidator();
    }

    [Theory]
    [MemberData(nameof(GetValidCommands))]
    public async Task ReturnSuccess_WhenCommandValid(UpdateSpecificationCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFalse_WhenCommandIsInvalid(UpdateSpecificationCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand =
            new UpdateSpecificationCommand(Guid.NewGuid(), "Specification", "value", SpecificationDataType.STRING);
        var outOfLength = "A".PadRight(101, 'A');

        yield return new object[] { validCommand with { Id = Guid.Empty } };
        yield return new object[] { validCommand with { Name = string.Empty } };
        yield return new object[] { validCommand with { Name = outOfLength } };
        yield return new object[] { validCommand with { Value = string.Empty } };
        yield return new object[] { validCommand with { Value = outOfLength } };
        yield return new object[] { validCommand with { Value = string.Empty, Type = SpecificationDataType.INT } };
    }

    public static IEnumerable<object[]> GetValidCommands()
    {
        var validCommand =
            new UpdateSpecificationCommand(Guid.NewGuid(), "Specification", "value", SpecificationDataType.STRING);
        yield return new object[] { validCommand };
        yield return new object[] { validCommand with { Name = string.Empty, Value = string.Empty } };
    }
}