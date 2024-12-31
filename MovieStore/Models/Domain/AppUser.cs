using Microsoft.AspNetCore.Identity;

namespace MovieStore.Models.Domain;

public class AppUser : IdentityUser
{
    public string? FullName { get; set; }
}