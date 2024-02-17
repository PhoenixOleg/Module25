using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.DAL.Entities;
using Module25.DAL.Repositories;
using Module25.Task_25_2_4.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module25.BLL.Services
{
    public class AuthorService
    {
        AuthorRepository authorRepository;

        public AuthorService()
        { 
            authorRepository = new AuthorRepository();
        }

        public List<Author> ShowAll()
        {
            List<Author> authorsList = new();

            List<AuthorEntity> findAuthors = authorRepository.GetAllAuthors();
            if (findAuthors.Count == 0)
            {
                throw new NoOneObjectException();
            }
            else
            {
                foreach (AuthorEntity authorEntity in findAuthors)
                {
                    authorsList.Add(ConstructAuthorModel(authorEntity));
                }
            }

            return authorsList;
        }

        public Author ShowByID(int id)
        {
            AuthorEntity findAuthor = authorRepository.GetAuthorByID(id);
            if (findAuthor == null)
            { 
                throw new ObjectNotFoundException(); 
            }

            return ConstructAuthorModel(findAuthor);
        }

        public void AddAuthor(AuthorAddingData authorAddingData)
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

            var countAuthors = (from AuthorEntity item  in authorRepository.GetAllAuthors()
                     where item.Name == authorAddingData.Name 
                     & item.MiddleName == authorAddingData.MiddleName 
                     & item.Surname == authorAddingData.Surname
                     select item.Id 
                     ).ToList().Count;
            
            if (countAuthors > 0)
            {
                throw new AlreadyExistsException();
            }

            if (authorRepository.AddAuthor(authorEntity) == 0)
            {
                throw new Exception();
            }
        }

        public void RemoveAuthor(Author? author)
        {
            if (author == null)
            {
                throw new NoOneObjectException();
            }

            AuthorEntity authorEntity = ConvertToAuthorEntity(author);

            if (authorRepository.DeleteAuthor(authorEntity) == 0)
            {
                throw new Exception();
            }
        }

        public void AddAuthorToBook(Author author, Book book)
        {
            if (author == null)
            {
                throw new NullReferenceException("Автор не выбран");
            }

            if (book == null)
            {
                throw new NullReferenceException("Книга не выбрана");
            }

            if (book.Authors.Where(a => a.Id == author.Id).Any())
            {
                throw new AlreadyExistsException();
            }

            AuthorEntity authorEntity = new()
            {   
                Id = author.Id,
                Name = author.Name,
                MiddleName = author.MiddleName,
                Surname = author.SurName,
                Books = new()
            };

            BookExtendedEntity bookExtendedEntity = new()
            {
                Id = book.Id,
                Description = book.Description,
                PublicationDate = book.PublicationDate,
                Title = book.Title,
                Authors = new() // Иначе ошибка, если какой-то автор уже присутствует
                                // The instance of entity type 'BookExtendedEntity' cannot be tracked because
                                // another instance with the same key
                                // value for {'Id'} is already being tracked.
            };


            if (authorRepository.AddAuthorToBook(authorEntity, bookExtendedEntity) == 0)
            {
                throw new Exception();
            }
        }

        public void DelAuthorFromBook(Author author, Book book)
        {
            if (author == null)
            {
                throw new NullReferenceException("Автор не выбран");
            }

            if (book == null)
            {
                throw new NullReferenceException("Книга не выбрана");
            }

            if (!book.Authors.Where(a => a.Id == author.Id).Any()) //Автор не существует в книге
            {
                throw new ObjectNotFoundException();
            }

            AuthorEntity authorEntity = new()
            {
                Id = author.Id,
                Name = author.Name,
                MiddleName = author.MiddleName,
                Surname = author.SurName,
            };

            BookExtendedEntity bookExtendedEntity = new()
            {
                Id = book.Id,
                Description = book.Description,
                PublicationDate = book.PublicationDate,
                Title = book.Title,
            };

            if (authorRepository.RemoveAuthorFromBook(authorEntity, bookExtendedEntity) == 0)
            {
                throw new Exception();
            }
        }

        public Author ConstructAuthorModel(AuthorEntity authorEntity)
        {
            return new Author(
                authorEntity.Id,
                authorEntity.Name,
                authorEntity.MiddleName,
                authorEntity.Surname,
                authorEntity.Books
                );
        }

        public AuthorEntity ConvertToAuthorEntity(Author author)
        {
            AuthorEntity authorEntity = new()
            {
                Id = author.Id,
                Name = author.Name,
                MiddleName = author.MiddleName,
                Surname = author.SurName
            };

            return authorEntity;
        }
    }
}
