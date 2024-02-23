using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Models
{
    /// <summary>
    /// Полная модель объекта Книга со списком авторов, жанров и подписчиков (читателей конкретной книги)
    /// </summary>
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly PublicationDate { get; set; }

        public List<AuthorEntity>? Authors { get; set; }
        public List<GenreEntity>? Genres { get; set; }
        public List<UserExtendedEntity>? Users { get; set; }

        public Book(int id, string title, string? description, DateOnly publicationDate)
        {
            Id = id;
            Title = title;
            Description = description;
            PublicationDate = publicationDate;
        }

        public Book(int id, string title, string? description, DateOnly publicationDate, List<AuthorEntity>? authors, List<GenreEntity> genres, List<UserExtendedEntity> users)
        {
            Id = id;
            Title = title;
            Description = description;
            PublicationDate = publicationDate;
            Authors = authors;
            Genres = genres;
            Users = users;
        }
    }
}
