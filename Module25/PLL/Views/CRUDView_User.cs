using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.BLL.Services;
using Module25.PLL.Helpers;
using SocialNetwork.PLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.PLL.Views
{
    public class CRUDView_User
    {
        UserService userService = new();
        CommonView commonView = new();

        public void Show()
        {
            string answer;
            User? selectedUser = null;
            
            do
            {
                Console.WriteLine("\nПолучить всех пользователей (нажмите 1)");
                Console.WriteLine("Получить (выбрать) пользователя по ID (нажмите 2)");
                Console.WriteLine("Добавить пользователя (нажмите 3)");
                Console.WriteLine("Удалить пользователя (нажмите 4)");
                Console.WriteLine("Удалить пользователя по ID (нажмите 5)");
                Console.WriteLine("Изменить имя пользователя (нажмите 6)");
                Console.WriteLine("Вернуться назад (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1": //Вывод данных о всех пользователях
                        {
                            commonView.Show_AllUsers();
                            break;
                        }

                    case "2": //Вывод данных о пользователе (выбор пользователя) по ID
                        {
                            int id = commonView.InputID("пользователя");

                            selectedUser = null; //Сброс ранее выбранного пользователя на случай ошибки при выборе
                            try
                            {
                                selectedUser = userService.ShowByID(id);
                                commonView.Show_SelectedUser(selectedUser);
                            }
                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("Пользователь не найден");
                            }
                            catch (Exception ex)
                            {
                                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
                            }
                            break;
                        }

                    case "3": //Добавление пользователя
                        {
                            UserRegistrationData userRegistrationData = new();
                            Console.Write("Введите имя пользователя: ");
                            userRegistrationData.Name = Console.ReadLine();
                            
                            Console.Write("Введите электронную почту пользователя: ");
                            userRegistrationData.Email = Console.ReadLine();

                            try
                            {
                                userService.AddUser(userRegistrationData);
                                SuccessMessage.Show("Пользователь " + userRegistrationData.Name + " успешно зарегистрирован");
                            }
                            catch (NameEmptyException)
                            {
                                AlertMessage.Show("Имя пользователя задано неверно.");
                            }                                                           
                            catch (EMailEmptyException)
                            {
                                AlertMessage.Show("Электронная почта пользователя задано неверно.");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null) //Смотрим наличие внутреннего исключения для перехвата ошибки уникальности поля eMail
                                {
                                    if (ex.InnerException.HResult == -2146232060)
                                    {
                                        AlertMessage.Show("В процессе регистрации нового пользователя произошла ошибка:\nПользователь с таким eMail уже существует");
                                    }
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе регистрации нового пользователя произошла ошибка:\n" + ex.Message);
                                }                                
                            };
                            break;
                        }

                    case "4": //Удаление выбранного пользователя
                        {
                            if (selectedUser != null)
                            {
                                commonView.Show_SelectedUser(selectedUser);

                                bool flag = false;
                                do
                                {
                                    Console.Write("Вы уверены, что хотите удалить этого пользователя? (Y/N) ");
                                    switch (Console.ReadLine()?.ToUpper())
                                    {
                                        case "Y":
                                            {
                                                flag = true;
                                                try
                                                {
                                                    userService.RemoveUser(selectedUser);
                                                    SuccessMessage.Show("Пользователь " + selectedUser?.Name + " успешно удален");
                                                    selectedUser = null;
                                                }
                                                catch (NoOneObjectException)
                                                {
                                                    //Передан Null
                                                    AlertMessage.Show("В процессе удаления пользователя произошла ошибка:\nПользователь не задан");
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.InnerException != null)
                                                    {
                                                        AlertMessage.Show("В процессе удаления пользователя произошла ошибка:\n" + ex.InnerException.Message);
                                                    }
                                                    else
                                                    {
                                                        AlertMessage.Show("В процессе удаления пользователя произошла ошибка:\n" + ex.Message);
                                                    }                                                    
                                                }
                                                break;
                                            }
                                        case "N":
                                            {
                                                flag = true;
                                                break;
                                            }
                                    }
                                }
                                while (flag != true);
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать пользователя по ID");
                            }
                            break;
                        }

                    case "5": //Удаление пользователя по ID
                        {
                            int id = commonView.InputID("пользователя для удаления");

                            try
                            {
                                userService.RemoveUser(id);
                                SuccessMessage.Show("Пользователь с ID = " + id + " успешно удален");
                            }
                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("В процессе удаления пользователя произошла ошибка:\nПользователь не найден");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе удаления пользователя произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе удаления пользователя произошла ошибка:\n" + ex.Message);
                                }
                            }
                            break;
                        }

                    case "6": //Изменение имени пользователя по ID
                        {
                            string name;

                            int id = commonView.InputID("пользователя для изменений имени");

                            Console.Write("Введите новое имя пользователя: ");
                            name = Console.ReadLine();

                            try
                            {
                                userService.UpdateUserNameByID(id, name);
                                SuccessMessage.Show("Имя пользователя с ID = " + id + " успешно обновлено");
                            }
                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("В процессе изменения имени пользователя произошла ошибка:\nПользователь не найден");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе изменения имени пользователя произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе изменения имени пользователя произошла ошибка:\n" + ex.Message);
                                }
                            }
                            break;
                        }
                }
            }
            while (answer != "0");
        }
    }
}

