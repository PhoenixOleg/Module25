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
    public class CRUDView_Genre
    {
        GenreService genreService = new();
        BookService bookService = new();
        
        public void Show()
        {
            string answer;
            Genre? selectedGenre = null;
            Book? selectedBook = null;

            do
            {
                Console.WriteLine("\nКниги:");
                Console.WriteLine("\t1. Получить все книги (нажмите 1)");
                Console.WriteLine("\t2. Получить (выбрать) книгу по ID (нажмите 2)");

                Console.WriteLine("\nРабота с жанрами произведений:");
                Console.WriteLine("\t3. Получить все жанры (нажмите 3)");
                Console.WriteLine("\t4. Получить (выбрать) жанр по ID (нажмите 4)");
                Console.WriteLine("\t5. Добавить жанр (нажмите 5)");
                Console.WriteLine("\t6. Удалить жанр (нажмите 6)");
                Console.WriteLine("\n\t7. Добавить жанр в произведение (нажмите 7)");
                Console.WriteLine("\t8. Удалить жанр из произведения (нажмите 8)");
                Console.WriteLine("0. Вернуться назад (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1":
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

                    case "3": //Получить все жанры
                        {
                            try
                            {
                                List<Genre> genres = genreService.ShowAll();

                                Console.WriteLine("Список жанров:");
                                Console.WriteLine("| {0, 4} | {1, 15} |", "ID", "Жанр");
                                foreach (Genre item in genres)
                                {
                                    Console.WriteLine("| {0, 4} | {1, 15} |", item.Id, item.Name);
                                }
                                Console.WriteLine();
                            }

                            catch (NoOneObjectException)
                            {
                                AlertMessage.Show("Справочник жанров пуст");
                            }

                            catch (Exception ex)
                            {
                                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
                            }
                            break;
                        }

                    case "4": //Получить (выбрать) жанр по ID
                        {
                            bool flag;
                            int id;

                            do
                            {
                                Console.Write("Введите ID жанра: ");
                                flag = int.TryParse(Console.ReadLine(), out id);
                            }
                            while (flag == false);

                            selectedGenre = null;
                            try
                            {
                                selectedGenre = genreService.ShowByID(id);
                                Console.WriteLine("Выбранный жанр:");
                                Console.WriteLine("| {0, 4} | {1, 15} |", "ID", "Жанр");
                                Console.WriteLine("| {0, 4} | {1, 15} |", selectedGenre.Id, selectedGenre.Name);
                                Console.WriteLine();
                            }
                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("Жанр не найден");
                            }
                            catch (Exception ex)
                            {
                                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
                            }
                            break;
                        }

                    case "5": //Добавление жанра
                        {
                            GenreAddingData genreAddingData = new();

                            Console.Write("Введите название жанра: ");
                            genreAddingData.Name = Console.ReadLine();

                            try
                            {
                                genreService.AddGenre(genreAddingData);
                                SuccessMessage.Show("Жанр " + genreAddingData.Name + " успешно добавлен в справочник");
                            }
                            catch (NameEmptyException)
                            {
                                AlertMessage.Show("Название жанра должны быть задано.");
                            }
                            catch (AlreadyExistsException)
                            {
                                AlertMessage.Show("В процессе добавления нового жанра произошла ошибка:\nжанр с таким названием уже существует");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе добавления нового жанра произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе добавления нового жанра произошла ошибка:\n" + ex.Message);
                                }
                            };
                            break;
                        }

                    case "6": //Удаление выбранного жанра
                        {
                            if (selectedGenre != null)
                            {
                                Console.WriteLine("Выбранный жанр:");
                                Console.WriteLine("| {0, 4} | {1, 15} |", "ID", "Жанр");
                                Console.WriteLine("| {0, 4} | {1, 15} |", selectedGenre.Id, selectedGenre.Name);
                                Console.WriteLine();

                                bool flag = false;
                                do
                                {
                                    Console.Write("Вы уверены, что хотите удалить этот жанр? При этом он удалится из всех(!) произведений (Y/N) ");
                                    switch (Console.ReadLine()?.ToUpper())
                                    {
                                        case "Y":
                                            {
                                                flag = true;
                                                try
                                                {
                                                    genreService.RemoveGenre(selectedGenre);
                                                    SuccessMessage.Show("Жанр " + selectedGenre?.Name + " успешно удален");
                                                    selectedGenre = null;
                                                }
                                                catch (NoOneObjectException)
                                                {
                                                    //Передан Null
                                                    AlertMessage.Show("В процессе удаления жанра произошла ошибка:\nЖанр не задан");
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.InnerException != null)
                                                    {
                                                        AlertMessage.Show("В процессе удаления жанра произошла ошибка:\n" + ex.InnerException.Message);
                                                    }
                                                    else
                                                    {
                                                        AlertMessage.Show("В процессе удаления жанра произошла ошибка:\n" + ex.Message);
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
                                AlertMessage.Show("Сначала следует выбрать жанр по ID");
                            }
                            break;
                        }

                    case "7": //Добавить жанр в произведение
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

                            if (selectedGenre != null)
                            {
                                Console.WriteLine("Выбранный жанр:");
                                Console.WriteLine("| {0, 4} | {1, 15} |", "ID", "Жанр");
                                Console.WriteLine("| {0, 4} | {1, 15} |", selectedGenre.Id, selectedGenre.Name);
                                Console.WriteLine();
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать жанр по ID");
                            }

                            try
                            {
                                genreService.AddGenreToBook(selectedGenre, selectedBook);
                                SuccessMessage.Show("Жанр " + selectedGenre.Name  + " успешно добавлен в книгу " + selectedBook.Title);
                                selectedGenre = null;
                                selectedBook = null;
                            }

                            catch (AlreadyExistsException)
                            {
                                AlertMessage.Show("В процессе добавления жанра в книгу произошла ошибка:\n" + selectedGenre?.Name + " уже является жанром книги " + selectedBook?.Title);
                            }

                            catch (NullReferenceException ex)
                            {
                                AlertMessage.Show("В процессе добавления жанра в книгу произошла ошибка:\n" + ex.Message);
                            }

                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе добавления жанра в книгу произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе добавления жанра в книгу произошла ошибка:\n" + ex.Message);
                                }
                            }
                            break;
                        }

                    case "8": //Удалить жанр из произведения
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

                            if (selectedGenre != null)
                            {
                                Console.WriteLine("Выбранный жанр:");
                                Console.WriteLine("| {0, 4} | {1, 15} |", "ID", "Жанр");
                                Console.WriteLine("| {0, 4} | {1, 15} |", selectedGenre.Id, selectedGenre.Name);
                                Console.WriteLine();
                            }
                            else
                            {
                                AlertMessage.Show("Сначала следует выбрать жанр по ID");
                            }

                            try
                            {
                                genreService.DelGenreFromBook(selectedGenre, selectedBook);
                                SuccessMessage.Show("Жанр " + selectedGenre.Name + " успешно удален из книги " + selectedBook.Title);
                                selectedGenre = null;
                                selectedBook = null;
                            }

                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("В процессе удаления жанра из книги произошла ошибка:\n" + selectedGenre?.Name + " не является жанром книги " + selectedBook?.Title);
                            }

                            catch (NullReferenceException ex)
                            {
                                AlertMessage.Show("В процессе удаления жанра из книги произошла ошибка:\n" + ex.Message);
                            }

                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе удаления жанра из книги произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе удаления жанра из книги произошла ошибка:\n" + ex.Message);
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
