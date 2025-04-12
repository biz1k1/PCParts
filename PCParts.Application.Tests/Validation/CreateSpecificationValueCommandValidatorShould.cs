﻿using FluentValidation.TestHelper;
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
    public class CreateSpecificationValueCommandValidatorShould
    {
        private readonly CreateSpecificationValueCommandValidator _sut;

        public CreateSpecificationValueCommandValidatorShould()
        {
            _sut = new CreateSpecificationValueCommandValidator();
        }

        [Theory]
        [MemberData(nameof(GetValidCommands))]
        public async Task ReturnSuccess_WhenCommandIsValid(CreateSpecificationValueCommand command)
        {
            var result = await _sut.TestValidateAsync(command);

            result.IsValid.Should().BeTrue();
        }

        [Theory]
        [MemberData(nameof(GetInvalidCommands))]
        public async Task ReturnFalse_WhenCommandIsInvalid(CreateSpecificationValueCommand command)
        {
            var result = await _sut.TestValidateAsync(command);

            result.IsValid.Should().BeFalse();
        }

        public static IEnumerable<object[]> GetInvalidCommands()
        {
            var validCommand = new CreateSpecificationValueCommand(Guid.NewGuid(), Guid.NewGuid(), "Value");
            var outOfLength = "A".PadRight(51, 'A');

            yield return new object[] { validCommand with { componentId = Guid.Empty } };
            yield return new object[] { validCommand with { specificationId = Guid.Empty } };
            yield return new object[] { validCommand with { componentId = Guid.Empty, specificationId = Guid.Empty } };
            yield return new object[] { validCommand with { value = String.Empty} };
            yield return new object[] { validCommand with { value = outOfLength } };
        }

        public static IEnumerable<object[]> GetValidCommands()
        {
            var validCommand = new CreateSpecificationValueCommand(Guid.NewGuid(), Guid.NewGuid(), "Value");
            yield return new object[] { validCommand };
        }
    }
}
