using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.Task_25_2_4.DAL.Entities;
using Module25.Task_25_2_4.DAL.Repositories;
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
               Name = userRegistrationData.Name
             , Email = userRegistrationData.Email
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
