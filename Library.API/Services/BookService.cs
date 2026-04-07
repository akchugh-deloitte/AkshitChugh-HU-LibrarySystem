using Library.Core.Entities;
using Library.Core.Interfaces;

namespace Library.API.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly ILoggingService _loggingService;

        public BookService(IBookRepository bookRepository, IIssueRepository issueRepository, ILoggingService loggingService)
        {
            _bookRepository = bookRepository;
            _issueRepository = issueRepository;
            _loggingService = loggingService;
        }

        public async Task<Book?> GetBookByIdAsync(Guid bookId)
        {
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book == null)
            {
                await _loggingService.LogInfoAsync($"Book with ID {bookId} not found.");
                return null;
            }
            return book;
        }

        public async Task<bool> IssueBookAsync(Guid bookId, Guid userId)
        {
            if (!await _bookRepository.IsAvailableAsync(bookId))
            {
                await _loggingService.LogInfoAsync($"Book with ID {bookId} is not available for issue.");
                return false;
            }
            var existingIssue = await _issueRepository.GetActiveIssueByBookAsync(bookId);
            if (existingIssue != null)
            {
                return false;
            }

            var issue = new BookIssue
            {
                Id = Guid.NewGuid(),
                BookId = bookId,
                UserId = userId,
                IssueDate = DateTime.UtcNow
            };

            await _issueRepository.AddAsync(issue);
            var book = await _bookRepository.GetByIdAsync(bookId);
            if (book != null)
            {
                book.IsAvailable = false;
                await _bookRepository.UpdateAsync(book);
            }

            await _loggingService.LogInfoAsync($"Book with ID {bookId} issued successfully.");
            return true;
        }

        public async Task<List<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }
    }
}