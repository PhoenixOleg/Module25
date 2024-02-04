using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.Task_25_2_4.DAL.Entities
{
    public class User
    {
        public int Id { get; set; } //PrimaryKey
        [Required]
        public string Name { get; set; } //Not NULL
        [Required]
        public string Email { get; set; } //Not NULL
    }
}
