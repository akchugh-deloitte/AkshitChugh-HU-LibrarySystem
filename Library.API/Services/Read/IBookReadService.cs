using Library.Core.DTO;

namespace Library.API.Services.Read
{
    public interface IBookReadService
    {
        Task<PagedResult<BookDto>> GetAvailableBooksAsync(BookFilter filter);
        Task<BookDto?> GetBookByIdAsync(Guid id);
        Task<List<BookDto>> GetBooksByAuthorAsync(string author);
    }
}
