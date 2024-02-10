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
    public class CRUDView_Book
    {
        BookService bookService = new();

        public void Show()
        {
            string answer;
            Book? selectedBook = null;

            do
            {
                Console.WriteLine("\nПолучить все книги (нажмите 1)");
                Console.WriteLine("Получить (выбрать) книгу по ID (нажмите 2)");
                Console.WriteLine("Добавить книгу (нажмите 3)");
                Console.WriteLine("Удалить книгу (нажмите 4)");
                Console.WriteLine("Удалить книгу по ID (нажмите 5)");
                Console.WriteLine("Изменить дату издания книги по ID (нажмите 6)");
                Console.WriteLine("Вернуться назад (нажмите 0)");

                answer = Console.ReadLine();
                switch (answer)
                {
                    case "1": //Вывод данных о всех книгах
                        {
                            try
                            {
                                List<Book> books = bookService.ShowAll();

                                Console.WriteLine("\n|ID\t|Title\t|Decription\t|Date\t|");
                                foreach (Book item in books)
                                {
                                    Console.WriteLine("|{0}\t|{1}\t|{2}\t|{3}\t|", item.Id, item.Title, item.Description, item.PublicationDate.Year);
                                }
                                Console.WriteLine();
                            }
                            catch (Exception ex)
                            {
                                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
                            }
                            break;
                        }

                    case "2": //Вывод данных о книге (выбор книги) по ID
                        {
                            bool flag;
                            int id;

                            do
                            {
                                Console.Write("Введите ID книги: ");
                                flag = int.TryParse(Console.ReadLine(), out id);
                            }
                            while (flag == false);

                            selectedBook = null; //Сброс ранее выбранной книги на случай ошибки при выборе
                            try
                            {
                                selectedBook = bookService.ShowByID(id);
                                Console.WriteLine("\n|ID\t|Title\t|Decription\t|Date\t|");
                                Console.WriteLine("|{0}\t|{1}\t|{2}\t|{3}\t|", selectedBook.Id, selectedBook.Title, selectedBook.Description, selectedBook.PublicationDate.Year );
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

                    case "3": //Добавление книги
                        {
                            BookAddingData bookAddingData = new();
                            Console.Write("Введите название книги: ");
                            bookAddingData.Title = Console.ReadLine();

                            Console.Write("Введите описание книги (необязательно): ");
                            bookAddingData.Description = Console.ReadLine();

                            bool flag;
                            DateOnly publicationDate;
                            do
                            {
                                Console.Write("Введите год издания книги: ");
                                flag = DateOnly.TryParse("01.01." + Console.ReadLine(), out publicationDate);
                            }
                            while (flag == false);

                            bookAddingData.PublicationDate = publicationDate;

                            try
                            {
                                bookService.AddBook(bookAddingData);
                                SuccessMessage.Show("Книга " + bookAddingData.Title + " успешно добавлена");
                            }
                            catch (NameEmptyException)
                            {
                                AlertMessage.Show("Название книги не указано.");
                            }
                            catch (DateOutOfRangeException)
                            {
                                AlertMessage.Show("Год издание книиги не может быть больше текущего года.");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null) 
                                {
                                    AlertMessage.Show("В процессе добавления новой книги произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе добавления новой книги произошла ошибка:\n" + ex.Message);
                                }
                            };
                            break;
                        }

                    case "4": //Удаление выбранной книги
                        {
                            if (selectedBook != null)
                            {
                                Console.WriteLine("Выбранная книга:");
                                Console.WriteLine("\n|ID\t|Title\t|Decription\t|Date\t|");
                                Console.WriteLine("|{0}\t|{1}\t|{2}\t|{3}\t|", selectedBook.Id, selectedBook.Title, selectedBook.Description, selectedBook.PublicationDate);
                                Console.WriteLine();

                                bool flag = false;
                                do
                                {
                                    Console.Write("Вы уверены, что хотите удалить эту книгу? (Y/N) ");
                                    switch (Console.ReadLine().ToUpper())
                                    {
                                        case "Y":
                                            {
                                                flag = true;
                                                try
                                                {
                                                    bookService.RemoveBook(selectedBook);
                                                    SuccessMessage.Show("Книга " + selectedBook.Title + " успешно удалена");
                                                    selectedBook = null;
                                                }
                                                catch (NoOneObjectException)
                                                {
                                                    //Передан Null
                                                    AlertMessage.Show("В процессе удаления книги произошла ошибка:\nКнига не задана");
                                                }
                                                catch (Exception ex)
                                                {
                                                    if (ex.InnerException != null)
                                                    {
                                                        AlertMessage.Show("В процессе удаления книги произошла ошибка:\n" + ex.InnerException.Message);
                                                    }
                                                    else
                                                    {
                                                        AlertMessage.Show("В процессе удаления книги произошла ошибка:\n" + ex.Message);
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
                                AlertMessage.Show("Сначала следует выбрать книгу по ID");
                            }
                            break;
                        }

                    case "5": //Удаление книги по ID
                        {
                            bool flag;
                            int id;

                            do
                            {
                                Console.Write("Введите ID книги для удаления: ");
                                flag = int.TryParse(Console.ReadLine(), out id);
                            }
                            while (flag == false);

                            try
                            {
                                bookService.RemoveBook(id);
                                SuccessMessage.Show("Книга с ID = " + id + " успешно удалена");
                            }
                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("В процессе удаления книги произошла ошибка:\nКнига не найдена");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе удаления книги произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе удаления книги произошла ошибка:\n" + ex.Message);
                                }
                            }
                            break;
                        }

                    case "6": //Изменение даты издания книги по ID
                        {
                            bool flag;
                            int id;
                            string title;
                            DateOnly publicationDate;

                            do
                            {
                                Console.Write("Введите ID книги для изменения даты издания: ");
                                flag = int.TryParse(Console.ReadLine(), out id);
                            }
                            while (flag == false);

                            do
                            {
                                Console.Write("Введите новый год издания книги: ");
                                flag = DateOnly.TryParse("01.01." + Console.ReadLine(), out publicationDate);
                            }
                            while (flag == false);

                            try
                            {
                                bookService.UpdateBookDateByID(id, publicationDate);
                                SuccessMessage.Show("Дата издания книги с ID = " + id + " успешно обновлена");
                            }
                            catch (ObjectNotFoundException)
                            {
                                AlertMessage.Show("В процессе изменения даты издания книги произошла ошибка:\nПользователь не найден");
                            }
                            catch (Exception ex)
                            {
                                if (ex.InnerException != null)
                                {
                                    AlertMessage.Show("В процессе изменения даты издания книги произошла ошибка:\n" + ex.InnerException.Message);
                                }
                                else
                                {
                                    AlertMessage.Show("В процессе изизменения даты издания книги произошла ошибка:\n" + ex.Message);
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
