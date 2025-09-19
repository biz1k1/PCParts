using AutoMapper;
using FluentAssertions;
using Moq;
using PCParts.Application.Abstraction.Storage;
using PCParts.Application.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.CategoryService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;
using PCParts.Domain.Specification.Category;
using PCParts.Storage.Mapping;

namespace PCParts.Application.Tests.Services;

public class CategoryServiceShould
{
    private readonly Mock<ICategoryStorage> _storage;
    private readonly CategoryService _sut;
    private readonly Mock<IValidationService> _validator;
    private readonly Mapper _mapper;


    public CategoryServiceShould()
    {
        _storage = new Mock<ICategoryStorage>();
        _validator = new Mock<IValidationService>();
        _mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile(new StorageProfile())));

        _sut = new CategoryService(_storage.Object, _validator.Object, _mapper);
    }

    [Fact]
    public async Task ReturnCategories_FromStorage()
    {
        var domainCategories = new List<Domain.Entities.Category>
        {
            new() { Id = Guid.Parse("2adf2542-8c22-4562-95b0-501701d91c90"), Name = "Category1" },
            new() { Id = Guid.Parse("c8ac8afa-3acf-4153-9df1-dc0912963596"), Name = "Category2" }
        };

        var applicationCategory = _mapper.Map<IEnumerable<Category>>(domainCategories);

        var returnCategoriesSetup = _storage.Setup(x =>
            x.GetCategories(It.IsAny<CategoryWithComponentSpec>(), It.IsAny<CancellationToken>()));
        returnCategoriesSetup.ReturnsAsync(domainCategories);

        var actual = await _sut.GetCategories(CancellationToken.None);
        actual.Should().BeEquivalentTo(applicationCategory);
        _storage.Verify(x => x.GetCategories(It.IsAny<CategoryWithComponentSpec>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ReturnCategory_FromStorage()
    {
        var domainCategory = new Domain.Entities.Category
        {
            Id = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"),
            Name = "name"
        };

        var applicationCategory = _mapper.Map<Category>(domainCategory);

        var getCategorySetup = _storage.Setup(x =>
            x.GetCategory(It.IsAny<Guid>(), It.IsAny<CategoryWithComponentSpec>(), CancellationToken.None));
        getCategorySetup.ReturnsAsync(domainCategory);

        var actual = await _sut.GetCategory(domainCategory.Id, CancellationToken.None);
        actual.Should().BeEquivalentTo(applicationCategory);
        _storage.Verify(x => x.GetCategory(domainCategory.Id, It.IsAny<CategoryWithComponentSpec>(), CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ReturnCreatedCategory()
    {
        var categoryCommand = new CreateCategoryCommand("name");

        var domainCategory = new Domain.Entities.Category
        {
            Id = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"),
            Name = categoryCommand.Name
        };

        var applicationCategory = _mapper.Map<Category>(domainCategory);


        var createCategorySetup = _storage.Setup(x =>
            x.CreateCategory(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        createCategorySetup.ReturnsAsync(domainCategory);

        var actual = await _sut.CreateCategory(categoryCommand, CancellationToken.None);
        actual.Should().BeEquivalentTo(applicationCategory);
        _storage.Verify(x => x.CreateCategory(categoryCommand.Name, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ReturnUpdatedCategory()
    {
        var command = new UpdateCategoryCommand(Guid.Parse("2adf2542-8c22-4562-95b0-501701d91c90"), "name");

        var domainCategory = new Domain.Entities.Category { Id = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"), Name = "name" };

        var applicationCategory = _mapper.Map<Category>(domainCategory);

        var updateCategorySetup = _storage.Setup(x =>
            x.UpdateCategory(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<CancellationToken>()));

        updateCategorySetup.ReturnsAsync(domainCategory);
        var getCategorySetup = _storage.Setup(x =>
            x.GetCategory(It.IsAny<Guid>(), null, CancellationToken.None));
        getCategorySetup.ReturnsAsync(domainCategory);

        var actual = await _sut.UpdateCategory(command, CancellationToken.None);
        actual.Should().BeEquivalentTo(applicationCategory);
        _storage.Verify(x => x.UpdateCategory(command.Id, command.Name, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ThrowCategoryNotFoundException_WhenUpdatedCategory_IfCategoryIsNull()
    {
        var categoryId = new UpdateCategoryCommand(Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae"), "title");

        await _sut.Invoking(x => x.UpdateCategory(categoryId, CancellationToken.None))
            .Should().ThrowAsync<EntityNotFoundException>();
    }

    [Fact]
    public async Task ThrowCategoryNotFoundException_WhenRemoveCategory_IfCategoryIsNull()
    {
        var categoryId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

        await _sut.Invoking(x => x.RemoveCategory(categoryId, CancellationToken.None))
            .Should().ThrowAsync<EntityNotFoundException>();
    }
}

