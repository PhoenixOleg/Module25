
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module25.DAL.Entities;
using Module25.Task_25_2_4.DAL.Entities;
using Module25.Task_25_2_4.DAL;
using Microsoft.EntityFrameworkCore;
using Module25.BLL.Models;
using Module25.BLL.Exceptions;

namespace Module25.DAL.Repositories
{
    public class AuthorRepository
    {
        public AuthorEntity GetAuthorByID(int id)
        {
            using (var db = new ExtendedDBContext(false))
            {
                AuthorEntity? authorEntity = db.Authors.Include(b => b.Books).Where(a => a.Id == id).FirstOrDefault();
                return authorEntity;
            }
        }

        public List<AuthorEntity> GetAllAuthors()
        {
            using (var db = new ExtendedDBContext(false))
            {
                List<AuthorEntity> authorEntities = db.Authors.ToList();
                return authorEntities;
            }
        }

        public int AddAuthor(AuthorEntity authorEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                db.Add(authorEntity);
                return db.SaveChanges();
            }
        }

        public int DeleteAuthor(AuthorEntity authorEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                db.Remove(authorEntity);
                return db.SaveChanges();
            }
        }

        public int AddAuthorToBook(AuthorEntity authorEntity, BookExtendedEntity bookExtendedEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                bookExtendedEntity.Authors.Add(authorEntity);
                db.Update(bookExtendedEntity);
                return db.SaveChanges();
            }
        }

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
