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

        public Genre ShowByID(int id)
        {
            GenreEntity findGenre = genreRepository.GetGenreByID(id);
            if (findGenre == null)
            {
                throw new ObjectNotFoundException();
            }

            return ConstructGenreModel(findGenre);
        }

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

        public Genre ConstructGenreModel(GenreEntity genreEntity)
        {
            return new Genre(
                genreEntity.Id,
                genreEntity.Name,
                genreEntity.Books
                );
        }

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
