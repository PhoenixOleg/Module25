using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Models
{
    /// <summary>
    /// Модель Пользователя
    /// </summary>
    public class User
    {
        public int Id { get; }
        public string Name { get; set; }
        public string Email { get; set; }

        public User(int Id, string Name, string Email) 
        { 
            this.Id = Id;
            this.Name = Name;
            this.Email = Email;
        }
    }
}
