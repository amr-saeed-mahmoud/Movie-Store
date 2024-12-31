using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MovieStore.Models.Domain;

namespace MovieStore.Models.Data;

public class AppDbContext : IdentityDbContext<AppUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MovieGenre>()
        .HasOne<Movie>()
        .WithMany()
        .HasForeignKey(mg => mg.MovieId) 
        .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<MovieGenre>()
        .HasOne<Genre>() 
        .WithMany() 
        .HasForeignKey(mg => mg.GenreId) 
        .OnDelete(DeleteBehavior.Cascade);
    }

    public DbSet<Movie>? Movies { get; set; }
    public DbSet<Genre>? Genres { get; set; }
    public DbSet<MovieGenre>? MoviesGenres { get; set; }
}