using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.DTO
{
    public class BookDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int Year { get; set; }
        public bool IsAvailable { get; set; }   
    }
}
