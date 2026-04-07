using Library.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.Interfaces
{
    public interface IBookRepository
    {
        Task<Book?> GetByIdAsync(Guid id);  
        Task<List<Book>> GetAllAsync();
        Task UpdateAsync(Book book);
        Task<bool> IsAvailableAsync(Guid bookId);
    }
}
