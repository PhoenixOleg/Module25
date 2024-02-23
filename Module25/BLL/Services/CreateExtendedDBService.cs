using Module25.BLL.Models;
using Module25.DAL;
using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Services
{
    public class CreateExtendedDBService
    {
        /// <summary>
        /// Создание расширеного контекста и заполнение тестовыми данными
        /// </summary>
        public void CreateExtendedDB()
        {
            try
            {
                using (var db = new ExtendedDBContext(true))
                {
                    var user1 = new UserExtendedEntity { Name = "User1", Email = "user1@gmail.com" };
                    var user2 = new UserExtendedEntity { Name = "User2", Email = "user2@gmail.com" };
                    var user3 = new UserExtendedEntity { Name = "User3", Email = "user3@gmail.com" };

                    var book1 = new BookExtendedEntity { Title = "Book1", Description = "Intresting book", PublicationDate = DateOnly.Parse("01.03.2015") };
                    var book2 = new BookExtendedEntity { Title = "Book2", PublicationDate = DateOnly.Parse("15.07.2017") };
                    var book3 = new BookExtendedEntity { Title = "Book3", PublicationDate = DateOnly.Parse("30.08.2020") };
                    var book4 = new BookExtendedEntity { Title = "Book4", Description = "Good book", PublicationDate = DateOnly.Parse("20.06.2021") };

                    var author1 = new AuthorEntity { Name = "Иван", MiddleName = "Иванович", Surname = "Иванов" };
                    var author2 = new AuthorEntity { Name = "Петр", MiddleName = "Петрович", Surname = "Петров" };
                    var author3 = new AuthorEntity { Name = "Сидор", MiddleName = "Сидорович", Surname = "Сидоров" };

                    var genre1 = new GenreEntity { Name = "Боевик" };
                    var genre2 = new GenreEntity { Name = "Приключения" };
                    var genre3 = new GenreEntity { Name = "Фантастика" };
                    var genre4 = new GenreEntity { Name = "Научпоп" };
                    var genre5 = new GenreEntity { Name = "Драма" };

                    //Добавляем авторов в книги
                    book1.Authors.Add(author1);
                    book2.Authors.AddRange(new[] { author2, author3 });
                    book3.Authors.Add(author3);
                    book4.Authors.AddRange(new[] { author1, author3 });

                    //Жанры в книги
                    book1.Genres.Add(genre1);
                    book2.Genres.AddRange(new[] { genre1, genre2 });
                    book3.Genres.AddRange(new[] { genre3, genre4 });
                    book4.Genres.AddRange(new[] { genre1, genre5 });

                    //добавляем книги к читателям
                    user1.Books.Add(book1);
                    user2.Books.AddRange(new[] { book1, book2, book4 });
                    user3.Books.Add(book3);

                    db.AddRange(user1, user2, user3);
                    db.AddRange(book1, book2, book3, book4);
                    db.AddRange(author1, author2, author3);
                    db.AddRange(genre1, genre2, genre3, genre4, genre5);

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
