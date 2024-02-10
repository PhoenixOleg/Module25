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
            using (var db = new MyDBContext(false))
            {
                BookEntity? BookEntity = db.Books.Where(Book => Book.Id == id).FirstOrDefault();
                return BookEntity;
            }
        }

        /// <summary>
        /// Метод получения всех книг
        /// </summary>
        /// <returns>Список экземпляров класса BookEntity</returns>
        public List<BookEntity> GetAllBook()
        {
            using (var db = new MyDBContext(false))
            {
                List<BookEntity> BookEntitys = db.Books.ToList();
                return BookEntitys;
            }
        }

        /// <summary>
        /// Метод добавления книги
        /// </summary>
        /// <param name="BookEntity">Экземпляр класса BookEntity</param>
        /// <returns>1 - если книга добавлена (количество обработанных строк); 
        /// 0 - книга не добавлена</returns>
        public int AddBook(BookEntity BookEntity)
        {
            using (var db = new MyDBContext(false))
            {
                db.Add(BookEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Метод удаления книги по экземпляру объекта книги
        /// </summary>
        /// <param name="BookEntity">Экземпляр класса BookEntity</param>
        /// <returns>1 - если книга удалена (количество обработанных строк); 
        /// 0 - книга не удалена</returns>
        public int DeleteBook(BookEntity BookEntity)
        {
            using (var db = new MyDBContext(false))
            {
                db.Remove(BookEntity);
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
            using (var db = new MyDBContext(false))
            {
                BookEntity? BookEntity = GetBookByID(id);
                if (BookEntity != null)
                {
                    db.Remove(BookEntity);
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
            using (var db = new MyDBContext(false))
            {
                BookEntity? BookEntity = GetBookByID(id);
                if (BookEntity != null)
                {
                    BookEntity.PublicationDate = publicationDate;
                    db.Update(BookEntity);
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
