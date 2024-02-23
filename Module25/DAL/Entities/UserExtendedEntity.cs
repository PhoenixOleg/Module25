using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.DAL.Entities
{
    public class UserExtendedEntity
    {
        /// <summary>
        /// Пользователи (читатели) со списком читаемых книг
        /// </summary>
        public int Id { get; set; } //PrimaryKey
        [Required]
        public string Name { get; set; } //Not NULL
        [Required]
        public string Email { get; set; } //Not NULL

        // Навигационное свойство
        //У пользователя "на руках" может несколько книг "many-to-many"
        public List<BookExtendedEntity> Books { get; set; } = new List<BookExtendedEntity>();
    }
}
