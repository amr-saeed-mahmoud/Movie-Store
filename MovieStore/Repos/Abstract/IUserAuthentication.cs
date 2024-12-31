using MovieStore.Models.DTO;

namespace MovieStore.Repos.Abstract;


public interface IUserAuthentication
{
    Task<Status> SignInAsync(SignInDto SignInInfo);
    Task<Status> LogInAsync(LogInDto LogInInfo);
    Task LogoutAsync();
}