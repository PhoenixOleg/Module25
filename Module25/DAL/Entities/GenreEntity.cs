using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.DAL.Entities
{
    public class GenreEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        // Навигационное свойство
        //В жанре может быть несколько книг "many-to-many"
        public List<BookExtendedEntity> Books { get; set; } = new List<BookExtendedEntity>();
    }
}
