using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Models
{
    public class BookAddingData
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateOnly PublicationDate { get; set; }
    }
}
