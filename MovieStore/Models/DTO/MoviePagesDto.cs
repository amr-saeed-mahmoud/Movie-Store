using MovieStore.Models.Domain;

namespace MovieStore.Models.Dto;


public class MoviePagesDto
{
    public IEnumerable<Movie>? MovieList { get; set; }
    public int PageSize { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string? Term { get; set; }
}