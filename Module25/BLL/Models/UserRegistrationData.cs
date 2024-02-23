using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Models
{
    /// <summary>
    /// Модель Пользователя для добавления в БД или поиска по eMail
    /// </summary>
    public class UserRegistrationData
    {
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
