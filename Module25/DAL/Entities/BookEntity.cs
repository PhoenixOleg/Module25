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
    public class BookEntity
    {
        public int Id { get; set; } //PrimaryKey
        [Required]
        public string Title { get; set; } //Not NULL
        public string? Description { get; set; } // Может быть NULL
        //[Column(TypeName = "date")] //Явно указываю тип данных столбца. Хранить время там не надо. Заменено на тип DateOnly
        public DateOnly PublicationDate { get; set; } //Выбран тип данных Date, а не int или char(4), чтобы можно было выбирать по критериям "новинки месяца" и т. п.
    }
}
