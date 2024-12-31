using MovieStore.Models.Domain;
using MovieStore.Repos.Abstract;

namespace MovieStore.UnitOfWork.Abstract;

public interface IMainUnit : IDisposable
{
    public IGenricRepo<Genre> Genres { get; }
    public IMovieRepo Movies { get; }
    public IGenricRepo<MovieGenre> MoviesGenres { get; }
    public IFileService FileService { get; }
}