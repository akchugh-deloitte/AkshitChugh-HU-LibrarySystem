using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Core.Entities
{
    public class Log
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
