using Microsoft.AspNetCore.Mvc;
using Library.Core.Interfaces;
using Library.API.Services.Read;
using Library.Core.DTO;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookReadService _bookService;
        private readonly ICacheService _cacheService;

        public BookController(IBookReadService bookService, ICacheService cacheService)
        {
            _bookService = bookService;
            _cacheService = cacheService;
        }

        [HttpGet("avaible")]
        public async Task<IActionResult> GetAvailableBooks(
            [FromQuery] string? search,
            [FromQuery] string? author,
            [FromQuery] int? year,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var filter = new BookFilter
            {
                Search = search,
                Author = author,
                Year = year,
                Page = page,
                PageSize = pageSize
            };

            var cacheKey = $"available_books_{search}_{author}_{year}_{page}_{pageSize}";
            var cacheResult = _cacheService.Get<PagedResult<BookDto>>(cacheKey);
            if (cacheResult != null)
            {
                return Ok(cacheResult);
            }
            var result = await _bookService.GetAvailableBooksAsync(filter);
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(5));
            return Ok(result);
        }

        [HttpGet("author/{author}")]
        public async Task<IActionResult> GetBooksByAuthor(string author)
        {
            var cacheKey = $"books_by_author_{author}";
            var cacheResult = _cacheService.Get<PagedResult<BookDto>>(cacheKey);
            if (cacheResult != null)
            {
                return Ok(cacheResult);
            }
            var result = await _bookService.GetBooksByAuthorAsync(author);
            _cacheService.Set(cacheKey, result, TimeSpan.FromMinutes(5));
            return Ok(result);
        }
    }
        }
