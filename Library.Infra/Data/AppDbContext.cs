using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Library.Core.Entities;

namespace Library.Infra.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BookIssue> BookIssues { get; set; }
        public DbSet<Log> Logs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configure Book entity
            modelBuilder.Entity<Book>()
                .Property(b => b.Id);

            modelBuilder.Entity<BookIssue>()
                .HasIndex(bi => new { bi.UserId, bi.ReturnAt });
        }
    }
}
