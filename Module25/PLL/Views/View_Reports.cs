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
    public class View_Reports
    {
        BookService bookService = new();

        public void Show()
        {
            string answer;
            do
            {
                Console.WriteLine("\nСправоченики:");
                Console.WriteLine("\tA. Получить список авторов - нажмите A");
                Console.WriteLine("\tB. Получить список книг - нажмите B");
                Console.WriteLine("\tG. Получить список жанров - нажмите G");
                Console.WriteLine("\tU. Получить список пользователей - нажмите U");

                Console.WriteLine("\nОтчеты:");
                Console.WriteLine("\t1. Получить список книг определенного жанра и вышедших между определенными годами - нажмите 1");
                Console.WriteLine("\t2. Получить количество книг определенного автора в библиотеке - нажмите 2");
                Console.WriteLine("\t3. Получить количество книг определенного жанра в библиотеке - нажмите 3");
                Console.WriteLine("\t4. Есть ли книга определенного автора и с определенным названием в библиотеке - нажмите 4");
                Console.WriteLine("\t5. Есть ли определенная книга на руках у пользователя - нажмите 5");
                Console.WriteLine("\t6. Получить количество книг на руках у пользователя - нажмите 6");
                Console.WriteLine("\t7. Получение последней вышедшей книги - нажмите 7");
                Console.WriteLine("\t8. Получение списка всех книг, отсортированного в алфавитном порядке по названию - нажмите 8");
                Console.WriteLine("\t9. Получение списка всех книг, отсортированного в порядке убывания года их выхода - нажмите 9");
                Console.WriteLine("\n0. Вернуться назад (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1": //Получить список книг определенного жанра и вышедших между определенными годами
                        {
                            //@@@

                            break;
                        }

                    case "2": //Получить количество книг определенного автора в библиотеке
                        {
                            AuthorAddingData authorAddingData = new();

                            Console.Write("Введите имя автора: ");
                            authorAddingData.Name = Console.ReadLine();

                            Console.Write("Введите отчество автора (если есть): ");
                            authorAddingData.MiddleName = Console.ReadLine();

                            Console.Write("Введите фамилию автора: ");
                            authorAddingData.Surname = Console.ReadLine();

                            try
                            {
                                SuccessMessage.Show("В библиотеке " + bookService.ShowCountBooksByAuthor(authorAddingData) + " книг(и,а) данного автора");
                            }
                            catch (NameEmptyException)
                            {
                                AlertMessage.Show("Имя и фамилия автора должны быть заданы.");
                            }
                            catch (Exception ex)
                            {
                                if (ex.Message == "Sequence contains no elements")
                                {
                                    AlertMessage.Show("Книги автора " + authorAddingData?.Name + " " + authorAddingData?.MiddleName + " " + authorAddingData?.Surname + " не найдены в библиотеке");
                                    break;
                                }

                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе поиска книг по автору произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе поиска книг по автору произошла ошибка:\n" + ex.Message);
                                }
                            };
                            break;
                        }

                    case "3": //Получить количество книг определенного жанра в библиотеке
                        {
                            GenreAddingData genreAddingData = new();

                            Console.Write("Введите название жанра: ");
                            genreAddingData.Name = Console.ReadLine();

                            try
                            {
                                SuccessMessage.Show("В библиотеке " + bookService.ShowCountBooksByGenre(genreAddingData) + " книг(и,а) этого жанра");
                            }
                            catch (NameEmptyException)
                            {
                                AlertMessage.Show("Название жанра должны быть задано.");
                            }

                            catch (Exception ex)
                            {
                                if (ex.Message == "Sequence contains no elements")
                                {
                                    AlertMessage.Show("Книги жанра " + genreAddingData.Name + " не найдены в библиотеке");
                                    break;
                                }

                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе поиска книг по жанру произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе поиска книг по жанру произошла ошибка:\n" + ex.Message);
                                }
                            };
                            break;
                        }

                    case "4": //Есть ли книга определенного автора и с определенным названием в библиотеке
                        {
                            AuthorAddingData authorAddingData = new();
                            BookAddingData bookAddingData = new();

                            Console.Write("Введите имя автора: ");
                            authorAddingData.Name = Console.ReadLine();

                            Console.Write("Введите отчество автора (если есть): ");
                            authorAddingData.MiddleName = Console.ReadLine();

                            Console.Write("Введите фамилию автора: ");
                            authorAddingData.Surname = Console.ReadLine();

                            Console.Write("Введите название книги: ");
                            bookAddingData.Title = Console.ReadLine();

                            try
                            {
                                if (bookService.IsBookByTitleAuthor(bookAddingData, authorAddingData))
                                {
                                    SuccessMessage.Show("В библиотеке книга этого автора с указанным названием есть!");
                                }
                                else
                                {
                                    SuccessMessage.Show("В библиотеке книга этого автора с указанным названием отсутствует");
                                }
                            }
                            catch (NameEmptyException)
                            {
                                AlertMessage.Show("Имя и фамилия автора, а также название книги должны быть заданы.");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе поиска книг по автору и названию произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе поиска книг по автору и названию произошла ошибка:\n" + ex.Message);
                                }
                            };
                            break;
                        }
                }
            }
            while (answer != "0");
        }

        //public int InputID(string name)
        //{
        //    bool flag;
        //    int id;
                        
        //    do
        //    {
        //        Console.Write("Введите ID " + name + ": ");
        //        flag = int.TryParse(Console.ReadLine(), out id);
                
        //        if (id < 1) 
        //        { 
        //            flag = false; 
        //        }
                
        //        if (!flag) 
        //        {
        //            Console.WriteLine("ID должен быть целым положительным числом. Попробуем еще раз");
        //        }
        //    }
        //    while (flag == false);
        //    return id;
        //}
    }
}
