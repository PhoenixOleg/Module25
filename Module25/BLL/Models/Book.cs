using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Models
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly PublicationDate { get; set; }

        public Book(int id, string title, string? description, DateOnly publicationDate)
        {
            Id = id;
            Title = title;
            Description = description;
            PublicationDate = publicationDate;
        }
    }
}
