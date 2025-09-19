using AutoMapper;
using FluentAssertions;
using PCParts.Application.Services.SpecificationValueService;
using Moq;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;
using PCParts.Application.Command;
using PCParts.Domain.Specification.Component;
using PCParts.Domain.Specification.SpecificationValue;
using PCParts.Storage.Mapping;

namespace PCParts.Application.Tests.Services
{
    public class SpecificationValueServiceShould
    {
        private readonly SpecificationValueService _sut;
        private readonly Mock<IComponentStorage> _componentStorage;
        private readonly Mock<ISpecificationValueStorage> _specificationValueStorage;
        private readonly Mock<IValidationService> _validator;
        private readonly Mapper _mapper;

        public SpecificationValueServiceShould()
        {
            _componentStorage = new Mock<IComponentStorage>();
            _specificationValueStorage = new Mock<ISpecificationValueStorage>();
            _validator = new Mock<IValidationService>();
            _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new StorageProfile())));

            _sut = new SpecificationValueService(
                _componentStorage.Object,
                _specificationValueStorage.Object,
                _validator.Object,
                _mapper
            );
        }

        [Fact]
        public async Task ReturnCreatedSpecificationValue()
        {
            var componentId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
            var specificationValueId = Guid.Parse("d8c4180f-cccc-4c93-8352-448079b77cd0");
            var commands = new CreateSpecificationValueCommand[] {
                new(specificationValueId, "name")
                {
                    Id = specificationValueId
                }};

            var domainComponent = new Domain.Entities.Component()
            {
                Id = componentId
            };

            var domainSpecificationValue = new Domain.Entities.SpecificationValue()
            {
                Id = specificationValueId,
            };

            var applicationSpecificationValue = _mapper.Map<SpecificationValue>(domainSpecificationValue);

            var getComponentSetup = _componentStorage.Setup(x =>
                x.GetComponent(It.IsAny<Guid>(), It.IsAny<ComponentWithSpecificationValueWithSpecificationSpec>(), CancellationToken.None));
            getComponentSetup.ReturnsAsync(domainComponent);
            var getCreatedSpecification = _specificationValueStorage.Setup(x =>
                x.CreateSpecificationValue(It.IsAny<Guid>(), It.IsAny<IEnumerable<Domain.Entities.SpecificationValue>>(), CancellationToken.None));
            getCreatedSpecification.ReturnsAsync(domainSpecificationValue);

            var actual = await _sut.CreateSpecificationsValues(componentId, commands, CancellationToken.None);
            actual.Should().BeEquivalentTo(applicationSpecificationValue);
            _componentStorage.Verify(x => x.GetComponent(componentId, It.IsAny<ComponentWithSpecificationValueWithSpecificationSpec>(), CancellationToken.None));

            _specificationValueStorage.Verify(x => x.CreateSpecificationValue(componentId, It.IsAny<IEnumerable<Domain.Entities.SpecificationValue>>(), CancellationToken.None));
        }

        [Fact]
        public async Task ThrowComponentNotFoundException_WhenCreateSpecificationValue_IfComponentIsNull()
        {
            await _sut.Invoking(x =>
                    x.CreateSpecificationsValues(Guid.Empty, new List<CreateSpecificationValueCommand>()
                        { new (Guid.Empty, null) }, CancellationToken.None))
                .Should().ThrowAsync<EntityNotFoundException>();
        }

        [Fact]
        public async Task ThrowSpecificationValueNotFoundException_WhenUpdateSpecificationValue_IfSpecificationIsNull()
        {
            var domainComponent = new Domain.Entities.Component();

            var getComponentSetup = _componentStorage.Setup(x =>
                x.GetComponent(It.IsAny<Guid>(), It.IsAny<ComponentWithSpecificationValueWithSpecificationSpec>(), CancellationToken.None));

            getComponentSetup.ReturnsAsync(domainComponent);

            await _sut.Invoking(x =>
                    x.UpdateSpecificationValue(new UpdateSpecificationValueCommand(Guid.Empty, null), CancellationToken.None))
                .Should().ThrowAsync<EntityNotFoundException>();
        }
        [Fact]
        public async Task ThrowInvalidSpecificationTypeException_WhenUpdateSpecificationValue_IfCommandTypeInvalid()
        {
            var specificationValueId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
            var specificationValue = new Domain.Entities.SpecificationValue()
            {
                Id = specificationValueId,
                Value = "title",
                Specification = new Domain.Entities.Specification()
                {
                    DataType = 0,
                }
            };

            var getComponentSetup = _specificationValueStorage.Setup(x =>
                x.GetSpecificationValue(It.IsAny<Guid>(), It.IsAny<SpecificationValueWithSpecificationSpec>(), CancellationToken.None));
            getComponentSetup.ReturnsAsync(specificationValue);

            await _sut.Invoking(x =>
                    x.UpdateSpecificationValue(new UpdateSpecificationValueCommand(specificationValueId, "title"), CancellationToken.None))
                .Should().ThrowAsync<InvalidSpecificationTypeException>();
        }
    }
}

