using Module25.Task_25_2_4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.DAL.Entities;
using Module25.DAL;
using Microsoft.EntityFrameworkCore;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.VisualBasic;

namespace Module25.Task_25_2_4.DAL.Repositories
{
    public class UserRepository
    {
        /// <summary>
        /// Метод возвращает пользователя по его ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Экземпляр класса UserEntity</returns>
        public UserEntity GetUserByID(int id)
        {
            using (var db = new BeginerDBContext(false))
            {
                UserEntity? userEntity = db.Users.Where(user => user.Id == id).FirstOrDefault();
                return userEntity;
            }
        }

        /// <summary>
        /// Метод получения всех пользователей
        /// </summary>
        /// <returns>Список экземпляров класса UserEntity</returns>
        public List<UserEntity> GetAllUser()
        {
            using (var db = new BeginerDBContext(false))
            {
                List<UserEntity> userEntities = db.Users.ToList();
                return userEntities;
            }
        }

        /// <summary>
        /// Метод добавления пользователя
        /// </summary>
        /// <param name="userEntity">Экземпляр класса UserEntity</param>
        /// <returns>1 - если пользователь добавлен (количество обработанных строк); 
        /// 0 - пользователь не добавлен</returns>
        public int AddUser(UserEntity userEntity)
        {
            using (var db = new BeginerDBContext(false))
            {
                db.Add(userEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Метод удаления пользователя по экземпляру объекта пользователя
        /// </summary>
        /// <param name="userEntity">Экземпляр класса UserEntity</param>
        /// <returns>1 - если пользователь удален (количество обработанных строк); 
        /// 0 - пользователь не удален</returns>
        public int DeleteUser(UserEntity userEntity)
        {
            using (var db = new BeginerDBContext(false))
            {
                db.Remove(userEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Метод удаления пользователя по его ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>1 - если пользователь удален (количество обработанных строк); 
        /// 0 - пользователь не удален;
        /// -1 - не найден пользователь по id</returns>
        public int DeleteUser(int id)
        {
            using (var db = new BeginerDBContext(false))
            {
                UserEntity? userEntity = GetUserByID(id);
                if (userEntity != null)
                {
                    db.Remove(userEntity);
                    return db.SaveChanges();
                }
                else
                {
                    return -1;
                }
            }
        }

        /// <summary>
        /// Метод обновления имени пользователя по его ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <returns>1 - если пользователь обновлен (количество обработанных строк); 
        /// 0 - пользователь не обновлен;
        /// -1 - не найден пользователь по id</returns>
        public int UpdateUserNameByID(int id, string name)
        {
            using (var db = new BeginerDBContext(false))
            {
                UserEntity? userEntity = GetUserByID(id);
                if (userEntity != null)
                {
                    userEntity.Name = name;
                    db.Update(userEntity);
                    return db.SaveChanges();
                }
                else
                {
                    return -1;
                }
            }
        }

        public int GiveBookToUser(UserExtendedEntity userExtendedEntity, BookExtendedEntity bookExtendedEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                bookExtendedEntity.Users.Add(userExtendedEntity);
                db.Update(bookExtendedEntity);
                return db.SaveChanges();
            }
        }

        public int GetBookFromUser(UserExtendedEntity userExtendedEntity, BookExtendedEntity bookExtendedEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                BookExtendedEntity targetBook = db.Books.Include(b => b.Users).Where(b => b.Id == bookExtendedEntity.Id).FirstOrDefault();
                if (targetBook == null)
                {
                    throw new NullReferenceException("Книга не найдена");
                };

                UserExtendedEntity userForRemove = targetBook.Users.Where(b => b.Id == userExtendedEntity.Id).FirstOrDefault();
                if (userForRemove == null)
                {
                    throw new NullReferenceException("Клиент не найден среди читателей данной книги");
                };

                targetBook.Users.Remove(userForRemove);

                return db.SaveChanges();
            }
        }

        public bool HaveUserBookByTitle(BookEntity bookEntity, UserEntity userEntity) //@@@ Task 5
        {
            using (var db = new ExtendedDBContext(false))
            {
                Console.WriteLine(db.Books
                    .Include(u => u.Users.Where(u => u.Email == userEntity.Email))
                    .Select(b => b.Title == bookEntity.Title).ToQueryString());

                //return db.Users
                //   .Include(b => b.Books)
                //   .Where(u => u.Email == userEntity.Email)
                //   .Select(b => b.Books.Where(b => b.Title == bookEntity.Title).Select(a => a.Id)).ToList().Any();

                return db.Books
                    .Include(u => u.Users.Where(u => u.Email == userEntity.Email))
                    .Select(b => b.Title == bookEntity.Title).ToList().Any();
            }
        }

        public int GetBooksCountHasUser(UserEntity userEntity)
        {
            //Task 6 Получить количество книг на руках у пользователя
            using (var db = new ExtendedDBContext(false))
            {
                return db.Users.Include(b => b.Books)
                .Where(u => u.Email == userEntity.Email)
                .Select(b => b.Books.Count).First();
            };
        }
    }
}
