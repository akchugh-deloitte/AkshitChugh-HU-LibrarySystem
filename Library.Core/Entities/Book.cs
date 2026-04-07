using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.Entities
{
    public class Book
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public int Year { get; set; }
        public bool IsAvailable { get; set; } = true; 
    }
}
