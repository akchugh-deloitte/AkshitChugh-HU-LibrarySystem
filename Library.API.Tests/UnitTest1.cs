using Moq;
using Library.Core.Entities;
using Library.Core.Interfaces;
using Library.API.Services;

namespace Library.API.Tests;

public class UnitTest1
{
    [Fact]
    public async Task IssueBookAsync_WhenAvailable()
    {
        var bookId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        var book = new Book { Id = bookId, Title="test", IsAvailable = true };
        var mockBook = new Mock<IBookRepository>();
        var mockIssue = new Mock<IIssueRepository>();
        var mockLogger = new Mock<ILoggingService>();

        mockBook.Setup(repo => repo.GetByIdAsync(bookId)).ReturnsAsync(book);
        mockBook.Setup(repo => repo.IsAvailableAsync(bookId)).ReturnsAsync(true);
        mockBook.Setup(repo => repo.UpdateAsync(It.IsAny<Book>())).Returns(Task.CompletedTask);

        var service = new BookService(mockBook.Object, mockIssue.Object, mockLogger.Object);

        var result = await service.IssueBookAsync(bookId, userId);
        Assert.True(result);
        Assert.False(book.IsAvailable);
    }
}
