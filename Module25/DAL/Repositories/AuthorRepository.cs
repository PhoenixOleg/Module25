
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module25.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Module25.BLL.Models;
using Module25.BLL.Exceptions;

namespace Module25.DAL.Repositories
{
    public class AuthorRepository
    {
        /// <summary>
        /// Получение автора по его ID
        /// </summary>
        /// <param name="id">ID автора (целое положительное число)</param>
        /// <returns>Экземпляр AuthorEntity с найденным автором или null</returns>
        public AuthorEntity? GetAuthorByID(int id)
        {
            using (var db = new ExtendedDBContext(false))
            {
                AuthorEntity? authorEntity = db.Authors.Include(b => b.Books).Where(a => a.Id == id).FirstOrDefault();
                return authorEntity;
            }
        }

        /// <summary>
        /// /Получение списка всех авторов
        /// </summary>
        /// <returns>Список экземпляров AuthorEntityс данным об авторах</returns>
        public List<AuthorEntity> GetAllAuthors()
        {
            using (var db = new ExtendedDBContext(false))
            {
                List<AuthorEntity> authorEntities = db.Authors.ToList();
                return authorEntities;
            }
        }

        /// <summary>
        /// Добавление автора
        /// </summary>
        /// <param name="authorEntity">Экземпляр AuthorEntity с данными автора</param>
        /// <returns>1 - если автор добавлен (количество обработанных строк); 
        /// 0 - автор не добавлен</returns>
        public int AddAuthor(AuthorEntity authorEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                db.Add(authorEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление автора
        /// </summary>
        /// <param name="authorEntity">Экземпляр AuthorEntity с данными автора</param>
        /// <returns>1 - если автор удален (количество обработанных строк); 
        /// 0 - автор не удален</returns>
        public int DeleteAuthor(AuthorEntity authorEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                db.Remove(authorEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Добавление автора в книгу
        /// </summary>
        /// <param name="authorEntity">Экземпляр AuthorEntity с данными автора</param>
        /// <param name="bookExtendedEntity">Экземпляр BookExtendedEntity с данными о книге</param>
        /// <returns>1 - автор добавлен в книгу;
        /// 0 - автор не добавлен в книгу</returns>
        public int AddAuthorToBook(AuthorEntity authorEntity, BookExtendedEntity bookExtendedEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                bookExtendedEntity.Authors.Add(authorEntity);
                db.Update(bookExtendedEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление автора из книги
        /// </summary>
        /// <param name="authorEntity">Экземпляр AuthorEntity с данными автора</param>
        /// <param name="bookExtendedEntity">Экземпляр BookExtendedEntity с данными о книге</param>
        /// <returns>1 - автор удален из книги;
        /// 0 - автор не удален из книги</returns>
        /// <exception cref="NullReferenceException">Не найдена книга или автор в книге</exception>
        public int RemoveAuthorFromBook(AuthorEntity authorEntity, BookExtendedEntity bookExtendedEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                BookExtendedEntity targetBook = db.Books.Include(b => b.Authors).Where(b => b.Id == bookExtendedEntity.Id).FirstOrDefault();
                if (targetBook == null)
                {
                    throw new NullReferenceException("Книга не найдена");
                };

                AuthorEntity authorForRemove = targetBook.Authors.Where(a => a.Id == authorEntity.Id).FirstOrDefault();
                if (authorForRemove == null)
                {
                    throw new NullReferenceException("Автор не найден среди создателей данной книги");
                };

                targetBook.Authors.Remove(authorForRemove);

                return db.SaveChanges();
            }
        }
    }
}
