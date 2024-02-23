using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Models
{
    /// <summary>
    /// Модель объекта Автор для добавления автора в БД и поиска по ФИО автора.
    /// </summary>
    public class AuthorAddingData
    {
        public string Name { get; set; }
        public string? MiddleName { get; set; }
        public string Surname { get; set; }
    }
}
