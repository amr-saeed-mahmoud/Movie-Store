using System.ComponentModel.DataAnnotations;

namespace MovieStore.Models.DTO;

public class SignInDto
{
    [Required]
    public string? FullName { get; set; }

    [Required]
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    [Required]
    [Compare("Password")]
    public string? PasswordConfirm { get; set; }

    [Required]
    public string? Role { get; set; }
    
}