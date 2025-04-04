using FluentAssertions;
using Microsoft.QualityTools.Testing.Fakes;
using Microsoft.QualityTools.Testing.Fakes.Shims;
using PCParts.Application.Helpers;
using PCParts.Application.Model.Command;
using PCParts.Domain.Enum;
using PCParts.Domain.Exceptions;
using PCParts.Application.Model.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PCParts.Application.Tests.Helpers
{
    public class ValidationHelperShould
    {
        [Fact]
        public async Task ReturnFalse_When_ValidationValid()
        {
            var result = ValidationHelper.IsValueValid(SpecificationDataType.INT, "text");
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ReturnTrue_When_ValidationInvalid()
        {
            var result = ValidationHelper.IsValueValid(SpecificationDataType.STRING, "text");
            result.Should().BeTrue();
        }
    }
}
