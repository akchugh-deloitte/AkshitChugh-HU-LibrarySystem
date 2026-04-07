using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.DTO
{
    public class BookFilter
    {
        public string? Search { get ; set; }
        public string? Author { get; set; }
        public int? Year { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
