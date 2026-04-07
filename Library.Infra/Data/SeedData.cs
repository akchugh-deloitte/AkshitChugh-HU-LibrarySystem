using System;
using System.Collections.Generic;
using System.Text;
using Library.Core.Entities;

namespace Library.Infra.Data
{
    public class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            if (context.Books.Any())
            {
                return; // DB has been seeded
            }
            var books = new List<Book>
            {
                new Book {Id = Guid.NewGuid(), Title = "The Great Gatsby", Author = "F. Scott Fitzgerald", Year = 1925, IsAvailable = true},
                new Book {Id = Guid.NewGuid(), Title = "To Kill a Mockingbird", Author = "Harper Lee", Year = 1960,  IsAvailable = true },
                new Book {Id = Guid.NewGuid(), Title = "1984", Author = "George Orwell", Year = 1949,  IsAvailable = true },
                new Book {Id = Guid.NewGuid(), Title = "Pride and Prejudice", Author = "Jane Austen", Year = 1813,  IsAvailable = true },
                new Book {Id = Guid.NewGuid(), Title = "The Catcher in the Rye", Author = "J.D. Salinger", Year = 1951,  IsAvailable = true}
            };
            context.Books.AddRange(books);
            context.SaveChanges();
        }
    }
}
