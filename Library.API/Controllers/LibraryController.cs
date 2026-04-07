using Microsoft.AspNetCore.Mvc;
using Library.Core.Interfaces;

namespace Library.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly IBookService _bookService;

        public LibraryController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpPost("issue")]
        public async Task<IActionResult> IssueBook(Guid bookId, Guid userId)
        {
            var result = await _bookService.IssueBookAsync(bookId, userId);
            if (result)
            {
                return Ok(new { Message = "Book issued successfully." });
            }
            else
            {
                return BadRequest(new { Message = "Failed to issue the book. It may not be available." });
            }
        }

        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableBooks()
        {
            var books = await _bookService.GetAllBooksAsync();
            return Ok(books);
        }
    }
}
