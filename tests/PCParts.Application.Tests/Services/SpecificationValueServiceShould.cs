//using FluentAssertions;
//using PCParts.Application.Services.SpecificationValueService;
//using Moq;
//using PCParts.Application.Model.Models;
//using PCParts.Application.Services.ValidationService;
//using PCParts.Application.Services.QueryBuilderService;
//using PCParts.Domain.Exceptions;
//using PCParts.Application.Abstraction;
//using PCParts.Application.Command;
//using PCParts.Application.Storages;

//namespace PCParts.Application.Tests.Services
//{
//    public class SpecificationValueServiceShould
//    {
//        private readonly ISpecificationValueService _sut;
//        private readonly Mock<IComponentStorage> _componentStorage;
//        private readonly Mock<ISpecificationValueStorage> _specificationValueStorage;
//        private readonly Mock<IValidationService> _validator;
//        private readonly Mock<IQueryBuilderService> _queryBuilder;
//        private readonly Mock<ISpecificationStorage> _specificationStorage;

//        public SpecificationValueServiceShould()
//        {
//            _componentStorage = new Mock<IComponentStorage>();
//            _specificationValueStorage = new Mock<ISpecificationValueStorage>();
//            _validator = new Mock<IValidationService>();
//            _queryBuilder = new Mock<IQueryBuilderService>();
//            _specificationStorage = new Mock<ISpecificationStorage>();

//            _sut = new SpecificationValueService(
//                _componentStorage.Object,
//                _specificationValueStorage.Object,
//                _validator.Object,
//                _queryBuilder.Object,
//                _specificationStorage.Object
//            );
//        }

//        [Fact]
//        public async Task ReturnCreatedSpecificationValue()
//        {
//            var componentId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");
//            var specificationId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
//            var specificationValueId = Guid.Parse("d8c4180f-cccc-4c93-8352-448079b77cd0");
//            var commands = new CreateSpecificationValueCommand[] { };
//            var component = new Component()
//            {
//                Id = componentId
//            };
//            var specification = new Specification()
//            {
//                Id = specificationId
//            };
//            var specificationValue = new SpecificationValue()
//            {
//                Id = specificationValueId,
//            };

//            var getComponentSetup = _componentStorage.Setup(x =>
//                x.GetComponent(It.IsAny<Guid>(), CancellationToken.None));
//            getComponentSetup.ReturnsAsync(component);
//            var getCreatedSpecification = _specificationValueStorage.Setup(x =>
//                x.CreateSpecificationValue(It.IsAny<Guid>(), It.IsAny<CreateSpecificationValueCommand[]>(), CancellationToken.None));
//            getCreatedSpecification.ReturnsAsync(specificationValue);

//            var actual = await _sut.CreateSpecificationsValues(componentId, commands, CancellationToken.None);
//            actual.Should().Be(specificationValue);
//            _componentStorage.Verify(x => x.GetComponent(componentId, CancellationToken.None));

//            _specificationValueStorage.Verify(x => x.CreateSpecificationValue(componentId, commands, CancellationToken.None));
//        }

//        [Fact]
//        public async Task ThrowComponentNotFoundException_WhenCreateSpecificationValue_IfComponentIsNull()
//        {
//            await _sut.Invoking(x =>
//                    x.CreateSpecificationsValues(Guid.Empty, new List<CreateSpecificationValueCommand>() { new CreateSpecificationValueCommand(Guid.Empty, null) }, CancellationToken.None))
//                .Should().ThrowAsync<EntityNotFoundException>();
//        }

//        [Fact]
//        public async Task ThrowSpecificationValueNotFoundException_WhenUpdateSpecificationValue_IfSpecificationIsNull()
//        {
//            var component = new Component();

//            var getComponentSetup = _componentStorage.Setup(x =>
//                x.GetComponent(It.IsAny<Guid>(), CancellationToken.None));
//            getComponentSetup.ReturnsAsync(component);

//            await _sut.Invoking(x =>
//                    x.UpdateSpecificationValue(new UpdateSpecificationValueCommand(Guid.Empty, null), CancellationToken.None))
//                .Should().ThrowAsync<EntityNotFoundException>();
//        }
//        [Fact]
//        public async Task ThrowInvalidSpecificationTypeException_WhenUpdateSpecificationValue_IfCommandTypeInvalid()
//        {
//            var specificationValueId = Guid.Parse("333357ef-a6ed-40ec-ae5f-44383f00ca17");
//            var specificationValue = new SpecificationValue()
//            {
//                Id = specificationValueId,
//                Value = "title",
//                Specification = new Specification()
//                {
//                    Type = 0,
//                }
//            };

//            var getComponentSetup = _specificationValueStorage.Setup(x =>
//                x.GetSpecificationValue(It.IsAny<Guid>(), It.IsAny<string[]>(), CancellationToken.None));
//            getComponentSetup.ReturnsAsync(specificationValue);

//            await _sut.Invoking(x =>
//                    x.UpdateSpecificationValue(new UpdateSpecificationValueCommand(specificationValueId, "title"), CancellationToken.None))
//                .Should().ThrowAsync<InvalidSpecificationTypeException>();
//        }
//    }
//}

