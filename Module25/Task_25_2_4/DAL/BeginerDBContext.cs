using Microsoft.EntityFrameworkCore;
using Module25.BLL.Models;
using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Module25.Task_25_2_4.DAL
{
    public class BeginerDBContext : DbContext
    {
        // Объекты таблицы Users
        public DbSet<UserEntity> Users { get; set; }

        // Объекты таблицы Books
        public DbSet<BookEntity> Books { get; set; }

        public BeginerDBContext(bool reCreatedDB)
        {
            if (reCreatedDB)
            {
                Database.EnsureDeleted(); //Чтобы можно было выполнить первое задание после "старших" заданий
            }
            
            Database.EnsureCreated();
        }

        /// <summary>
        /// Метод для задания ограничения уникальности на поле Email через FluentAPI
        /// Создает некластаризованный индекс с ограничением на уникальность
        /// </summary>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserEntity>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=WIN-ACL07JRD9H1\SQLEXPRESS;TrustServerCertificate=True;Trusted_Connection=false;Database=HomeWork;User ID=sa;Password=1q2W3e4R");
        }
    }
}
