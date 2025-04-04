using FluentAssertions;
using FluentValidation.TestHelper;
using PCParts.Application.Model.Command;
using PCParts.Application.Validation;

namespace PCParts.Application.Tests.Validation;

public class UpdateCategoryCommandValidatorShould
{
    private readonly UpdateCategoryCommandValidator _sut;

    public UpdateCategoryCommandValidatorShould()
    {
        _sut = new UpdateCategoryCommandValidator();
    }

    [Theory]
    [MemberData(nameof(GetValidCommands))]
    public async Task ReturnSuccess_WhenCommandIsValid(UpdateCategoryCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFalse_WhenCommandInvalid(UpdateCategoryCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new UpdateCategoryCommand(Guid.NewGuid(), "Name");
        var outOfLength = "A".PadRight(101, 'A');

        yield return new object[] { validCommand with { Id = Guid.Empty } };
        yield return new object[] { validCommand with { Name = string.Empty } };
        yield return new object[] { validCommand with { Name = outOfLength } };
    }

    public static IEnumerable<object[]> GetValidCommands()
    {
        var validCommand = new UpdateCategoryCommand(Guid.NewGuid(), "Name");
        yield return new object[] { validCommand };
    }
}