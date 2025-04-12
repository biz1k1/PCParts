using FluentAssertions;
using FluentValidation.TestHelper;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Enum;
using PCParts.Application.Validation;

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
        var validCommand = new UpdateSpecificationCommand(Guid.NewGuid(), "Specification", SpecificationDataType.STRING);
        var outOfLength = "A".PadRight(51, 'A');

        yield return new object[] { validCommand with { Id = Guid.Empty } };
        yield return new object[] { validCommand with { Name = String.Empty, Type = 0} };
        yield return new object[] { validCommand with { Name = outOfLength } };
        yield return new object[] { validCommand with { Type = 0 } };
    }

    public static IEnumerable<object[]> GetValidCommands()
    {
        var validCommand = new UpdateSpecificationCommand(Guid.NewGuid(), "Specification", SpecificationDataType.STRING);
        yield return new object[] { validCommand };
        yield return new object[] { validCommand with { Name = string.Empty} };
    }
}