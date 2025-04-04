using FluentAssertions;
using Moq;
using PCParts.Application.AbstractionStorage;
using PCParts.Application.Model.Command;
using PCParts.Application.Model.Models;
using PCParts.Application.Services.CategoryService;
using PCParts.Application.Services.ValidationService;
using PCParts.Domain.Exceptions;

namespace PCParts.Application.Tests.Services;

public class CategoryServiceShould
{
    private readonly Mock<ICategoryStorage> _storage;
    private readonly ICategoryService _sut;
    private readonly Mock<IValidationService> _validator;


    public CategoryServiceShould()
    {
        _storage = new Mock<ICategoryStorage>();
        _validator = new Mock<IValidationService>();

        _sut = new CategoryService(_storage.Object, _validator.Object);
    }

    [Fact]
    public async Task ReturnCategories_FromStorage()
    {
        var categories = new Category[]
        {
            new() { Id = Guid.Parse("2adf2542-8c22-4562-95b0-501701d91c90"), Name = "Category1" },
            new() { Id = Guid.Parse("c8ac8afa-3acf-4153-9df1-dc0912963596"), Name = "Category2" }
        };

        var returnCategoriesSetup = _storage.Setup(x =>
            x.GetCategories(It.IsAny<CancellationToken>()));
        returnCategoriesSetup.ReturnsAsync(categories);

        var actual = await _sut.GetCategories(CancellationToken.None);
        actual.Should().BeSameAs(categories);
        _storage.Verify(x => x.GetCategories(CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ReturnCategory_FromStorage()
    {
        var category = new Category { Id = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"), Name = "name" };
        var command = new UpdateCategoryCommand(Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"), "name");

        var getCategorySetup = _storage.Setup(x => 
            x.GetCategory(It.IsAny<Guid>(), CancellationToken.None));
        getCategorySetup.ReturnsAsync(category);

        var actual = await _sut.GetCategory(category.Id, CancellationToken.None);
        actual.Should().BeSameAs(category);
        _storage.Verify(x => x.GetCategory(category.Id, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ReturnCreatedCategory()
    {
        var categoryCommand = new CreateCategoryCommand("name");
        var category = new Category
        {
            Id = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"),
            Name = categoryCommand.Name
        };

        var createCategorySetup = _storage.Setup(x =>
            x.CreateCategory(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        createCategorySetup.ReturnsAsync(category);

        var actual = await _sut.CreateCategory(categoryCommand, CancellationToken.None);
        actual.Should().BeSameAs(category);
        _storage.Verify(x => x.CreateCategory(categoryCommand.Name, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ReturnUpdatedCategory()
    {
        var category = new Category { Id = Guid.Parse("a5186d90-49f9-4baf-928b-b2ad117df55a"), Name = "name" };
        var command = new UpdateCategoryCommand(Guid.Parse("2adf2542-8c22-4562-95b0-501701d91c90"), "name");

        var updateCategorySetup = _storage.Setup(x =>
            x.UpdateCategory(It.IsAny<UpdateCategoryCommand>(), It.IsAny<CancellationToken>()));
        updateCategorySetup.ReturnsAsync(category);
        var getCategorySetup = _storage.Setup(x =>
            x.GetCategory(It.IsAny<Guid>(), CancellationToken.None));
        getCategorySetup.ReturnsAsync(category);

        var actual = await _sut.UpdateCategory(command, CancellationToken.None);
        actual.Should().BeSameAs(category);
        _storage.Verify(x => x.UpdateCategory(command, CancellationToken.None), Times.Once);
    }

    [Fact]
    public async Task ThrowCategoryNotFoundException_WhenUpdatedCategory_IfCategoryIsNull()
    {
        var categoryId = new UpdateCategoryCommand(Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae"),"title");

        await _sut.Invoking(x => x.UpdateCategory(categoryId, CancellationToken.None))
            .Should().ThrowAsync<CategoryNotFoundException>();
    }

    [Fact]
    public async Task ThrowCategoryNotFoundException_WhenRemoveCategory_IfCategoryIsNull()
    {
        var categoryId = Guid.Parse("0bf6ed66-f924-4372-8d93-14acdb1c3fae");

        await _sut.Invoking(x => x.RemoveCategory(categoryId, CancellationToken.None))
            .Should().ThrowAsync<CategoryNotFoundException>();
    }
}