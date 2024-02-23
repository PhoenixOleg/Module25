using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Models
{
    /// <summary>
    /// Полная модель Жанра со список книг этого жанра
    /// </summary>
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public List<BookExtendedEntity>? Books { get; set; }

        public Genre(int id, string name, List<BookExtendedEntity>? books)
        {
            Id = id;
            Name = name;
            Books = books;
        }
    }
}
