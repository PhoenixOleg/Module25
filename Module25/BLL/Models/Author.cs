using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? MiddleName { get; set; }
        public string SurName { get; set; }
        public List<BookExtendedEntity>? Books { get; set; }

        public Author(int id, string name, string? middleName, string surName, List<BookExtendedEntity>? books)
        {
            Id = id;
            Name = name;
            MiddleName = middleName;
            SurName = surName;
            Books = books;
        }
    }
}
