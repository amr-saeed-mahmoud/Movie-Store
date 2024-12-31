using System.ComponentModel.DataAnnotations;

namespace MovieStore.Models.Domain;


public class Genre
{
    [Key]
    public int Id { get; set; }
    
    [Required, MaxLength(50)]
    public string? GenreName { get; set; }
}