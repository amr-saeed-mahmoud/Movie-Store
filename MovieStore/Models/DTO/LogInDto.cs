using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace MovieStore.Models.DTO;

public class LogInDto
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    public string? Password { get; set; }
}