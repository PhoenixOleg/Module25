using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.DAL.Entities;
using Module25.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Services
{
    public class UserService
    {
        UserRepository userRepository;

        public UserService() 
        {
            userRepository = new UserRepository();
        }

        /// <summary>
        /// Метод возвращения списка всех пользователей слоя BLL
        /// </summary>
        /// <returns>Список экземпляров User</returns>
        /// <exception cref="NoOneObjectException">Список пуст</exception>
        public List<User> ShowAll()
        {
            List<User> usersList = new();
               
            List<UserEntity> findUsers = userRepository.GetAllUser();
            if (findUsers.Count  == 0)
            { 
                throw new NoOneObjectException(); 
            }
            else
            {
                foreach (UserEntity userEntity in findUsers)
                {
                    usersList.Add(ConstructUserModel(userEntity));
                }
            }

            return usersList;
        }

        /// <summary>
        /// Метод получения пользователя по ID слоя BLL
        /// </summary>
        /// <param name="id">ID автора</param>
        /// <returns>Экземпляр User со сведениями о пользователе</returns>
        /// <exception cref="ObjectNotFoundException">Пользователь не найден</exception>
        public User ShowByID(int id)
        {
            UserEntity findUser = userRepository.GetUserByID(id);
            if (findUser == null)
            { throw new ObjectNotFoundException(); }

            return ConstructUserModel(findUser);
        }

        /// <summary>
        /// Метод добавления пользователя в БД слоя BLL
        /// </summary>
        /// <param name="userRegistrationData">Экземпляр UserRegistrationData</param>
        /// <exception cref="NameEmptyException">Имя или фамилия пользователя не указаны</exception>
        /// <exception cref="EMailEmptyException">Электронный адрес пользователя не указан или некорректен</exception>
        /// <exception cref="Exception">Пользователь не добавлен по иным причинам</exception>
        public void AddUser(UserRegistrationData userRegistrationData)
        {
            if (string.IsNullOrEmpty(userRegistrationData.Name) || string.IsNullOrWhiteSpace (userRegistrationData.Name))
            {
                throw new NameEmptyException();
            }

            if (string.IsNullOrEmpty(userRegistrationData.Email) || string.IsNullOrWhiteSpace(userRegistrationData.Email))
            {
                throw new EMailEmptyException();
            }

            if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
                throw new EMailEmptyException();

            UserEntity userEntity = new()
            { 
               Name = userRegistrationData.Name,
               Email = userRegistrationData.Email
            };

            if (userRepository.AddUser(userEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Метод удаления пользователя из БД слоя BLL
        /// </summary>
        /// <param name="user">Экземпляр User</param>
        /// <exception cref="NoOneObjectException">Пользователь не задан</exception>
        /// <exception cref="Exception">Пользователь не удален по иным причинам</exception>
        public void RemoveUser(User? user) 
        { 
        if (user == null)
            {
                throw new NoOneObjectException();
            }

            UserEntity userEntity = ConvertToUserEntity(user);

            if (userRepository.DeleteUser(userEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Метод удаления пользователя из БД по его ID слоя BLL
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <exception cref="ObjectNotFoundException">Пользователь не найден</exception>
        /// <exception cref="Exception">Пользователь не удален по иным причинам</exception>
        public void RemoveUser(int id)
        {
            switch (userRepository.DeleteUser(id))
                {
                case -1:
                    {
                        throw new ObjectNotFoundException();
                    }
                case 0:
                    {
                        throw new Exception();
                    }                    
            }
        }

        /// <summary>
        /// Метод изменения имени пользователя по его ID слоя BLL
        /// </summary>
        /// <param name="id">ID пользователя</param>
        /// <param name="userName">Новое имя пользователя</param>
        /// <exception cref="NameEmptyException">Новое имя не задано</exception>
        /// <exception cref="ObjectNotFoundException">Пользователь не найден</exception>
        /// <exception cref="Exception">Имя пользователя не обновлено по иным причинам</exception>
        public void UpdateUserNameByID(int id, string userName)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrWhiteSpace(userName))
            {
                throw new NameEmptyException();
            }

            switch (userRepository.UpdateUserNameByID(id, userName))
            {
                case -1:
                    {
                        throw new ObjectNotFoundException();
                    }
                case 0:
                    {
                        throw new Exception();
                    }
            }
        }

        /// <summary>
        /// Метод выдачи книги "на рукаи" пользователю слоя BLL
        /// </summary>
        /// <param name="user">Экземпляр User</param>
        /// <param name="book">Экземпляр Book</param>
        /// <exception cref="NullReferenceException">Книга или пользователь не заданы</exception>
        /// <exception cref="AlreadyExistsException">Книга уже "на руках" у пользователя</exception>
        /// <exception cref="Exception">Книга не выдана пользователю по иным причинам</exception>
        public void GiveBookToUser(User user, Book book)
        {
            if (user == null)
            {
                throw new NullReferenceException("Клиент не выбран");
            }

            if (book == null)
            {
                throw new NullReferenceException("Книга не выбрана");
            }

            if (book.Users.Where(u => u.Id == user.Id).Any())
            {
                throw new AlreadyExistsException();
            }

            UserExtendedEntity userExtendedEntity = new()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            BookExtendedEntity bookExtendedEntity = new()
            {
                Id = book.Id,
                Description = book.Description,
                PublicationDate = book.PublicationDate,
                Title = book.Title,
                Users = new() 
            };


            if (userRepository.GiveBookToUser(userExtendedEntity, bookExtendedEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Метод "сдачи" книги пользователем в библиотеку слоя BLL
        /// </summary>
        /// <param name="user">Экземпляр User</param>
        /// <param name="book">Экземпляр Book</param>
        /// <exception cref="NullReferenceException">Книга или пользователь не заданы</exception>
        /// <exception cref="ObjectNotFoundException">Пользователь не подписан на эту книгу</exception>
        /// <exception cref="Exception">Книга не возвращена по иным причинам</exception>
        public void GetBookFromUser(User user, Book book)
        {
            if (user == null)
            {
                throw new NullReferenceException("Клиент не выбран");
            }

            if (book == null)
            {
                throw new NullReferenceException("Книга не выбрана");
            }

            if (!book.Users.Where(u => u.Id == user.Id).Any()) //Читатель не существует в книге
            {
                throw new ObjectNotFoundException();
            }

            UserExtendedEntity userExtendedEntity = new()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            BookExtendedEntity bookExtendedEntity = new()
            {
                Id = book.Id,
                Description = book.Description,
                PublicationDate = book.PublicationDate,
                Title = book.Title,
                Users = new()
            };

            if (userRepository.GetBookFromUser(userExtendedEntity, bookExtendedEntity) == 0)
            {
                throw new Exception();
            }

        }

        /// <summary>
        /// Метод проверки наличия книги "на руках" у пользователем по eMail пользователя и названию книги слоя BLL
        /// </summary>
        /// <param name="bookAddingData">Экземпляр BookAddingData (название книги)</param>
        /// <param name="userRegistrationData">Экземпляр UserRegistrationData (eMail пользователя)</param>
        /// <returns>true - книга у пользователя, 
        /// false - книге нет у пользователя</returns>
        /// <exception cref="EMailEmptyException">Электронный адрес пользователя не указан или некорректен</exception>
        /// <exception cref="NameEmptyException">Название книги не задано</exception>
        public bool HaveUserBookByTitle(BookAddingData bookAddingData, UserRegistrationData userRegistrationData)
        {
            if (string.IsNullOrEmpty(userRegistrationData.Email) || string.IsNullOrWhiteSpace(userRegistrationData.Email))
            {
                throw new EMailEmptyException();
            }

            if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
                throw new EMailEmptyException();

            UserEntity userEntity = new()
            {
                Name = userRegistrationData.Name,
                Email = userRegistrationData.Email
            };

            if (string.IsNullOrEmpty(bookAddingData.Title) || string.IsNullOrWhiteSpace(bookAddingData.Title))
            {
                throw new NameEmptyException();
            }

            BookExtendedEntity bookExtendedEntity = new()
            {
                Title = bookAddingData.Title
            };

            return userRepository.HaveUserBookByTitle(bookExtendedEntity, userEntity);
        }

        /// <summary>
        /// Получение количества книг "на руках" у пользователя
        /// </summary>
        /// <param name="userRegistrationData">Экземпляр UserRegistrationData (eMail пользователя)</param>
        /// <returns>Количество книг у пользователя</returns>
        /// <exception cref="EMailEmptyException">Электронный адрес пользователя не указан или некорректен</exception>
        public int GetBooksCountHasUser(UserRegistrationData userRegistrationData)
        {
            if (string.IsNullOrEmpty(userRegistrationData.Email) || string.IsNullOrWhiteSpace(userRegistrationData.Email))
            {
                throw new EMailEmptyException();
            }

            if (!new EmailAddressAttribute().IsValid(userRegistrationData.Email))
                throw new EMailEmptyException();

            UserEntity userEntity = new()
            {
                Name = userRegistrationData.Name,
                Email = userRegistrationData.Email
            };

            return userRepository.GetBooksCountHasUser(userEntity);
        }

        /// <summary>
        /// Создание User из UserEntity при возврате с уровня DAL
        /// </summary>
        /// <param name="userEntity">Экземпляр UserEntity</param>
        /// <returns>Экземпляр User</returns>
        public User ConstructUserModel(UserEntity userEntity)
        {
            return new User(userEntity.Id
                , userEntity.Name
                , userEntity.Email
                );
        }

        /// <summary>
        /// Создание UserEntity из User для передачи на уровень DAL
        /// </summary>
        /// <param name="user">Экземпляр Use</param>
        /// <returns>Экземпляр UserEntity</returns>
        public UserEntity ConvertToUserEntity(User user) 
        {
            UserEntity userEntity = new()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return userEntity;
        }
    }
}
