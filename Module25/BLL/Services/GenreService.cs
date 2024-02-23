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
    public class GenreService
    {
        GenreRepository genreRepository;

        public GenreService()
        {
            genreRepository = new GenreRepository();
        }

        /// <summary>
        /// Метод возвращения списка всех жанров слоя BLL
        /// </summary>
        /// <returns>Список экземпляров Genre</returns>
        /// <exception cref="NoOneObjectException">Список пуст</exception>
        public List<Genre> ShowAll()
        {
            List<Genre> genresList = new();

            List<GenreEntity> findGenres = genreRepository.GetAllGenres();
            if (findGenres.Count == 0)
            {
                throw new NoOneObjectException();
            }
            else
            {
                foreach (GenreEntity genreEntity in findGenres)
                {
                    genresList.Add(ConstructGenreModel(genreEntity));
                }
            }

            return genresList;
        }

        /// <summary>
        /// Метод получения жанра по ID слоя BLL
        /// </summary>
        /// <param name="id">ID автора</param>
        /// <returns>Экземпляр Genre</returns>
        /// <exception cref="ObjectNotFoundException">Жанр не найден</exception>
        public Genre ShowByID(int id)
        {
            GenreEntity findGenre = genreRepository.GetGenreByID(id);
            if (findGenre == null)
            {
                throw new ObjectNotFoundException();
            }

            return ConstructGenreModel(findGenre);
        }

        /// <summary>
        /// Метод добавления жанра в БД слоя BLL
        /// </summary>
        /// <param name="genreAddingData">Экземпляр GenreAddingData (название жанра)</param>
        /// <exception cref="NameEmptyException">Название жанра не задано</exception>
        /// <exception cref="AlreadyExistsException">Жанр уже существует в БД</exception>
        /// <exception cref="Exception">Жанр не добавлен</exception>
        public void AddGenre(GenreAddingData genreAddingData)
        {
            if (string.IsNullOrEmpty(genreAddingData.Name) || string.IsNullOrWhiteSpace(genreAddingData.Name))
            {
                throw new NameEmptyException();
            }

            GenreEntity genreEntity = new()
            {
                Name = genreAddingData.Name,
            };

            //Не самый эффективный способ.
            var countGenres = (from GenreEntity item in genreRepository.GetAllGenres()
                                where item.Name == genreAddingData.Name
                                select item.Id
                     ).ToList().Count;

            if (countGenres > 0)
            {
                throw new AlreadyExistsException();
            }

            if (genreRepository.AddGenre(genreEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Метод удаления жанра в БД слоя BLL
        /// </summary>
        /// <param name="genre">Экземпляр Genre</param>
        /// <exception cref="NoOneObjectException">Жанр не задан</exception>
        /// <exception cref="Exception">Жанр не удален</exception>
        public void RemoveGenre(Genre? genre)
        {
            if (genre == null)
            {
                throw new NoOneObjectException();
            }

            GenreEntity genreEntity = ConvertToGenreEntity(genre);

            if (genreRepository.DeleteGenre(genreEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Метод добавления жанра в книгу слоя BLL
        /// </summary>
        /// <param name="genre">Экземпляр Genre</param>
        /// <param name="book">Экземпляр Book</param>
        /// <exception cref="NullReferenceException">Жанр или книга не заданы</exception>
        /// <exception cref="AlreadyExistsException">Книга уже относится к этому жанру</exception>
        /// <exception cref="Exception">Книга не добавлена в жанр</exception>
        public void AddGenreToBook(Genre genre, Book book)
        {
            if (genre == null)
            {
                throw new NullReferenceException("Жанр не выбран");
            }

            if (book == null)
            {
                throw new NullReferenceException("Книга не выбрана");
            }

            if (book.Genres.Where(g => g.Id == genre.Id).Any())
            {
                throw new AlreadyExistsException();
            }

            GenreEntity genreEntity = new()
            {
                Id = genre.Id,
                Name = genre.Name,
                Books = new()
            };

            BookExtendedEntity bookExtendedEntity = new()
            {
                Id = book.Id,
                Description = book.Description,
                PublicationDate = book.PublicationDate,
                Title = book.Title,
                Genres = new()
            };


            if (genreRepository.AddGenreToBook(genreEntity, bookExtendedEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Метод удаления жанра из книги слоя BLL
        /// </summary>
        /// <param name="genre">Экземпляр Genre</param>
        /// <param name="book">Экземпляр Book</param>
        /// <exception cref="NullReferenceException">Жанр или книга не заданы</exception>
        /// <exception cref="ObjectNotFoundException">Книга не относится к этому жанру</exception>
        /// <exception cref="Exception">Книга не удалена из жанра</exception>
        public void DelGenreFromBook(Genre genre, Book book)
        {
            if (genre == null)
            {
                throw new NullReferenceException("Жанр не выбран");
            }

            if (book == null)
            {
                throw new NullReferenceException("Книга не выбрана");
            }

            if (!book.Genres.Where(a => a.Id == genre.Id).Any()) //Жанр не существует в книге
            {
                throw new ObjectNotFoundException();
            }

            GenreEntity genreEntity = new()
            {
                Id = genre.Id,
                Name = genre.Name,
            };

            BookExtendedEntity bookExtendedEntity = new()
            {
                Id = book.Id,
                Description = book.Description,
                PublicationDate = book.PublicationDate,
                Title = book.Title,
            };

            if (genreRepository.RemoveGenreFromBook(genreEntity, bookExtendedEntity) == 0)
            {
                throw new Exception();
            }
        }

        /// <summary>
        /// Создание Genre из GenreEntity при возврате с уровня DAL
        /// </summary>
        /// <param name="genreEntity">Полученный экземпляр GenreEntity</param>
        /// <returns>Экземпляр Genre</returns>
        public Genre ConstructGenreModel(GenreEntity genreEntity)
        {
            return new Genre(
                genreEntity.Id,
                genreEntity.Name,
                genreEntity.Books
                );
        }

        /// <summary>
        /// Создание GenreEntity из Genre для передачи на уровень DAL
        /// </summary>
        /// <param name="genre">Передаваемый экземпляр Genre</param>
        /// <returns>Экземпляр GenreEntity</returns>
        public GenreEntity ConvertToGenreEntity(Genre genre)
        {
            GenreEntity genreEntity = new()
            {
                Id = genre.Id,
                Name = genre.Name
            };

            return genreEntity;
        }
    }
}
