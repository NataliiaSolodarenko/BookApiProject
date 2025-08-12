using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BookApiProject.Controllers;
using BookApiProject.AuthorDTOs;
using BookApiProject.Services;
using BookApiProject.Exceptions;
namespace BookApiProject.Tests;

public class AuthorsControllerTests
{
    [Fact]
    public async Task GetAllAsync_ShouldReturnOkWithAuthors()
    {
        var mockService = new Mock<IAuthorService>();
        mockService
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(new List<AuthorReadDto>
            {
                new AuthorReadDto {Id = 1, FirstName = "Test Author"}
            });

        var controller = new AuthorsController(mockService.Object);

        var actionResult = await controller.GetAllAsync();

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var authors = Assert.IsType<List<AuthorReadDto>>(okResult.Value);
        Assert.Single(authors);
        Assert.Equal("Test Author", authors[0].FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnOkWithAuthor()
    {
        var mockService = new Mock<IAuthorService>();
        mockService
            .Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync(
                new AuthorReadDto { Id = 1, FirstName = "Test Author" }
            );

        var controller = new AuthorsController(mockService.Object);

        var actionResult = await controller.GetByIdAsync(1);

        var okResult = Assert.IsType<OkObjectResult>(actionResult.Result);
        var author = Assert.IsType<AuthorReadDto>(okResult.Value);
        Assert.Equal("Test Author", author.FirstName);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNothing()
    {
        var mockService = new Mock<IAuthorService>();
        mockService
            .Setup(s => s.GetByIdAsync(10))
            .ThrowsAsync(new AuthorNotFoundException(10));

        var controller = new AuthorsController(mockService.Object);

        await Assert.ThrowsAsync<AuthorNotFoundException>(() => controller.GetByIdAsync(10));
    }

    [Fact]
    public async Task CreateAsync_ShouldReturnCreatedWithAuthor()
    {
        var mockService = new Mock<IAuthorService>();
        var newAuthor = new AuthorCreateDto { FirstName = "New Author" };
        mockService
            .Setup(s => s.CreateAsync(It.Is<AuthorCreateDto>(dto => dto.FirstName == "New Author")))
            .ReturnsAsync(new AuthorReadDto { Id = 2, FirstName = "New Author" });

        var controller = new AuthorsController(mockService.Object);

        var result = await controller.CreateAsync(newAuthor);

        var createdResult = Assert.IsType<CreatedResult>(result);
        var returnedDto = Assert.IsType<AuthorReadDto>(createdResult.Value);
        Assert.Equal(2, returnedDto.Id);
        Assert.Equal("New Author", returnedDto.FirstName);
        Assert.Equal("/api/authors/2", createdResult.Location);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNoContent()
    {
        var mockService = new Mock<IAuthorService>();
        mockService
            .Setup(s => s.UpdateAsync(1, It.Is<AuthorUpdateDto>(dto => dto.FirstName == "New Author")))
            .ReturnsAsync(true);

        var controller = new AuthorsController(mockService.Object);

        var result = await controller.UpdateAsync(1, new AuthorUpdateDto { FirstName = "Updated Author" });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNothing()
    {
        var mockService = new Mock<IAuthorService>();
        mockService
            .Setup(s => s.UpdateAsync(10, It.Is<AuthorUpdateDto>(dto => dto.FirstName == "Updated Author")))
            .ThrowsAsync(new AuthorNotFoundException(10));

        var controller = new AuthorsController(mockService.Object);

        await Assert.ThrowsAsync<AuthorNotFoundException>(() => controller.UpdateAsync(10, new AuthorUpdateDto { FirstName = "Updated Author" }));
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNoContent()
    {
        var mockService = new Mock<IAuthorService>();
        mockService
            .Setup(s => s.DeleteAsync(1))
            .ReturnsAsync(true);

        var controller = new AuthorsController(mockService.Object);

        var result = await controller.DeleteAsync(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnNothing()
    {
        var mockService = new Mock<IAuthorService>();
        mockService
            .Setup(s => s.DeleteAsync(10))
            .ThrowsAsync(new AuthorNotFoundException(10));

        var controller = new AuthorsController(mockService.Object);

        await Assert.ThrowsAsync<AuthorNotFoundException>(() => controller.DeleteAsync(10));
    }
}