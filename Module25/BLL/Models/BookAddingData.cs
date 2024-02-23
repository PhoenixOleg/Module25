using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Models
{
    /// <summary>
    /// Модель объекта Книга для добавления в БД или поиска базовым атрибутам (например, по названию)
    /// </summary>
    public class BookAddingData
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly PublicationDate { get; set; }
    }
}
