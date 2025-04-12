using FluentValidation.TestHelper;
using PCParts.Application.Model.Command;
using PCParts.Application.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;

namespace PCParts.Application.Tests.Validation
{
    public class UpdateSpecificationValueCommandValidatorShould
    {
        private readonly UpdateSpecificationValueCommandValidator _sut;

        public UpdateSpecificationValueCommandValidatorShould()
        {
            _sut = new UpdateSpecificationValueCommandValidator();
        }

        [Theory]
        [MemberData(nameof(GetValidCommands))]
        public async Task ReturnSuccess_WhenCommandValid(UpdateSpecificationValueCommand command)
        {
            var result = await _sut.TestValidateAsync(command);

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetInvalidCommands))]
        public async Task ReturnFalse_WhenCommandIsInvalid(UpdateSpecificationValueCommand command)
        {
            var result = await _sut.TestValidateAsync(command);

            result.IsValid.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetInvalidCommands()
        {
            var validCommand = new UpdateSpecificationValueCommand(Guid.NewGuid(), "Specification");
            var outOfLength = "A".PadRight(51, 'A');

            yield return new object[] { validCommand with { Id = Guid.Empty } };
            yield return new object[] { validCommand with { Value = string.Empty } };
            yield return new object[] { validCommand with { Value = outOfLength } };
        }

        public static IEnumerable<object[]> GetValidCommands()
        {
            var validCommand = new UpdateSpecificationValueCommand(Guid.NewGuid(), "Specification");
            yield return new object[] { validCommand };
        }
    }
}
