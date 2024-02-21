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

        public User ShowByID(int id)
        {
            UserEntity findUser = userRepository.GetUserByID(id);
            if (findUser == null)
            { throw new ObjectNotFoundException(); }

            return ConstructUserModel(findUser);
        }

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

        public User ConstructUserModel(UserEntity userEntity)
        {
            return new User(userEntity.Id
                , userEntity.Name
                , userEntity.Email
                );
        }

        public UserEntity ConvertToUserEntity( User user ) 
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
