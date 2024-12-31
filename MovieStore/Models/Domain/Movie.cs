using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MovieStore.Models.Domain;


public class Movie
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(50)]
    public string? Title { get; set; }
    
    public string? ReleaseYear { get; set; }
    public string? MovieImage { get; set; }  // stores movie image name with extension (eg, image0001.jpg)
    
    [Required, MaxLength(50)]
    public string? Cast { get; set; }
    
    [Required, MaxLength(200)]
    public string? Director { get; set; }

    [NotMapped]
    [Required]
    public List<int>? Genres { get; set; } = new List<int>();

    [Display(Name = "Image File")]
    [NotMapped]
    public IFormFile? ImageFile { get; set; }

    [NotMapped]
    public string? GenreNames { get; set; }

    [NotMapped]
    public MultiSelectList ? MultiGenreList { get; set; }

}