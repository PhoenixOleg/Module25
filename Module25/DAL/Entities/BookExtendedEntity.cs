using Module25.BLL.Models;
using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.DAL.Entities
{
    public class BookExtendedEntity
    {
        public int Id { get; set; } //PrimaryKey
        [Required]
        public string Title { get; set; } //Not NULL
        public string? Description { get; set; } // Может быть NULL
        //[Column(TypeName = "date")] //Явно указываю тип данных столбца. Хранить время там не надо. Заменено на тип DateOnly
        public DateOnly PublicationDate { get; set; } //Выбран тип данных Date, а не int или char(4), чтобы можно было выбирать по критериям "новинки месяца" и т. п.

        // Навигационное свойство
        //У книги может несколько авторов "many-to-many"
        public List<AuthorEntity> Authors { get; set; } = new List<AuthorEntity>();

        // Навигационное свойство
        //У книги может несколько жанров "many-to-many" (приключение, фантастика)
        public List<GenreEntity> Genres { get; set; } = new List<GenreEntity>();

        // Навигационное свойство
        //Книга может "на руках" у нескольких пользователей "many-to-many"
        public List<UserExtendedEntity> Users { get; set; } = new List<UserExtendedEntity>();
    }
}
