using Module25.DAL.Entities;
using Module25.Task_25_2_4.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.Task_25_2_4.BLL
{
    public class CommonService
    {
        public void CreateFirstDB() 
        {
            try
            {
                using (var db = new BeginerDBContext(true))
                {
                    var user1 = new UserEntity { Name = "User1", Email = "user1@gmail.com" };
                    var user2 = new UserEntity { Name = "User2", Email = "user2@gmail.com" };
                    var user3 = new UserEntity { Name = "User3", Email = "user3@gmail.com" };

                    var book1 = new BookEntity { Title = "Book1", Description = "Intresting book", PublicationDate = DateOnly.Parse("01.03.2015") };
                    var book2 = new BookEntity { Title = "Book2", PublicationDate = DateOnly.Parse("15.07.2017") };
                    var book3 = new BookEntity { Title = "Book3", PublicationDate = DateOnly.Parse("30.08.2020") };
                    var book4 = new BookEntity { Title = "Book4", Description = "Good book", PublicationDate = DateOnly.Parse("20.06.2021") };

                    db.AddRange(user1, user2, user3);
                    db.AddRange(book1, book2, book3, book4);

                    db.SaveChanges();
                }
            }
            catch
            {
                throw; //Отправляю ошибку обратно на уровень PLL, чтобы там отобразить сообщение и/или при необходимости вызвать методы обработки в BLL (если будут)
            }
        }
    }
}
