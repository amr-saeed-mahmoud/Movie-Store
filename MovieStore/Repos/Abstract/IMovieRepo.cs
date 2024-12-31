using MovieStore.Models.Domain;
using MovieStore.Models.Dto;

namespace MovieStore.Repos.Abstract;

public interface IMovieRepo : IGenricRepo<Movie>
{
    Task<MoviePagesDto> FetchMoviesWithPagingAsync(string searchTerm = "", bool enablePaging = false, int currentPage = 0);
    List<int> GetGenreByMovieId(int movieId);
}