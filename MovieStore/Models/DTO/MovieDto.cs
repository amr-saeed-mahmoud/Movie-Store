using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStore.Models.Domain;

namespace MovieStore.Models.Dto;

[NotMapped]
public class MovieDto : Movie
{

    [Display(Name = "Genres List")]
    [NotMapped]
    public IEnumerable<SelectListItem>? GenresList { get; set; }
}