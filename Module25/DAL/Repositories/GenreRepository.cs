using Microsoft.EntityFrameworkCore;
using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.DAL.Repositories
{
    public class GenreRepository
    {

        /// <summary>
        /// Получение жанра по его ID
        /// </summary>
        /// <param name="id">ID жанра</param>
        /// <returns>Экземпляр GenreEntity или null</returns>
        public GenreEntity? GetGenreByID(int id)
        {
            using (var db = new ExtendedDBContext(false))
            {
                GenreEntity? genreEntity = db.Genres.Include(b => b.Books).Where(a => a.Id == id).FirstOrDefault();
                return genreEntity;
            }
        }

        /// <summary>
        /// Получение списка всех жанров
        /// </summary>
        /// <returns>Список экземпляров GenreEntity</returns>
        public List<GenreEntity> GetAllGenres()
        {
            using (var db = new ExtendedDBContext(false))
            {
                List<GenreEntity> genreEntities = db.Genres.ToList();
                return genreEntities;
            }
        }

        /// <summary>
        /// Добавление жанра в БД
        /// </summary>
        /// <param name="genreEntity">Экземпляр GenreEntity (с названием жанра)</param>
        /// <returns>1 - жанр добавлен (количество обработанных строк); 
        /// 0 - жанр не добавлен</returns>
        public int AddGenre(GenreEntity genreEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                db.Add(genreEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление жанра из БД
        /// </summary>
        /// <param name="genreEntity">Экземпляр GenreEntity</param>
        /// <returns>1 - жанр удален (количество обработанных строк); 
        /// 0 - жанр не удален</returns>
        public int DeleteGenre(GenreEntity genreEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                db.Remove(genreEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Добавление жанра в внигу
        /// </summary>
        /// <param name="genreEntity">Экземпляр GenreEntity</param>
        /// <param name="bookExtendedEntity">Экземпляр BookExtendedEntity</param>
        /// <returns>1 - жанр добавлен (количество обработанных строк); 
        /// 0 - жанр не добавлен</returns>
        public int AddGenreToBook(GenreEntity genreEntity, BookExtendedEntity bookExtendedEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                bookExtendedEntity.Genres.Add(genreEntity);
                db.Update(bookExtendedEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Удаление жанра из внигу
        /// </summary>
        /// <param name="genreEntity">Экземпляр GenreEntity</param>
        /// <param name="bookExtendedEntity">Экземпляр BookExtendedEntity</param>
        /// <returns>1 - жанр удален (количество обработанных строк); 
        /// 0 - жанр не удален</returns>
        /// <exception cref="NullReferenceException">Не найдена книга или выбранная книга не относится к этому жанру</exception>
        public int RemoveGenreFromBook(GenreEntity genreEntity, BookExtendedEntity bookExtendedEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                BookExtendedEntity targetBook = db.Books.Include(b => b.Genres).Where(b => b.Id == bookExtendedEntity.Id).FirstOrDefault();
                if (targetBook == null)
                {
                    throw new NullReferenceException("Книга не найдена");
                };

                GenreEntity genreForRemove = targetBook.Genres.Where(g => g.Id == genreEntity.Id).FirstOrDefault();
                if (genreForRemove == null)
                {
                    throw new NullReferenceException("Жанр не принадлежит данной книге");
                };

                targetBook.Genres.Remove(genreForRemove);

                return db.SaveChanges();
            }
        }

    }
}
