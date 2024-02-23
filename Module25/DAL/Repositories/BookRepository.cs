using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using Module25.BLL.Models;
using Module25.DAL;
using Module25.DAL.Entities;
using Module25.Task_25_2_4.DAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.DAL.Repositories
{
    public class BookRepository
    {
        /// <summary>
        /// Метод возвращает книгу по ее ID (без авторов и жанров)
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <returns>Экземпляр класса BookEntity или null</returns>
        public BookEntity? GetBookByID(int id)
        {
            using (var db = new BeginerDBContext(false))
            {
                BookEntity? bookEntity = db.Books.Where(Book => Book.Id == id).FirstOrDefault();
                return bookEntity;
            }
        }

        /// <summary>
        /// Метод возвращает книгу по ее ID c авторами и жанрами
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <returns>Экземпляр класса BookExtendedEntity или null</returns>
        public BookExtendedEntity? GetBookByID_Extended(int id)
        {
            using (var db = new ExtendedDBContext(false))
            {
                BookExtendedEntity? bookExtendedEntity = db.Books.Include(b => b.Authors).Include(b => b.Genres).Include(b => b.Users).Where(Book => Book.Id == id).FirstOrDefault();
                return bookExtendedEntity;
            }
        }

        /// <summary>
        /// Метод получения всех книг (без авторов и жанров)
        /// </summary>
        /// <returns>Список экземпляров класса BookEntity</returns>
        public List<BookEntity> GetAllBooks()
        {
            using (var db = new BeginerDBContext(false))
            {
                List<BookEntity> bookEntities = db.Books.ToList();
                return bookEntities;
            }
        }

        /// <summary>
        /// Метод получения всех книг c авторами и жанрами
        /// </summary>
        /// <returns>Список экземпляров класса BookExtendedEntity</returns>
        public List<BookExtendedEntity> GetAllBooks_Extended()
        {
            using (var db = new ExtendedDBContext(false))
            {
                List<BookExtendedEntity> bookExtendedEntities = db.Books.Include(b => b.Authors).Include(b => b.Genres).Include(b => b.Users).ToList();
                return bookExtendedEntities;
            }
        }

        /// <summary>
        /// Метод добавления книги
        /// </summary>
        /// <param name="BookEntity">Экземпляр класса BookEntity</param>
        /// <returns>1 - если книга добавлена (количество обработанных строк); 
        /// 0 - книга не добавлена</returns>
        public int AddBook(BookEntity bookEntity)
        {
            using (var db = new BeginerDBContext(false))
            {
                db.Add(bookEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Метод удаления книги по экземпляру объекта книги
        /// </summary>
        /// <param name="BookEntity">Экземпляр класса BookEntity</param>
        /// <returns>1 - если книга удалена (количество обработанных строк); 
        /// 0 - книга не удалена</returns>
        public int DeleteBook(BookEntity bookEntity)
        {
            using (var db = new BeginerDBContext(false))
            {
                db.Remove(bookEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Метод удаления книги по ее ID
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <returns>1 - если книга удалена (количество обработанных строк); 
        /// 0 - книга не удалена;
        /// -1 - книга не найдена по id</returns>
        public int DeleteBook(int id)
        {
            using (var db = new BeginerDBContext(false))
            {
                BookEntity? bookEntity = GetBookByID(id);
                if (bookEntity != null)
                {
                    db.Remove(bookEntity);
                    return db.SaveChanges();
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Метод обновления года издания книги по е ID
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <param name="publicationDate">Новая дата издания книги</param>
        /// <returns>1 - если книга обновлена (количество обработанных строк); 
        /// 0 - книга не обновлнена;
        /// -1 - книга не найдена по id</returns>
        public int UpdateBookNameByID(int id, DateOnly publicationDate)
        {
            using (var db = new BeginerDBContext(false))
            {
                BookEntity? bookEntity = GetBookByID(id);
                if (bookEntity != null)
                {
                    bookEntity.PublicationDate = publicationDate;
                    db.Update(bookEntity);
                    return db.SaveChanges();
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Получение количества книга конкретного автора (соавтора) по ФИО
        /// </summary>
        /// <param name="authorEntity">Экземпляр класса AuthorEntity с данными об авторе (ФИО. ID не учитывается в этом методе)</param>
        /// <returns>Количество найденных книг</returns>
        public int GetCountBooksByAuthor(AuthorEntity authorEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                return db.Authors
                   .Include(b => b.Books)
                   .Where(a => a.Name == authorEntity.Name && a.MiddleName == authorEntity.MiddleName && a.Surname == authorEntity.Surname)
                   .Select(b => b.Books.Count).First();
            }
        }

        /// <summary>
        /// Получение количества книга конкретного жанра
        /// </summary>
        /// <param name="genreEntity">Экземпляр класса GenreEntity (с названием жанра)</param>
        /// <returns>Количество найденных книг</returns>
        public int GetCountBooksByGenre(GenreEntity genreEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                return db.Genres
                   .Include(b => b.Books)
                   .Where(g => g.Name == genreEntity.Name)
                   .Select(b => b.Books.Count).First();
            }
        }


        /// <summary>
        /// Метод проверяет, есть ли книга с конкретным названием и ФИО автора (соавтора) в библиотеке
        /// </summary>
        /// <param name="authorEntity">Экземпляр класса AuthorEntity с данными об авторе (ФИО. ID не учитывается в этом методе)</param>
        /// <param name="bookEntity">Экземпляр класса BookEntity (с названием  книги)</param>
        /// <returns>true - книга найдена по названию и ФИО автора,
        /// false - книга не найдена</returns>
        public bool IsBookByTitleAuthor(AuthorEntity authorEntity, BookEntity bookEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                return db.Authors
                   .Include(b => b.Books)
                   .Where(a => a.Name == authorEntity.Name && a.MiddleName == authorEntity.MiddleName && a.Surname == authorEntity.Surname)
                   .Select(b => b.Books.Where(b => b.Title == bookEntity.Title)).ToList().Any();
            }
        }

        /// <summary>
        /// Получение последней вышедшей (изданной) книги (Task 7)
        /// </summary>
        /// <returns>Список экземпляров класса BookExtendedEntity (список книг)</returns>
        public List<BookExtendedEntity> GetLastPublishedBook()
        {
            using (var db = new ExtendedDBContext(false))
            {
                return db.Books.Where(b => b.PublicationDate == db.Books.Max(m => m.PublicationDate))
                    .Include(g => g.Genres)
                    .Include(a => a.Authors)
                    .ToList();
            }
        }

        /// <summary>
        /// Получение списка всех книг, отсортированного в алфавитном порядке по названию (Task 8)
        /// </summary>
        /// <returns>Список экземпляров класса BookExtendedEntity (список книг)</returns>
        public List<BookExtendedEntity> GetAllBooksNameAsc()
        {
            using (var db = new ExtendedDBContext(false))
            {
                return db.Books.OrderBy(b => b.Title)
                    .Include(g => g.Genres)
                    .Include(a => a.Authors)
                    .ToList();
            }
        }

        /// <summary>
        /// Получение списка всех книг, отсортированного в порядке убывания года их выхода (Task 9)
        /// </summary>
        /// <returns>Список экземпляров класса BookExtendedEntity (список книг)</returns>
        public List<BookExtendedEntity> GetAllBooksPubDesc()
        {
            using (var db = new ExtendedDBContext(false))
            {
                return db.Books.OrderByDescending(b => b.PublicationDate)
                    .Include(g => g.Genres)
                    .Include(a => a.Authors)
                    .ToList();
            }
        }

        /// <summary>
        /// Получить список книг определенного жанра и вышедших между определенными годами (Task 1)
        /// </summary>
        /// <param name="genreEntity">Экземпляр класса GenreEntity (с названием жанра)</param>
        /// <param name="dateInterval">Кортеж с интервалом дат.</param>
        /// <returns>Список экземпляров класса BookExtendedEntity (список книг)</returns>
        public List<BookExtendedEntity> GetBooksByGenrePubYear(GenreEntity genreEntity, (DateOnly beginDate, DateOnly endDate) dateInterval)
        {
            using (var db = new ExtendedDBContext(false))
            {
                return db.Books
                    .Include(a => a.Authors)
                    .Include(g => g.Genres)
                    .Where(b => b.PublicationDate >= dateInterval.beginDate & b.PublicationDate < dateInterval.endDate)
                    //.Include(g => g.Genres.Where(g => g.Name == genreEntity.Name)) //Она фильтрует список подгружаемых в книгу жанров но не фильтрует список книг
                    .Where(book => book.Genres.Contains(db.Genres.Where(g => g.Name == genreEntity.Name).FirstOrDefault()))
                    .ToList();

                #region Два варианта с SQL
                //Долго не мог составить запрос EF поэтому стал пробовать прямой SQL. Убирать из кода этот кусок не стал

                //SqlParameter pGenreName = new SqlParameter("@GenreName", genreEntity.Name);
                //SqlParameter pBeginDate = new SqlParameter("@BeginDate", dateInterval.beginDate);
                //SqlParameter pEndDate = new SqlParameter("@EndDate", dateInterval.endDate);

                //Метод FromSqlRaw (менее устойчивый к инекциям)
                //return db.Books
                //.FromSqlRaw("SELECT b.*, gl.Name GenreName, authors.Name, authors.Surname, authors.MiddleName FROM Books b\r\nJOIN (\r\n\tSELECT bg.BooksId, bg.GenresId from BookExtendedEntityGenreEntity bg\r\n\tJOIN Genres g on bg.GenresId = g.Id \r\n\tWHERE Name = @GenreName\r\n\t) genre on b.Id = genre.BooksId\r\nLEFT JOIN (\r\n    SELECT ab.BooksId, a.MiddleName, a.Name, a.Surname\r\n    FROM AuthorEntityBookExtendedEntity ab\r\n    JOIN Authors a ON ab.AuthorsId = a.Id\r\n) authors ON b.Id = authors.BooksId\r\nJOIN (\r\n\tSELECT g.Name, bg.BooksId from BookExtendedEntityGenreEntity bg\r\n\tJOIN Genres g on bg.GenresId = g.Id \r\n) gl ON gl.BooksId  = b.Id\r\nWHERE PublicationDate BETWEEN @BeginDate AND @EndDate", pGenreName, pBeginDate, pEndDate)
                //.Include(g => g.Genres)
                //.Include(a => a.Authors)
                //.ToList();

                //Метод FromSql - устойчивый (https://learn.microsoft.com/ru-ru/ef/core/querying/sql-queries)
                //return db.Books
                //        .FromSql($"SELECT b.*, gl.Name GenreName, authors.Name, authors.Surname, authors.MiddleName FROM Books b\r\nJOIN (\r\n\tSELECT bg.BooksId, bg.GenresId from BookExtendedEntityGenreEntity bg\r\n\tJOIN Genres g on bg.GenresId = g.Id \r\n\tWHERE Name = {pGenreName}\r\n\t) genre on b.Id = genre.BooksId\r\nLEFT JOIN (\r\n    SELECT ab.BooksId, a.MiddleName, a.Name, a.Surname\r\n    FROM AuthorEntityBookExtendedEntity ab\r\n    JOIN Authors a ON ab.AuthorsId = a.Id\r\n) authors ON b.Id = authors.BooksId\r\nJOIN (\r\n\tSELECT g.Name, bg.BooksId from BookExtendedEntityGenreEntity bg\r\n\tJOIN Genres g on bg.GenresId = g.Id \r\n) gl ON gl.BooksId  = b.Id\r\nWHERE PublicationDate BETWEEN {pBeginDate} AND {pEndDate}")
                //        .Include(g => g.Genres)
                //        .Include(a => a.Authors)
                //        .ToList();
                #endregion
            };
        }
    }
}
