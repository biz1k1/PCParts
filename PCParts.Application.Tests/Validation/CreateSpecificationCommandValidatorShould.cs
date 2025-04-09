using FluentAssertions;
using FluentValidation.TestHelper;
using PCParts.Application.Model.Command;
using PCParts.Application.Validation;
using PCParts.Domain.Enum;

namespace PCParts.Application.Tests.Validation;

public class CreateSpecificationCommandValidatorShould
{
    private readonly CreateSpecificationCommandValidator _sut;

    public CreateSpecificationCommandValidatorShould()
    {
        _sut = new CreateSpecificationCommandValidator();
    }

    //[Theory]
    //[MemberData(nameof(GetValidCommands))]
    //public async Task ReturnSuccess_WhenCommandIsValid(CreateSpecificationCommand command)
    //{
    //    var result = await _sut.TestValidateAsync(command);

    //    result.IsValid.Should().BeTrue();
    //}

    //[Theory]
    //[MemberData(nameof(GetInvalidCommands))]
    //public async Task ReturnFalse_WhenCommandIsInvalid(CreateSpecificationCommand command)
    //{
    //    var result = await _sut.TestValidateAsync(command);

    //    result.IsValid.Should().BeFalse();
    //}

    //public static IEnumerable<object[]> GetInvalidCommands()
    //{
    //    var validCommand =
    //        new CreateSpecificationCommand(Guid.NewGuid(), "Specification", "value", SpecificationDataType.STRING);
    //    var outOfLength = "A".PadRight(101, 'A');

    //    yield return new object[] { validCommand with { ComponentId = Guid.Empty } };
    //    yield return new object[] { validCommand with { Name = string.Empty } };
    //    yield return new object[] { validCommand with { Name = outOfLength } };
    //    yield return new object[] { validCommand with { Value = null } };
    //    yield return new object[] { validCommand with { Value = outOfLength } };
    //    yield return new object[] { validCommand with { Value = null, DataType = SpecificationDataType.INT } };
    //}

    //public static IEnumerable<object[]> GetValidCommands()
    //{
    //    var validCommand =
    //        new CreateSpecificationCommand(Guid.NewGuid(), "Specification", "value", SpecificationDataType.STRING);
    //    yield return new object[] { validCommand };
    //}
}