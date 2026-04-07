using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.Entities
{
    public class BookIssue
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid BookId { get; set; }
        public DateTime? IssueDate { get; set; }
        public DateTime? ReturnAt { get; set; }
    }
}
