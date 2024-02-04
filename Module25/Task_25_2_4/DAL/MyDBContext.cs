using Microsoft.EntityFrameworkCore;
using Module25.Task_25_2_4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Module25.Task_25_2_4.DAL
{
    public class MyDBContext : DbContext
    {
        // Объекты таблицы Users
        public DbSet<User> Users { get; set; }

        // Объекты таблицы Books
        public DbSet<Book> Books { get; set; }

        public MyDBContext()
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=WIN-ACL07JRD9H1\SQLEXPRESS;TrustServerCertificate=True;Trusted_Connection=false;Database=HomeWork;User ID=sa;Password=1q2W3e4R");
        }
    }
}
