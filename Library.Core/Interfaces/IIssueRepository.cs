using System;
using System.Collections.Generic;
using System.Text;
using Library.Core.Entities;

namespace Library.Core.Interfaces
{
    public interface IIssueRepository
    {
        Task<BookIssue> AddAsync(BookIssue issue);
        Task<List<BookIssue>> GetActiveIssueAsync();
        Task<BookIssue?> GetActiveIssueByBookAsync(Guid bookId);

    }
}
