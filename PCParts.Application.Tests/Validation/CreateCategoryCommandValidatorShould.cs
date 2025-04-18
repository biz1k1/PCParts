using FluentAssertions;
using FluentValidation.TestHelper;
using PCParts.Application.Command;
using PCParts.Application.Validation;

namespace PCParts.Application.Tests.Validation;

public class CreateCategoryCommandValidatorShould
{
    private readonly CreateCategoryCommandValidator _sut;

    public CreateCategoryCommandValidatorShould()
    {
        _sut = new CreateCategoryCommandValidator();
    }

    [Fact]
    public async Task ReturnSuccess_WhenCommandValid()
    {
        var result = await _sut.TestValidateAsync(new CreateCategoryCommand("name"));

        result.IsValid.Should().BeTrue();
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public async Task ReturnFalse_WhenCommandInvalid(CreateCategoryCommand command)
    {
        var result = await _sut.TestValidateAsync(command);

        result.IsValid.Should().BeFalse();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new CreateCategoryCommand("Name");
        var outOfLength = "A".PadRight(51, 'A');

        yield return new object[] { validCommand with { Name = string.Empty } };
        yield return new object[] { validCommand with { Name = outOfLength } };
    }
}