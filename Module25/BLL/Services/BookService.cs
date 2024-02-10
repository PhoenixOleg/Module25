using Microsoft.IdentityModel.Tokens;
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
    public class BookService
    {
        BookRepository BookRepository;

        public BookService()
        {
            BookRepository = new BookRepository();
        }

        public List<Book> ShowAll()
        {
            List<Book> BooksList = new();

            List<BookEntity> findBooks = BookRepository.GetAllBook();
            if (findBooks.Count == 0)
            {
                throw new NoOneObjectException();
            }
            else
            {
                foreach (BookEntity BookEntity in findBooks)
                {
                    BooksList.Add(ConstructBookModel(BookEntity));
                }
            }

            return BooksList;
        }

        public Book ShowByID(int id)
        {
            BookEntity findBook = BookRepository.GetBookByID(id);
            if (findBook == null)
            { throw new ObjectNotFoundException(); }

            return ConstructBookModel(findBook);
        }

        public void AddBook(BookAddingData BookAddingData)
        {
            if (string.IsNullOrEmpty(BookAddingData.Title) || string.IsNullOrWhiteSpace(BookAddingData.Title))
            {
                throw new NameEmptyException();
            }

            if (BookAddingData.PublicationDate.CompareTo(DateOnly.FromDateTime(DateTime.Now)) > 0) //Только на превышение текушего года. Допустим в библиотеке есть оцифрованные старые книги)
            {
                throw new DateOutOfRangeException();
            }

            BookEntity BookEntity = new()
            {
                Title = BookAddingData.Title
                , Description = BookAddingData.Description
                , PublicationDate = BookAddingData.PublicationDate
            };

            if (BookRepository.AddBook(BookEntity) == 0)
            {
                throw new Exception();
            }
        }

        public void RemoveBook(Book? Book)
        {
            if (Book == null)
            {
                throw new NoOneObjectException();
            }

            BookEntity BookEntity = ConvertToBookEntity(Book);

            if (BookRepository.DeleteBook(BookEntity) == 0)
            {
                throw new Exception();
            }
        }

        public void RemoveBook(int id)
        {
            switch (BookRepository.DeleteBook(id))
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

            switch (BookRepository.UpdateBookNameByID(id, publicationDate))
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

        public Book ConstructBookModel(BookEntity BookEntity)
        {
            return new Book(
                BookEntity.Id,
                BookEntity.Title,
                BookEntity.Description,
                BookEntity.PublicationDate
                );
        }

        public BookEntity ConvertToBookEntity(Book Book)
        {
            BookEntity BookEntity = new()
            {
                Id = Book.Id,
                Title = Book.Title,
                Description = Book.Description,
                PublicationDate = Book.PublicationDate
            };

            return BookEntity;
        }
    }

}
