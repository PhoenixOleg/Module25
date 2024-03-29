﻿using System;
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
using Module25.Task_25_2_4.DAL;

namespace Module25.DAL.Repositories
{
    public class UserRepository
    {
        /// <summary>
        /// Метод возвращает пользователя по его ID
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <returns>Экземпляр класса UserEntity или null</returns>
        public UserEntity? GetUserByID(int id)
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
        /// <param name="id">ID пользователя</param>
        /// <param name="name">Новое имя пользователя</param>
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


        /// <summary>
        /// Метод получения книгм пользователем "на руки" (подписка на книгу)
        /// </summary>
        /// <param name="userExtendedEntity">Экземпляр класса UserExtendedEntity</param>
        /// <param name="bookExtendedEntity">Экземпляр класса BookExtendedEntity</param>
        /// <returns>1 - пользователь успешно подписался на книгу - пользователь добавлен в ее читатели (количество обработанных строк); 
        /// 0 - пользователь не добавлен в читатели книги</returns>
        public int GiveBookToUser(UserExtendedEntity userExtendedEntity, BookExtendedEntity bookExtendedEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                bookExtendedEntity.Users.Add(userExtendedEntity);
                db.Update(bookExtendedEntity);
                return db.SaveChanges();
            }
        }

        /// <summary>
        /// Метод "сдачи" книги в библиотеку (отписка от книги)
        /// </summary>
        /// <param name="userExtendedEntity">Экземпляр класса UserExtendedEntity</param>
        /// <param name="bookExtendedEntity">Экземпляр класса BookExtendedEntity</param>
        /// <returns>1 - пользователь успешно отписался от книги - пользователь удален из ее читателей (количество обработанных строк); 
        /// 0 - пользователь не удален из читателей книги</returns>
        /// <exception cref="NullReferenceException">Не найдена книга или пользователь не подписан на нее</exception>
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

        /// <summary>
        /// Есть ли определенная книга на руках у пользователя (наличие подписки по названию книги) Task 5
        /// </summary>
        /// <param name="bookExtendedEntity">Экземпляр класса BookExtendedEntity (название книги)</param>
        /// <param name="userEntity">Экземпляр класса UserEntity (с eMail пользователя)</param>
        /// <returns>true - пользователь подписан на книгу,
        /// false - пользователь не подписан на книгу</returns>
        public bool HaveUserBookByTitle(BookExtendedEntity bookExtendedEntity, UserEntity userEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                ///Можно сделать сначала проверку на наличие книги или книг по названию
                BookExtendedEntity? book = db.Books.Where(b => b.Title == bookExtendedEntity.Title).FirstOrDefault();
                return db.Users
                   .Include(b => b.Books)
                   .Where(u => u.Email == userEntity.Email)
                   .Any(b => b.Books.Contains(db.Books.Where(b => b.Title == bookExtendedEntity.Title).FirstOrDefault()));
            }
        }

        /// <summary>
        /// Получить количество книг на руках у пользователя (/Task 6)
        /// </summary>
        /// <param name="userEntity">>Экземпляр класса UserEntity (с eMail пользователя)</param>
        /// <returns>Количество книг, на которые подписан пользователь</returns>
        public int GetBooksCountHasUser(UserEntity userEntity)
        {
            using (var db = new ExtendedDBContext(false))
            {
                return db.Users.Include(b => b.Books)
                .Where(u => u.Email == userEntity.Email)
                .Select(b => b.Books.Count).First();
            };
        }
    }
}
