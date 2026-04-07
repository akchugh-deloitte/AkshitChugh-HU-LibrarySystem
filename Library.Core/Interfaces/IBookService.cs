using System;
using System.Collections.Generic;
using System.Text;
using Library.Core.Entities;

namespace Library.Core.Interfaces
{
    public interface IBookService
    {
        Task<Book?> GetBookByIdAsync(Guid id);
        Task<bool> IssueBookAsync(Guid userId, Guid bookId);
        Task<List<Book>> GetAllBooksAsync();
    }
}
