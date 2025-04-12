using FluentAssertions;
using FluentValidation.TestHelper;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Enum;
using PCParts.Application.Validation;

namespace PCParts.Application.Tests.Validation;

public class CreateSpecificationCommandValidatorShould
{
    private readonly CreateSpecificationCommandValidator _sut;

    public CreateSpecificationCommandValidatorShould()
    {
        _sut = new CreateSpecificationCommandValidator();
    }

    [Theory]
    [MemberData(nameof(GetValidCommands))]
    public async Task ReturnSuccess_WhenCommandIsValid(CreateSpecificationCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFalse_WhenCommandIsInvalid(CreateSpecificationCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand =
            new CreateSpecificationCommand(Guid.NewGuid(), "Specification", SpecificationDataType.STRING);
        var outOfLength = "A".PadRight(51, 'A');

        yield return new object[] { validCommand with { Name = string.Empty } };
        yield return new object[] { validCommand with { Name = outOfLength } };
        yield return new object[] { validCommand with { CategoryId = Guid.Empty }};
        yield return new object[] { validCommand with { Type = 0}};
    }

    public static IEnumerable<object[]> GetValidCommands()
    {
        var validCommand =
            new CreateSpecificationCommand(Guid.NewGuid(), "Specification", SpecificationDataType.STRING);
        yield return new object[] { validCommand };
    }
}