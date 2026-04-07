using Library.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Library.Infra.Data;
using Library.Core.Entities;

namespace Library.Infra.Repositories
{
    public class IssueRepository : IIssueRepository
    {
        private readonly AppDbContext _context;
        public IssueRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<BookIssue> AddAsync(BookIssue issue)
        {
            _context.BookIssues.Add(issue);
            await _context.SaveChangesAsync();
            return issue;
        }

        public async Task<List<BookIssue>> GetActiveIssueAsync()
        {
            return await _context.BookIssues
                .Where(bi => bi.ReturnAt == null)
                .ToListAsync();
        }

        public async Task<BookIssue?> GetActiveIssueByBookAsync(Guid bookId)
        {
            return await _context.BookIssues
                .Where(bi => bi.BookId == bookId && bi.ReturnAt == null)
                .FirstOrDefaultAsync();
        }
    }
}
