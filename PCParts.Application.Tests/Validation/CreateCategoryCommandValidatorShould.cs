using FluentAssertions;
using FluentValidation.TestHelper;
using PCParts.Application.Model.Command;
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

    [Fact]
    public async Task ReturnFalse_WhenCommandInvalid()
    {
        var result = await _sut.TestValidateAsync(new CreateCategoryCommand(null));

        result.IsValid.Should().BeFalse();
    }
}