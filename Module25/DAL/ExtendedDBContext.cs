using Microsoft.EntityFrameworkCore;
using Module25.BLL.Models;
using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Module25.DAL
{
    public class ExtendedDBContext : DbContext
    {
        // Объекты таблицы Users
        public DbSet<UserExtendedEntity> Users { get; set; }

        // Объекты таблицы Books
        public DbSet<BookExtendedEntity> Books { get; set; }

        // Объекты таблицы Author
        public DbSet<AuthorEntity> Authors { get; set; }

        // Объекты таблицы Genres
        public DbSet<GenreEntity> Genres { get; set; }

        public ExtendedDBContext(bool reCreatedDB)
        {
            if (reCreatedDB)
            {
                Database.EnsureDeleted(); //Чтобы не убивать данные
            }
            
            Database.EnsureCreated();            
        }

        /// <summary>
        /// Метод для задания ограничения уникальности на поле Email через FluentAPI
        /// Создает некластаризованный индекс с ограничением на уникальность
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserExtendedEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(Program.connectionString);
        }
    }
}
