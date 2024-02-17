using Microsoft.EntityFrameworkCore;
using Module25.BLL.Models;
using Module25.DAL;
using Module25.DAL.Entities;
using Module25.Task_25_2_4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.Task_25_2_4.DAL.Repositories
{
    public class BookRepository
    {
        /// <summary>
        /// Метод возвращает книгу по ее ID
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <returns>Экземпляр класса BookEntity</returns>
        public BookEntity GetBookByID(int id)
        {
            using (var db = new BeginerDBContext(false))
            {
                BookEntity? bookEntity = db.Books.Where(Book => Book.Id == id).FirstOrDefault();
                return bookEntity;
            }
        }

        public BookExtendedEntity GetBookByID_Extended(int id)
        {
            using (var db = new ExtendedDBContext(false))
            {
                BookExtendedEntity? bookExtendedEntity = db.Books.Include(b => b.Authors).Include(b => b.Genres).Include(b => b.Users).Where(Book => Book.Id == id).FirstOrDefault();
                return bookExtendedEntity;
            }
        }
     
        /// <summary>
        /// Метод получения всех книг
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
        /// <param name="id"></param>
        /// <param name="publicationDate"></param>
        /// <returns>1 - если книга обновлена (количество обработанных строк); 
        /// 0 - книга не обновлнеа;
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
    }
}
