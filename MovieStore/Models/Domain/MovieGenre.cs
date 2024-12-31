using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieStore.Models.Domain;


public class MovieGenre
{
    [Key]
    public int Id { get; set; }
    
    [ForeignKey("Movie")]
    public int MovieId { get; set; }
    
    [ForeignKey("Genre")]
    public int GenreId { get; set; }
}