using FluentAssertions;
using FluentValidation.TestHelper;
using PCParts.Application.Model.Command;
using PCParts.Application.Validation;

namespace PCParts.Application.Tests.Validation;

public class CreateComponentCommandValidatorShould
{
    private readonly CreateComponentCommandValidator _sut;

    public CreateComponentCommandValidatorShould()
    {
        _sut = new CreateComponentCommandValidator();
    }

    [Theory]
    [MemberData(nameof(GetValidCommands))]
    public async Task ReturnSuccess_WhenCommandIsValid(CreateComponentCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFalse_WhenCommandInvalid(CreateComponentCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new CreateComponentCommand("Name", Guid.NewGuid());
        var outOfLength = "A".PadRight(51, 'A');

        yield return new object[] { validCommand with { CategoryId = Guid.Empty } };
        yield return new object[] { validCommand with { Name = string.Empty } };
        yield return new object[] { validCommand with { Name = outOfLength } };
    }

    public static IEnumerable<object[]> GetValidCommands()
    {
        var validCommand = new CreateComponentCommand("Name", Guid.NewGuid());
        yield return new object[] { validCommand };
    }
}