using MovieStore.Models.Data;
using MovieStore.Models.Domain;
using MovieStore.Repos.Abstract;
using MovieStore.Repos.Implemention;
using MovieStore.UnitOfWork.Abstract;

namespace MovieStore.UnitOfWork.Implementation;


public class MainUnit : IMainUnit
{
    private readonly AppDbContext _db;

    public MainUnit(AppDbContext db, IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        Genres = new GenricRepo<Genre>(_db);
        Movies = new MovieRepo(_db, this, webHostEnvironment);
        MoviesGenres = new GenricRepo<MovieGenre>(_db);
        FileService = new FileService(webHostEnvironment);
    }

    public IGenricRepo<Genre> Genres { get; private set; }
    public IMovieRepo Movies { get; private set; }
    public IGenricRepo<MovieGenre> MoviesGenres { get; private set; }
    public IFileService FileService { get; private set; }

    public void Dispose()
    {
        _db.Dispose();
    }
}