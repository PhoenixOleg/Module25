using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.BLL.Services;
using Module25.PLL.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.PLL.Views
{
    public class CommonView
    {
        AuthorService authorService = new();
        BookService bookService = new();
        GenreService genreService = new();
        UserService userService = new();

        /// <summary>
        /// Вывод списка всех книг с жанрами и авторами*
        /// </summary>
        /// <param name="isExtended">true - Используется расширенная схема с жанрами и авторами (по-умолчанию). false - только список книг без доп атрибутов</param>
        public void Show_AllBooks(bool isExtended = true)
        {
            if (isExtended)
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
            }
            else
            {
                try
                {
                    List<Book> books = bookService.ShowAll();

                    Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} |", "ID", "Название", "Описание", "Год");
                    foreach (Book item in books)
                    {
                        Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} |", item.Id, item.Title, item.Description, item.PublicationDate.Year);
                    }
                    Console.WriteLine();
                }
                catch (Exception ex)
                {
                    AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// Вывод списка всех авторов
        /// </summary>
        public void Show_AllAuthors()
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
        }

        /// <summary>
        /// Вывод списка всех жанров
        /// </summary>
        public void Show_AllGenres()
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
        }


        /// <summary>
        /// Вывод списка всех жанров
        /// </summary>
        public void Show_AllUsers()
        {
            try
            {
                List<User> users = userService.ShowAll();

                Console.WriteLine("| {0, 4} | {1, 20} | {2, 20} |", "ID", "Имя пользователя", "e-mail");
                foreach (User item in users)
                {
                    Console.WriteLine("| {0, 4} | {1, 20} | {2, 20} |", item.Id, item.Name, item.Email);
                }
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                AlertMessage.Show("Возникла ошибка:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Вывод данных о выбранной книге
        /// </summary>
        /// <param name="selectedBook">Экземпляр класса Book - выбранная книга</param>
        public void Show_SelectedBook(Book selectedBook)
        {
            Console.SetWindowSize(180, Console.WindowHeight);

            Console.WriteLine("Выбранная книга:");
            Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", "ID", "Название", "Описание", "Год", "Автор(ы)", "Жанр");
            if (selectedBook.Authors != null && selectedBook.Genres != null)  
            {
                Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", selectedBook.Id, selectedBook.Title, selectedBook.Description, selectedBook.PublicationDate.Year, string.Join(" ", selectedBook.Authors.Select(a => a.Surname + " " + a.Name + " " + a.MiddleName).ToArray()), string.Join(" ", selectedBook.Genres.Select(g => g.Name).ToArray()));
            }
            else
            {
                Console.WriteLine("| {0, 4} | {1, 30} | {2, 40} | {3, 4} | {4, 50} | {5, 30} |", selectedBook.Id, selectedBook.Title, selectedBook.Description, selectedBook.PublicationDate.Year, "", "");
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Вывод данных о выбранном авторе
        /// </summary>
        /// <param name="selectedAuthor">Экземпляр класса Author - выбранный автор</param>
        public void Show_selectedAuthor(Author selectedAuthor)
        {
            Console.WriteLine("Выбранный автор:");
            Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", "ID", "Имя автора", "Отчество автора", "Фамилия автора");
            Console.WriteLine("| {0, 4} | {1, 15} | {2, 15} | {3, 15} |", selectedAuthor.Id, selectedAuthor.Name, selectedAuthor.MiddleName, selectedAuthor.SurName);
            Console.WriteLine();
        }

        /// <summary>
        /// Вывод данных о выбранном жанре
        /// </summary>
        /// <param name="selectedGenre">Экземпляр класса Genre - выбранный жанр</param>
        public void Show_SelectedGenre(Genre selectedGenre)
        {
            Console.WriteLine("Выбранный жанр:");
            Console.WriteLine("| {0, 4} | {1, 15} |", "ID", "Жанр");
            Console.WriteLine("| {0, 4} | {1, 15} |", selectedGenre.Id, selectedGenre.Name);
            Console.WriteLine();
        }

        /// <summary>
        /// Вывод данных о выбранном Пользователе
        /// </summary>
        /// <param name="selectedUser">Экземпляр класса User - выбранный пользователе</param>
        public void Show_SelectedUser(User selectedUser)
        {
            Console.WriteLine("Выбранный пользователь:");
            Console.WriteLine("| {0, 4} | {1, 20} | {2, 20} |", "ID", "Имя пользователя", "e-mail");
            Console.WriteLine("| {0, 4} | {1, 20} | {2, 20} |", selectedUser.Id, selectedUser.Name, selectedUser.Email);
            Console.WriteLine();
        }

        /// <summary>
        /// Получение ID с консоли для поиска объекта 
        /// </summary>
        /// <param name="name">имя объекта (книга, автор и т. п.)</param>
        /// <returns>ID - целое положительное число</returns>
        public int InputID(string name)
        {
            bool flag;
            int id;

            do
            {
                Console.Write("Введите ID " + name + ": ");
                flag = int.TryParse(Console.ReadLine(), out id);

                if (id < 1)
                {
                    flag = false;
                }

                if (!flag)
                {
                    AlertMessage.Show("ID должен быть целым положительным числом. Попробуем еще раз");
                }
            }
            while (flag == false);
            return id;
        }
    }
}
