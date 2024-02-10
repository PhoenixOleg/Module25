using Module25.Task_25_2_4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module25.BLL.Exceptions;
using Module25.BLL.Models;

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
                List<UserEntity> userEntitys = db.Users.ToList();
                return userEntitys;
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
    }
}
