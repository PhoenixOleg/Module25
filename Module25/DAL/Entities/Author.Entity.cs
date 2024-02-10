using Module25.BLL.Models;
using Module25.Task_25_2_4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.DAL.Entities
{
    public class AuthorEntity
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string? MiddleName { get; set; }
        [Required]
        public string Surname { get; set; }

        // Навигационное свойство
        //У автора может несколько книг "many-to-many"
        public List<BookExtendedEntity> Books { get; set; } = new List<BookExtendedEntity>();
    }
}
