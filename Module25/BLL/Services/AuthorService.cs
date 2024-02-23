using Module25.BLL.Exceptions;
using Module25.BLL.Models;
using Module25.DAL.Entities;
using Module25.DAL.Repositories;
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

        /// <summary>
        /// Метод возвращения списка всех авторов слоя BLL
        /// </summary>
        /// <returns>Список экземпляров Author</returns>
        /// <exception cref="NoOneObjectException">Список пуст</exception>
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

        /// <summary>
        /// Метод получения автора по ID слоя BLL
        /// </summary>
        /// <param name="id">ID автора</param>
        /// <returns>Экземпляр Author со сведениями об авторе</returns>
        /// <exception cref="ObjectNotFoundException">Автор не найден</exception>
        public Author ShowByID(int id)
        {
            AuthorEntity findAuthor = authorRepository.GetAuthorByID(id);
            if (findAuthor == null)
            { 
                throw new ObjectNotFoundException(); 
            }

            return ConstructAuthorModel(findAuthor);
        }

        /// <summary>
        /// Метод добавления автора в БД слоя BLL
        /// </summary>
        /// <param name="authorAddingData">Экзепляр модели AuthorAddingData (ФИО автора)</param>
        /// <exception cref="NameEmptyException">Имя или фамилия пусты</exception>
        /// <exception cref="AlreadyExistsException">Автор с такими ФИО уже присутствуетв БД (на практике надо добавлять еще страну автора, дату рождения для полной идентификации и т. п.)</exception>
        /// <exception cref="Exception">Автор не был добавлен по иным причинам</exception>
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

        /// <summary>
        /// Метод удаления автора из БД слоя BL
        /// </summary>
        /// <param name="author">Экземпляр Author</param>
        /// <exception cref="NoOneObjectException">Автор не задан</exception>
        /// <exception cref="Exception">Автор не удален</exception>
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

        /// <summary>
        /// Метод добавления автора в книгу слоя BLL
        /// </summary>
        /// <param name="author">Экземпляр Author</param>
        /// <param name="book">Экземпляр Book</param>
        /// <exception cref="NullReferenceException">Автор или книга не заданы</exception>
        /// <exception cref="AlreadyExistsException">Автор уже находится в списке писателей этой книги</exception>
        /// <exception cref="Exception">Автор не добавлен</exception>
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

            if (book.Authors.Any(a => a.Id == author.Id))
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
                Authors = new() // Иначе ошибка, если какой-то автор уже присутствует млм вообще этот атрибут здесь не трогать
                                // The instance of entity type 'BookExtendedEntity' cannot be tracked because
                                // another instance with the same key
                                // value for {'Id'} is already being tracked.
            };


            if (authorRepository.AddAuthorToBook(authorEntity, bookExtendedEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Метод удаления автора из книги слоя BLL
        /// </summary>
        /// <param name="author">Автор или книга не заданы</param>
        /// <param name="book">Экземпляр Book</param>
        /// <exception cref="NullReferenceException">Автор или книга не заданы</exception>
        /// <exception cref="ObjectNotFoundException">Автор не находится в списке писателей этой книги</exception>
        /// <exception cref="Exception">Автор не удален</exception>
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

        /// <summary>
        /// Создание Author из AuthorEntity при возврате с уровня DAL
        /// </summary>
        /// <param name="authorEntity">Полученный экземпляр AuthorEntity</param>
        /// <returns>Экземпляр Author</returns>
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

        /// <summary>
        /// Создание AuthorEntity из Author для передачи на уровень DAL
        /// </summary>
        /// <param name="author">Передаваемый экземпляр Author</param>
        /// <returns>Экземпляр AuthorEntity</returns>
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
