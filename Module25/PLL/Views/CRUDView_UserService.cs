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
    public class CRUDView_UserService
    {
        UserService userService = new();
        BookService bookService = new();

        CommonView commonView = new();
        public void Show()
        {
            string answer;
            User? selectedUser = null;
            Book? selectedBook = null;

            do
            {
                Console.WriteLine("\nКниги:");
                Console.WriteLine("\t1. Получить все книги (нажмите 1)");
                Console.WriteLine("\t2. Получить (выбрать) книгу по ID (нажмите 2)");

                Console.WriteLine("\nРабота с клиентами библиотеки:");
                Console.WriteLine("\t3. Получить всех клиентов (нажмите 3)");
                Console.WriteLine("\t4. Получить (выбрать) клиента по ID (нажмите 4)");
                Console.WriteLine("\t5. Добавить клиента (нажмите 5)");
                Console.WriteLine("\t6. Удалить клиента (нажмите 6)");
                Console.WriteLine("\n\t7. Выдать клиенту произведение (нажмите 7)");
                Console.WriteLine("\t8. Сдать произведение (нажмите 8)");
                Console.WriteLine("0. Вернуться назад (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1": //Получить все книги
                        {
                            commonView.Show_AllBooks(true);
                            break;
                        }

                    case "2": //Получить (выбрать) книгу по ID
                        {
                            int id = commonView.InputID("книги");

                            selectedBook = null;

                            try
                            {
                                selectedBook = bookService.ShowByID_Extended(id);
                                commonView.Show_SelectedBook(selectedBook);
                            }
                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("Книга не найдена");
                            }
                            catch (Exception ex)
                            {
                                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
                            }

                            break;
                        }

                    case "3": //Вывод данных о всех пользователях
                        {
                            commonView.Show_AllUsers();
                            break;
                        }

                    case "4": //Вывод данных о пользователе (выбор пользователя) по ID
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

                    case "5": //Добавление пользователя
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

                    case "6": //Удаление выбранного пользователя
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

                    case "7": //Выдать клиенту произведение
                        {
                            if (selectedBook != null)
                            {
                                commonView.Show_SelectedBook(selectedBook);
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать книгу по ID");
                            }

                            if (selectedUser != null)
                            {
                                commonView.Show_SelectedUser(selectedUser);
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать пользователя по ID");
                            }

                            if (selectedBook != null && selectedUser != null)
                            {
                                try
                                {
                                    userService.GiveBookToUser(selectedUser, selectedBook);
                                    SuccessMessage.Show("Клиенту " + selectedUser.Name + " (" + selectedUser.Email + ") успешно выдана в книга " + selectedBook.Title);
                                    selectedUser = null;
                                    selectedBook = null;
                                }
                                catch (AlreadyExistsException)
                                {
                                    AlertMessage.Show("В процессе выдачи книги произошла ошибка:\n" + selectedUser?.Name + " (" + selectedUser?.Email + ") уже читает книгу " + selectedBook?.Title);
                                }

                                catch (NullReferenceException ex)
                                {
                                    AlertMessage.Show("В процессе выдачи книги произошла ошибка:\n" + ex.Message);
                                }

                                catch (Exception ex)
                                {
                                    if (ex.InnerException != null)
                                    {
                                        AlertMessage.Show("В процессе выдачи книги произошла ошибка:\n" + ex.InnerException.Message);
                                    }
                                    else
                                    {
                                        AlertMessage.Show("В процессе выдачи книги произошла ошибка:\n" + ex.Message);
                                    }
                                }
                            }
                            break;
                        }

                    case "8": //Сдать произведение
                        {
                            if (selectedBook != null)
                            {
                                commonView.Show_SelectedBook(selectedBook);
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать книгу по ID");
                            }

                            if (selectedUser != null)
                            {
                                commonView.Show_SelectedUser(selectedUser);
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать пользователя по ID");
                            }

                            if (selectedBook != null && selectedUser != null)
                            {
                                try
                                {
                                    userService.GetBookFromUser(selectedUser, selectedBook);
                                    SuccessMessage.Show("Клиент " + selectedUser.Name + " (" + selectedUser.Email + ") успешно сдал книгу " + selectedBook.Title);
                                    selectedUser = null;
                                    selectedBook = null;
                                }

                                catch (ObjectNotFoundException)
                                {
                                    AlertMessage.Show("В процессе возвращения книги в библиотеку произошла ошибка:\n" + "Клиент " + selectedUser?.Name + " (" + selectedUser?.Email + ") не является читателем книги " + selectedBook?.Title);
                                }

                                catch (NullReferenceException ex)
                                {
                                    AlertMessage.Show("В процессе возвращения книги в библиотеку произошла ошибка:\n" + ex.Message);
                                }

                                catch (Exception ex)
                                {
                                    if (ex.InnerException != null)
                                    {
                                        AlertMessage.Show("В процессе возвращения книги в библиотеку произошла ошибка:\n" + ex.InnerException.Message);
                                    }
                                    else
                                    {
                                        AlertMessage.Show("В процессе удаления автора из книги произошла ошибка:\n" + ex.Message);
                                    }
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
