using Microsoft.IdentityModel.Tokens;
using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.DAL.Entities;
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
    public class BookService
    {
        BookRepository bookRepository;

        public BookService()
        {
            bookRepository = new BookRepository();
        }

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

        public Book ShowByID(int id)
        {
            BookEntity findBook = bookRepository.GetBookByID(id);
            if (findBook == null)
            { throw new ObjectNotFoundException(); }

            return ConstructBookModel(findBook);
        }

        public Book ShowByID_Extended(int id)
        {
            BookExtendedEntity findBook = bookRepository.GetBookByID_Extended(id);
            if (findBook == null)
            { throw new ObjectNotFoundException(); }

            return ConstructBookModel_Extended(findBook);
        }

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

        public Book ConstructBookModel(BookEntity bookEntity)
        {
            return new Book(
                bookEntity.Id,
                bookEntity.Title,
                bookEntity.Description,
                bookEntity.PublicationDate
                );
        }

        public Book ConstructBookModel_Extended(BookExtendedEntity bookExtendedEntity)
        {
            return new Book(
                bookExtendedEntity.Id,
                bookExtendedEntity.Title,
                bookExtendedEntity.Description,
                bookExtendedEntity.PublicationDate,
                bookExtendedEntity.Authors,
                bookExtendedEntity.Genres,
                bookExtendedEntity.Users
                );
        }

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
    }
}
