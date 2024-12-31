using System.Linq.Expressions;
using Azure;
using Microsoft.EntityFrameworkCore;
using MovieStore.Models.Data;
using MovieStore.Models.Domain;
using MovieStore.Models.Dto;
using MovieStore.Repos.Abstract;
using MovieStore.UnitOfWork.Abstract;

namespace MovieStore.Repos.Implemention;

public class MovieRepo : IMovieRepo
{

    private readonly IWebHostEnvironment _Environment;
    private readonly IMainUnit _MainUnit;
    private readonly AppDbContext _db;

    public MovieRepo(AppDbContext db, IMainUnit mainUnit, IWebHostEnvironment webHostEnvironment)
    {
        _db = db;
        _MainUnit = mainUnit;
        _Environment = webHostEnvironment;
    }

    public async Task<MoviePagesDto> FetchMoviesWithPagingAsync(string searchTerm, bool enablePaging, int currentPage)
    {
        var Movies = await _db.Movies!.ToListAsync();
        var MovesPages = new MoviePagesDto();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            searchTerm = searchTerm.ToLower();
            Movies = Movies.Where(a => a.Title!.ToLower().StartsWith(searchTerm)).ToList();
        }
        if (enablePaging)
        {
            // here we will apply paging
            int pageSize = 5;
            int count = Movies.Count;
            int TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Movies = Movies.Skip((currentPage - 1)*pageSize).Take(pageSize).ToList();
            MovesPages.PageSize = pageSize;
            MovesPages.CurrentPage = currentPage;
            MovesPages.TotalPages = TotalPages;
        }
        foreach (var movie in Movies)
        {
            var genres = (from genre in _db.Genres
                          join mg in _db.MoviesGenres!
                          on genre.Id equals mg.GenreId
                          where mg.MovieId == movie.Id
                          select genre.GenreName
                          ).ToList();
            var genreNames = string.Join(',', genres);
            movie.GenreNames = genreNames;
        }
        MovesPages.MovieList = Movies;
        return MovesPages;
    }


    public async Task<bool> AddAsync(Movie entity)
    {
        try
        {
            if(entity.Genres == null)
            {
                return false;
            }

            await _db.Movies!.AddAsync(entity);
            _db.SaveChanges();
            
            foreach(int GenreID in entity.Genres)
            {
                var movieGenre = new MovieGenre
                {
                    MovieId = entity.Id,
                    GenreId = GenreID
                };
                _db.MoviesGenres!.Add(movieGenre);
            }
            _db.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task DeleteAsync(int id)
    {
       var data = await GetByIdAsync(id);
       if(data != null)
       {
            var movieGenres = _db.MoviesGenres!.Where(a => a.MovieId == data.Id);
            foreach(var movieGenre in movieGenres)
            {
                _db.MoviesGenres!.Remove(movieGenre);
            }
            if(!string.IsNullOrEmpty(data.MovieImage))
            {
                string UplaodPath = Path.Combine(_Environment.WebRootPath , "Upload"); 
                string ImagePath = Path.Combine(UplaodPath, data.MovieImage ?? "");
                bool result = _MainUnit.FileService.DeleteImage(ImagePath);
            }
            
            _db.Movies!.Remove(data);
            _db.SaveChanges();
       }
    }

    public async Task<IEnumerable<Movie>> FindAsync(Expression<Func<Movie, bool>> predicate)
    {
        return await _db.Movies!.Where(predicate).ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllAsync()
    {
        var Movies = await _db.Movies!.ToListAsync();
        return Movies;
    }

    public async Task<Movie?> GetByIdAsync(int id)
    {
        var Movie = await _db.Movies!.FindAsync(id);
            if(Movie != null)
            {
                return Movie;
            }
            return new Movie();
    }

    public async Task<bool> UpdateAsync(Movie entity)
    {
        try
        {
           
            var genresToDeleted = _db.MoviesGenres!.Where(a => a.MovieId == entity.Id && !entity.Genres!.Contains(a.GenreId)).ToList();
            foreach(var mGenre in genresToDeleted)
            {
                _db.MoviesGenres!.Remove(mGenre);
            }
            foreach (int genId in entity.Genres!)
            {
                var movieGenre = await _db.MoviesGenres!.FirstOrDefaultAsync(a => a.MovieId == entity.Id && a.GenreId == genId);
                if (movieGenre == null)
                {
                    movieGenre = new MovieGenre { GenreId = genId, MovieId = entity.Id };
                    _db.MoviesGenres!.Add(movieGenre);
                }
            }
            _db.Movies!.Update(entity);
            _db.SaveChanges();
            return true;
        }
        catch
        {
            return false;
        }
    }

    public List<int> GetGenreByMovieId(int movieId)
    {
        var genreIds = _db.MoviesGenres!.Where(a => a.MovieId == movieId).Select(a => a.GenreId).ToList();
        return genreIds;
    }
       

}