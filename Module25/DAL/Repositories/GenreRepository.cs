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
        public GenreEntity GetGenreByID(int id)
        {
            using (var db = new ExtendedDBContext(false))
            {
                GenreEntity? genreEntity = db.Genres.Include(b => b.Books).Where(a => a.Id == id).FirstOrDefault();
                return genreEntity;
            }
        }

        public List<GenreEntity> GetAllGenres()
        {
            using (var db = new ExtendedDBContext(false))
            {
                List<GenreEntity> genreEntities = db.Genres.ToList();
                return genreEntities;
            }
        }

        public int AddGenre(GenreEntity genreEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                db.Add(genreEntity);
                return db.SaveChanges();
            }
        }

        public int DeleteGenre(GenreEntity genreEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                db.Remove(genreEntity);
                return db.SaveChanges();
            }
        }

        public int AddGenreToBook(GenreEntity genreEntity, BookExtendedEntity bookExtendedEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                bookExtendedEntity.Genres.Add(genreEntity);
                db.Update(bookExtendedEntity);
                return db.SaveChanges();
            }
        }

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
