using FluentAssertions;
using FluentValidation.TestHelper;
using PCParts.Application.Command;
using PCParts.Application.Validation;

namespace PCParts.Application.Tests.Validation;

public class UpdateComponentCommandValidatorShould
{
    private readonly UpdateComponentCommandValidator _sut;

    public UpdateComponentCommandValidatorShould()
    {
        _sut = new UpdateComponentCommandValidator();
    }

    [Theory]
    [MemberData(nameof(GetValidCommands))]
    public async Task ReturnSuccess_WhenCommandValid(UpdateComponentCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFalse_WhenCommandInvalid(UpdateComponentCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new UpdateComponentCommand(Guid.NewGuid(), "Name");
        var outOfLength = "A".PadRight(51, 'A');

        yield return new object[] { validCommand with { Id = Guid.Empty } };
        yield return new object[] { validCommand with { Name = outOfLength } };
    }

    public static IEnumerable<object[]> GetValidCommands()
    {
        var validCommand = new UpdateComponentCommand(Guid.NewGuid(), "Name");
        yield return new object[] { validCommand };
        yield return new object[] { validCommand with { Name = string.Empty } };
    }
}