using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Library.Infra.Data;
using Library.Core.Interfaces;
using Library.Core.Entities;

namespace Library.Infra.Repositories
{
    public class BookRepository: IBookRepository
    {
        private readonly AppDbContext _context;
        public BookRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Book?> GetByIdAsync(Guid id)
        {
            return await _context.Books.FindAsync(id);
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return await _context.Books.ToListAsync();
        }

        public async Task UpdateAsync(Book book)
        {
            _context.Books.Update(book);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsAvailableAsync(Guid bookId)
        {
            var book = await _context.Books.FindAsync(bookId);
            return book != null && book.IsAvailable;
        }
    }
}
