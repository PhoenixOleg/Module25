using Azure;
using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.BLL.Services;
using Module25.DAL.Entities;
using Module25.PLL.Helpers;
using SocialNetwork.PLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.PLL.Views
{
    public class CRUDView_Author
    {
        AuthorService authorService = new();
        BookService bookService = new();

        public void Show()
        {
            string answer;
            Author? selectedAuthor = null;
            Book? selectedBook = null;

            do
            {
                Console.WriteLine("\nКниги:");
                Console.WriteLine("\t1. Получить все книги (нажмите 1)");
                Console.WriteLine("\t2. Получить (выбрать) книгу по ID (нажмите 2)");

                Console.WriteLine("\nРабота с авторами произведений:");
                Console.WriteLine("\t3. Получить всех авторов (нажмите 3)");
                Console.WriteLine("\t4. Получить (выбрать) автора по ID (нажмите 4)");
                Console.WriteLine("\t5. Добавить автора (нажмите 5)");
                Console.WriteLine("\t6. Удалить автора (нажмите 6)");
                Console.WriteLine("\n\t7. Добавить автора в произведение (нажмите 7)");
                Console.WriteLine("\t8. Удалить автора из произведения (нажмите 8)");
                Console.WriteLine("0. Вернуться назад (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1": //Получить все книги
                        {
                            try
                            {
                                Console.SetWindowSize(180, Console.WindowHeight);
                                Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", "ID", "Название", "Описание", "Год", "Автор(ы)", "Жанр");
                                foreach (Book item in bookService.ShowAll_Extended())
                                {
                                    Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", item.Id, item.Title, item.Description, item.PublicationDate.Year, string.Join(" ", item.Authors.Select(a => a.Surname + " " + a.Name + " " + a.MiddleName).ToArray()), string.Join(" ", item.Genres.Select(g => g.Name).ToArray()));
                                }
                            }
                            catch (NoOneObjectException)
                            {
                                AlertMessage.Show("Список книг пуст");
                            }
                            catch (Exception ex)
                            {
                                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
                            }

                            break;
                        }

                    case "2": //Получить (выбрать) книгу по ID
                        {
                            bool flag;
                            int id;

                            do
                            {
                                Console.Write("Введите ID книги: ");
                                flag = int.TryParse(Console.ReadLine(), out id);
                            }
                            while (flag == false);

                            selectedBook = null;

                            try
                            {
                                selectedBook = bookService.ShowByID_Extended(id);
                                Console.SetWindowSize(180, Console.WindowHeight);
                                Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", "ID", "Название", "Описание", "Год", "Автор(ы)", "Жанр");
                                Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", selectedBook.Id, selectedBook.Title, selectedBook.Description, selectedBook.PublicationDate.Year, string.Join(" ", selectedBook.Authors.Select(a => a.Surname + " " + a.Name + " " + a.MiddleName).ToArray()), string.Join(" ", selectedBook.Genres.Select(g => g.Name).ToArray()));
                                Console.WriteLine();
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

                    case "3": //Получить всех авторов
                        {
                            try
                            {
                                List<Author> authors = authorService.ShowAll();

                                Console.WriteLine("Список авторов:");
                                Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", "ID", "Имя автора", "Отчество автора", "Фамилия автора");
                                foreach (Author item in authors)
                                {
                                    Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", item.Id, item.Name, item.MiddleName, item.SurName);
                                }
                                Console.WriteLine();
                            }

                            catch (NoOneObjectException)
                            {
                                AlertMessage.Show("Справочник авторов пуст");
                            }

                            catch (Exception ex)
                            {
                                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
                            }
                            break;
                        }

                    case "4": //Получить (выбрать) автора по ID
                        {
                            bool flag;
                            int id;

                            do
                            {
                                Console.Write("Введите ID автора: ");
                                flag = int.TryParse(Console.ReadLine(), out id);
                            }
                            while (flag == false);

                            selectedAuthor = null;
                            try
                            {
                                selectedAuthor = authorService.ShowByID(id);
                                Console.WriteLine("Выбранный автор:");
                                Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", "ID", "Имя автора", "Отчество автора", "Фамилия автора");
                                Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", selectedAuthor.Id, selectedAuthor.Name, selectedAuthor.MiddleName, selectedAuthor.SurName);
                                Console.WriteLine();
                            }
                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("Автор не найден");
                            }
                            catch (Exception ex)
                            {
                                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
                            }
                            break;
                        }

                    case "5": //Добавление автора
                        {
                            AuthorAddingData authorAddingData = new();

                            Console.Write("Введите имя автора: ");
                            authorAddingData.Name = Console.ReadLine();

                            Console.Write("Введите фамилию автора: ");
                            authorAddingData.Surname = Console.ReadLine();

                            Console.Write("Введите отчество автора (если есть): ");
                            authorAddingData.MiddleName = Console.ReadLine();

                            try
                            {
                                authorService.AddAuthor(authorAddingData);
                                SuccessMessage.Show("Автор " + authorAddingData.Name + " " + authorAddingData.Surname + " успешно добавлен в справочник");
                            }
                            catch (NameEmptyException)
                            {
                                AlertMessage.Show("Имя и фамилия автора должны быть заданы.");
                            }
                            catch (AlreadyExistsException)
                            {
                                AlertMessage.Show("В процессе добавления нового автора произошла ошибка:\nАвтор с таким ФИО уже существует");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе добавления нового автора произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе добавления нового автора произошла ошибка:\n" + ex.Message);
                                }
                            };
                            break;
                        }

                    case "6": //Удаление выбранного автора
                        {
                            if (selectedAuthor != null)
                            {
                                Console.WriteLine("Выбранный автор:");
                                Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", "ID", "Имя автора", "Отчество автора", "Фамилия автора");
                                Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", selectedAuthor.Id, selectedAuthor.Name, selectedAuthor.MiddleName, selectedAuthor.SurName);
                                Console.WriteLine();

                                bool flag = false;
                                do
                                {
                                    Console.Write("Вы уверены, что хотите удалить этого автора? При этом он удалится из всех(!) произведений (Y/N) ");
                                    switch (Console.ReadLine()?.ToUpper())
                                    {
                                        case "Y":
                                            {
                                                flag = true;
                                                try
                                                {
                                                    authorService.RemoveAuthor(selectedAuthor);
                                                    SuccessMessage.Show("Автор " + selectedAuthor?.Name + " " + selectedAuthor?.SurName + " успешно удален");
                                                    selectedAuthor = null;
                                                }
                                                catch (NoOneObjectException)
                                                {
                                                    //Передан Null
                                                    AlertMessage.Show("В процессе удаления автора произошла ошибка:\nАвтор не задан");
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.InnerException != null)
                                                    {
                                                        AlertMessage.Show("В процессе удаления автора произошла ошибка:\n" + ex.InnerException.Message);
                                                    }
                                                    else
                                                    {
                                                        AlertMessage.Show("В процессе удаления автора произошла ошибка:\n" + ex.Message);
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
                                AlertMessage.Show("Сначала следует выбрать автора по ID");
                            }
                            break;
                        }

                    case "7": //Добавить автора в произведение
                        {
                            if (selectedBook != null)
                            {
                                Console.SetWindowSize(180, Console.WindowHeight);
                                Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", "ID", "Название", "Описание", "Год", "Автор(ы)", "Жанр");
                                Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", selectedBook.Id, selectedBook.Title, selectedBook.Description, selectedBook.PublicationDate.Year, string.Join(" ", selectedBook.Authors.Select(a => a.Surname + " " + a.Name + " " + a.MiddleName).ToArray()), string.Join(" ", selectedBook.Genres.Select(g => g.Name).ToArray()));
                                Console.WriteLine();
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать книгу по ID");
                            }

                            if (selectedAuthor != null)
                            {
                                Console.WriteLine("Выбранный автор:");
                                Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", "ID", "Имя автора", "Отчество автора", "Фамилия автора");
                                Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", selectedAuthor.Id, selectedAuthor.Name, selectedAuthor.MiddleName, selectedAuthor.SurName);
                                Console.WriteLine();
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать автора по ID");
                            }

                            try
                            {
                                authorService.AddAuthorToBook(selectedAuthor, selectedBook);
                                SuccessMessage.Show("Автор " + selectedAuthor.Name + " " + selectedAuthor.SurName + " успешно добавлен в книгу " + selectedBook.Title);
                                selectedAuthor = null;
                                selectedBook = null;
                            }

                            catch (AlreadyExistsException)
                            {
                                AlertMessage.Show("В процессе добавления автора в книгу произошла ошибка:\n" + selectedAuthor?.Name + " " + selectedAuthor?.SurName + " уже является автором книги " + selectedBook?.Title);
                            }

                            catch (NullReferenceException ex)
                            {
                                AlertMessage.Show("В процессе добавления автора в книгу произошла ошибка:\n" + ex.Message);
                            }

                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе добавления автора в книгу произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе добавления автора в книгу произошла ошибка:\n" + ex.Message);
                                }
                            }
                            break;
                        }

                    case "8": //Удалить автора из произведения
                        {
                            if (selectedBook != null)
                            {
                                Console.SetWindowSize(180, Console.WindowHeight);
                                Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", "ID", "Название", "Описание", "Год", "Автор(ы)", "Жанр");
                                Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", selectedBook.Id, selectedBook.Title, selectedBook.Description, selectedBook.PublicationDate.Year, string.Join(" ", selectedBook.Authors.Select(a => a.Surname + " " + a.Name + " " + a.MiddleName).ToArray()), string.Join(" ", selectedBook.Genres.Select(g => g.Name).ToArray()));
                                Console.WriteLine();
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать книгу по ID");
                            }

                            if (selectedAuthor != null)
                            {
                                Console.WriteLine("Выбранный автор:");
                                Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", "ID", "Имя автора", "Отчество автора", "Фамилия автора");
                                Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", selectedAuthor.Id, selectedAuthor.Name, selectedAuthor.MiddleName, selectedAuthor.SurName);
                                Console.WriteLine();
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать автора по ID");
                            }

                            try
                            {
                                authorService.DelAuthorFromBook(selectedAuthor, selectedBook);
                                SuccessMessage.Show("Автор " + selectedAuthor.Name + " " + selectedAuthor.SurName + " успешно удален из книги " + selectedBook.Title);
                                selectedAuthor = null;
                                selectedBook = null;
                            }

                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("В процессе удаления автора из книги произошла ошибка:\n" + selectedAuthor?.Name + " " + selectedAuthor?.SurName + " не является автором книги " + selectedBook?.Title);
                            }

                            catch (NullReferenceException ex)
                            {
                                AlertMessage.Show("В процессе удаления автора из книги произошла ошибка:\n" + ex.Message);
                            }

                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе удаления автора из книги произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе удаления автора из книги произошла ошибка:\n" + ex.Message);
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
