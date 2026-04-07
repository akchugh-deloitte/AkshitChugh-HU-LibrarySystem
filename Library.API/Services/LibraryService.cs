using Microsoft.EntityFrameworkCore;
using Library.Core.Entities;
using Library.Infra.Data;

namespace Library.API.Services
{
    public class LibraryService
    {
        private readonly AppDbContext _context;
        public LibraryService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<object> IssueBook(Guid userId, Guid bookId)
        {
            var issueBook = new BookIssue
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                BookId = bookId,
                IssueDate = DateTime.UtcNow
            };

            _context.BookIssues.Add(issueBook);
            await _context.SaveChangesAsync();

            var book = await _context.Books.FindAsync(bookId);
            if(book != null && book.IsAvailable)
            {
                book.IsAvailable = false;
                await _context.SaveChangesAsync();
            }

            _context.Logs.Add(new Log
            {
                Id = Guid.NewGuid(),
                Message = $"Book with ID {bookId} issued to user with ID {userId}",
                CreatedAt = DateTime.UtcNow
            });

            await _context.SaveChangesAsync();
            return issueBook;
        }

        public Book GetBookFromCache(Guid bookId)
        {
            // todo
            return null;
        }
    }
}
