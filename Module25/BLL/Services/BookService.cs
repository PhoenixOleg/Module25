using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
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
    public class BookService
    {
        BookRepository bookRepository;

        public BookService()
        {
            bookRepository = new BookRepository();
        }

        /// <summary>
        /// Метод возвращения списка всех книг слоя BLL без данных об авторах и жанрах
        /// </summary>
        /// <returns>Список экземпляров Book</returns>
        /// <exception cref="NoOneObjectException">Список пуст</exception>
        public List<Book> ShowAll()
        {
            List<Book> booksList = new();

            List<BookEntity> findBooks = bookRepository.GetAllBooks();
            if (findBooks.Count == 0)
            {
                throw new NoOneObjectException();
            }
            else
            {
                foreach (BookEntity bookEntity in findBooks)
                {
                    booksList.Add(ConstructBookModel(bookEntity));
                }
            }

            return booksList;
        }

        /// <summary>
        /// Метод возвращения списка всех книг слоя BLL c данными об авторах и жанрах
        /// </summary>
        /// <returns>Список экземпляров Book</returns>
        /// <exception cref="NoOneObjectException">Список пуст</exception>
        public List<Book> ShowAll_Extended()
        {
            List<Book> booksList = new();

            List<BookExtendedEntity> findBooks = bookRepository.GetAllBooks_Extended();
            if (findBooks.Count == 0)
            {
                throw new NoOneObjectException();
            }
            else
            {
                foreach (BookExtendedEntity bookExtendedEntity in findBooks)
                {
                    booksList.Add(ConstructBookModel_Extended(bookExtendedEntity));
                }
            }

            return booksList;
        }

        /// <summary>
        /// Метод получения книги по ID слоя BLL без данных об авторах и жанрах
        /// </summary>
        /// <param name="id">Id книги</param>
        /// <returns>Экземпляр Book</returns>
        /// <exception cref="ObjectNotFoundException">Книга не найдена</exception>
        public Book ShowByID(int id)
        {
            BookEntity findBook = bookRepository.GetBookByID(id);
            if (findBook == null)
            { throw new ObjectNotFoundException(); }

            return ConstructBookModel(findBook);
        }

        /// <summary>
        /// Метод получения книги по ID слоя BLL c данными об авторах и жанрах
        /// </summary>
        /// <param name="id">Id книги</param>
        /// <returns>Книга не найдена</returns>
        /// <exception cref="ObjectNotFoundException"></exception>
        public Book ShowByID_Extended(int id)
        {
            BookExtendedEntity findBook = bookRepository.GetBookByID_Extended(id);
            if (findBook == null)
            { throw new ObjectNotFoundException(); }

            return ConstructBookModel_Extended(findBook);
        }

        /// <summary>
        /// Метод получения количества книг автора по ФИО уровня BLL
        /// </summary>
        /// <param name="authorAddingData">Экземпляр AuthorAddingData (ФИО автора)</param>
        /// <returns>Количество найденных книг</returns>
        /// <exception cref="NameEmptyException">Имя или фамилия автора не указаны</exception>
        public int ShowCountBooksByAuthor(AuthorAddingData authorAddingData)
        {
            if (string.IsNullOrEmpty(authorAddingData.Name) || string.IsNullOrWhiteSpace(authorAddingData.Name))
            {
                throw new NameEmptyException();
            }

            if (string.IsNullOrEmpty(authorAddingData.Surname) || string.IsNullOrWhiteSpace(authorAddingData.Surname))
            {
                throw new NameEmptyException();
            }

            AuthorEntity authorEntity = new()
            {
                Name = authorAddingData.Name,
                MiddleName = authorAddingData.MiddleName,
                Surname = authorAddingData.Surname
            };

            return bookRepository.GetCountBooksByAuthor(authorEntity);
        }

        /// <summary>
        /// Метод получения количества книг по названию жанра уровня BLL
        /// </summary>
        /// <param name="genreAddingData">Экземпляр GenreAddingData (название жанра)</param>
        /// <returns>Количество найденных книг</returns>
        /// <exception cref="NameEmptyException">Название жанра не задано</exception>
        public int ShowCountBooksByGenre(GenreAddingData genreAddingData)
        {
            if (string.IsNullOrEmpty(genreAddingData.Name) || string.IsNullOrWhiteSpace(genreAddingData.Name))
            {
                throw new NameEmptyException();
            }

            GenreEntity genreEntity = new()
            {
                Name = genreAddingData.Name,
            };

            return bookRepository.GetCountBooksByGenre(genreEntity);
        }

        /// <summary>
        /// Метод проверки наличия книги в библиотеке по названию и ФИО автора уровня BLL
        /// </summary>
        /// <param name="bookAddingData">Экземпляр BookAddingData (название книги)</param>
        /// <param name="authorAddingData">Экземпляр AuthorAddingData (ФИО автора (соавтора)</param>
        /// <returns>true - книга найдена,
        /// false - книга не найдена</returns>
        /// <exception cref="NameEmptyException"></exception>
        public bool IsBookByTitleAuthor(BookAddingData bookAddingData, AuthorAddingData authorAddingData)
        {
            if (string.IsNullOrEmpty(authorAddingData.Name) || string.IsNullOrWhiteSpace(authorAddingData.Name))
            {
                throw new NameEmptyException();
            }

            if (string.IsNullOrEmpty(authorAddingData.Surname) || string.IsNullOrWhiteSpace(authorAddingData.Surname))
            {
                throw new NameEmptyException();
            }

            if (string.IsNullOrEmpty(bookAddingData.Title) || string.IsNullOrWhiteSpace(bookAddingData.Title))
            {
                throw new NameEmptyException();
            }

            AuthorEntity authorEntity = new()
            {
                Name = authorAddingData.Name,
                MiddleName = authorAddingData.MiddleName,
                Surname = authorAddingData.Surname
            };

            BookEntity bookEntity = new()
            {
                Title = bookAddingData.Title
            };

            return bookRepository.IsBookByTitleAuthor(authorEntity, bookEntity);
        }

        /// <summary>
        /// Метод добавления книги в библиотеку уровня BLL
        /// </summary>
        /// <param name="bookAddingData">Экземпляр BookAddingData</param>
        /// <exception cref="NameEmptyException">Название книги не указано</exception>
        /// <exception cref="DateOutOfRangeException">Дата издания книги вне диапазона (больше текущей)</exception>
        /// <exception cref="Exception">Иные ошибки добавления книги</exception>
        public void AddBook(BookAddingData bookAddingData)
        {
            if (string.IsNullOrEmpty(bookAddingData.Title) || string.IsNullOrWhiteSpace(bookAddingData.Title))
            {
                throw new NameEmptyException();
            }

            if (bookAddingData.PublicationDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0) //Только на превышение текушего года. Допустим в библиотеке есть оцифрованные старые книги)
            {
                throw new DateOutOfRangeException();
            }

            BookEntity bookEntity = new()
            {
                Title = bookAddingData.Title
                ,
                Description = bookAddingData.Description
                ,
                PublicationDate = bookAddingData.PublicationDate
            };

            if (bookRepository.AddBook(bookEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Метод удаления книги из библиотеки уровня BLL
        /// </summary>
        /// <param name="book">Экземпляр Book</param>
        /// <exception cref="NoOneObjectException">Книга не найдена</exception>
        /// <exception cref="Exception">Иная ошибка удаления книги</exception>
        public void RemoveBook(Book? book)
        {
            if (book == null)
            {
                throw new NoOneObjectException();
            }

            BookEntity bookEntity = ConvertToBookEntity(book);

            if (bookRepository.DeleteBook(bookEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Метод удаления книги из библиотеки по ее ID уровня BLL
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <exception cref="ObjectNotFoundException">книга не найдена</exception>
        /// <exception cref="Exception">Иная ошибка удаления книги</exception>
        public void RemoveBook(int id)
        {
            switch (bookRepository.DeleteBook(id))
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
        /// Метод обновления даты издания книги по ее ID уровня BLL
        /// </summary>
        /// <param name="id">ID книги</param>
        /// <param name="publicationDate">Новая дата издания</param>
        /// <exception cref="DateOutOfRangeException">Дата вне диапазона (больше текущей)</exception>
        /// <exception cref="ObjectNotFoundException">Книга не найдена</exception>
        /// <exception cref="Exception">Иная ошибка обновления даты издания книги</exception>
        public void UpdateBookDateByID(int id, DateOnly publicationDate)
        {
            if (publicationDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0) //Только на превышение текушего года. Допустим в библиотеке есть оцифрованные старые книги)
            {
                throw new DateOutOfRangeException();
            }

            switch (bookRepository.UpdateBookNameByID(id, publicationDate))
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
        /// Создание упрощенного Book (без авторов, жанров) из BookEntity при возврате с уровня DAL
        /// </summary>
        /// <param name="bookEntity">Полученный экземпляр BookEntity</param>
        /// <returns>Экземпляр Book</returns>
        public Book ConstructBookModel(BookEntity bookEntity)
        {
            return new Book(
                bookEntity.Id,
                bookEntity.Title,
                bookEntity.Description,
                bookEntity.PublicationDate
                );
        }

        /// <summary>
        /// Создание Book (c авторами и жанрами) из BookExtendedEntity при возврате с уровня DAL
        /// </summary>
        /// <param name="bookExtendedEntity">Полученный экземпляр BookExtendedEntity</param>
        /// <returns>Экземпляр Book</returns>
        public Book ConstructBookModel_Extended(BookExtendedEntity bookExtendedEntity)
        {
            return new Book(
                bookExtendedEntity.Id,
                bookExtendedEntity.Title,
                bookExtendedEntity.Description,
                bookExtendedEntity.PublicationDate,
                bookExtendedEntity.Authors,
                bookExtendedEntity.Genres,
                new List<UserExtendedEntity>() //Список пользователей не используется на уровне презентации и не запрашиваетяся на уровне DAL,
                                               //поэтому делаю пустой список, чтобы не загружатьпамять - пользователей может быть тысячи
                                               //Наверное тут лучше вести новую модель без пользователей
                );
        }

        /// <summary>
        /// Создание BookEntity из Book для передачи на уровень DAL
        /// </summary>
        /// <param name="book">Передаваемый экземпляр Book</param>
        /// <returns>Экземпляр BookEntity</returns>
        public BookEntity ConvertToBookEntity(Book book)
        {
            BookEntity bookEntity = new()
            {
                Id = book.Id,
                Title = book.Title,
                Description = book.Description,
                PublicationDate = book.PublicationDate
            };

            return bookEntity;
        }

        /// <summary>
        /// Получение списка по названию жанра и дате издания на уровне BLL
        /// </summary>
        /// <param name="genreAddingData">"Экземпляр GenreAddingData (название жанра)</param>
        /// <param name="dateInterval">Кортеж с интервалом дат издания</param>
        /// <returns>Список экземпляров Book</returns>
        /// <exception cref="NameEmptyException">Название жанра не задано</exception>
        /// <exception cref="DateOutOfRangeException">Начальная дата диапазона больше текущей даты</exception>
        /// <exception cref="InvalidDateIntervalException">Дата начала диапазона больше даты его окончания</exception>
        /// <exception cref="NoOneObjectException">Список пуст</exception>
        public List<Book> ShowBooksByGenrePubYear(GenreAddingData genreAddingData, (DateOnly beginDate, DateOnly endDate) dateInterval)
        {
            List<Book> booksList = new();

            if (string.IsNullOrEmpty(genreAddingData.Name) || string.IsNullOrWhiteSpace(genreAddingData.Name))
            {
                throw new NameEmptyException();
            }

            if (dateInterval.beginDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0 || dateInterval.endDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0)
            {
                throw new DateOutOfRangeException();
            }

            if (dateInterval.beginDate.CompareTo(dateInterval.endDate) >= 0)
            {
                throw new InvalidDateIntervalException();
            }

            GenreEntity genreEntity = new()
            {
                Name = genreAddingData.Name,
            };

            List<BookExtendedEntity> findBooks = bookRepository.GetBooksByGenrePubYear(genreEntity, dateInterval);

            if (findBooks.Count == 0)
            {
                throw new NoOneObjectException();
            }
            else
            {
                foreach (BookExtendedEntity bookExtendedEntity in findBooks)
                {
                    booksList.Add(ConstructBookModel_Extended(bookExtendedEntity));
                }
            }

            return booksList;
        }

        /// <summary>
        /// Получение последней изданной книги (списка книг) на уровне BLL
        /// </summary>
        /// <returns>Список экземпляров Book</returns>
        /// <exception cref="NoOneObjectException">Список пуст</exception>
        public List<Book> ShowLastPublishedBook()
        {
            List<Book> booksList = new();

            List<BookExtendedEntity> findBooks = bookRepository.GetLastPublishedBook();

            if (findBooks.Count == 0)
            {
                throw new NoOneObjectException();
            }
            else
            {
                foreach (BookExtendedEntity bookExtendedEntity in findBooks)
                {
                    booksList.Add(ConstructBookModel_Extended(bookExtendedEntity));
                }
            }

            return booksList;
        }

        /// <summary>
        /// Получение спика всех книг, отсортированного в алфавитном порядке по названию
        /// </summary>
        /// <returns>Список экземпляров Book</returns>
        /// <exception cref="NoOneObjectException">Список пуст</exception>
        public List<Book> ShowAllBooksNameAsc()
        {
            List<Book> booksList = new();

            List<BookExtendedEntity> findBooks = bookRepository.GetAllBooksNameAsc();

            if (findBooks.Count == 0)
            {
                throw new NoOneObjectException();
            }
            else
            {
                foreach (BookExtendedEntity bookExtendedEntity in findBooks)
                {
                    booksList.Add(ConstructBookModel_Extended(bookExtendedEntity));
                }
            }

            return booksList;
        }

        /// <summary>
        /// Получение спика всех книг, отсортированного в порядке убывания даты их выхода
        /// </summary>
        /// <returns>Список экземпляров Book</returns>
        /// <exception cref="NoOneObjectException">Список пуст</exception>
        public List<Book> ShowAllBooksPubDesc()
        {
            List<Book> booksList = new();

            List<BookExtendedEntity> findBooks = bookRepository.GetAllBooksPubDesc();

            if (findBooks.Count == 0)
            {
                throw new NoOneObjectException();
            }
            else
            {
                foreach (BookExtendedEntity bookExtendedEntity in findBooks)
                {
                    booksList.Add(ConstructBookModel_Extended(bookExtendedEntity));
                }
            }

            return booksList;
        }
    }
}
