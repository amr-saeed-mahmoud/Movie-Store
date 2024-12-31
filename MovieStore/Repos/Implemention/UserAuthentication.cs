using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MovieStore.Models.Domain;
using MovieStore.Models.DTO;
using MovieStore.Repos.Abstract;

namespace MovieStore.Repos.Implemention;


public class UserAuthentication : IUserAuthentication
{

    private readonly UserManager<AppUser> _UserManager;
    private readonly RoleManager<IdentityRole> _RoleManager;
    private readonly SignInManager<AppUser> _SignInManager;

    public UserAuthentication
       (UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<AppUser> signInManager)
    {
        _UserManager = userManager;
        _RoleManager = roleManager;
        _SignInManager = signInManager;
    }

    public async Task<Status> LogInAsync(LogInDto LogInInfo)
    {
        Status status = new Status();

        AppUser? User = await _UserManager.FindByNameAsync(LogInInfo.UserName ?? "");
        if(User == null)
        {
            status.StatusCode = 0;
            status.Message = "username is not exist.";
            return status;
        }

        if(LogInInfo.Password != null && !await _UserManager.CheckPasswordAsync(User, LogInInfo.Password))
        {
            status.StatusCode = 0;
            status.Message = "invalid password.";
            return status;           
        }

        var SignInResult = await _SignInManager.PasswordSignInAsync(User, LogInInfo.Password!, false, false);
        if(!SignInResult.Succeeded)
        {
            status.StatusCode = 0;
            status.Message = "Error on logging in";
            return status;
        }
        
        var userRoles = await _UserManager.GetRolesAsync(User);
        var authClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, User.UserName!)
        };
        foreach (var userRole in userRoles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        status.StatusCode = 1;
        status.Message = "Logged in successfully";
        return status;
    }

    public async Task LogoutAsync()
    {
        await _SignInManager.SignOutAsync();
    }

    public async Task<Status> SignInAsync(SignInDto SignInInfo)
    {
        Status status = new Status();
        AppUser? UserExists = await _UserManager.FindByNameAsync(SignInInfo.UserName ?? "");
        if(UserExists != null)
        {
            status.StatusCode = 0;
            status.Message = "user already exists.";
            return status;
        }
        AppUser User = new AppUser()
        {
            Email = SignInInfo.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = SignInInfo.UserName,
            FullName = SignInInfo.FullName,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true
        };

        if(SignInInfo.Password == null || SignInInfo.Role == null)
        {
            status.StatusCode = 0;
            status.Message = "Faield";
            return status;
        }

        var result = await _UserManager.CreateAsync(User, SignInInfo.Password);
        
        if(!result.Succeeded)
        {
            status.StatusCode = 0;
            status.Message = "User creation failed";
            return status;
        }
        
        if(!await _RoleManager.RoleExistsAsync(SignInInfo.Role))
        {
            await _RoleManager.CreateAsync(new IdentityRole(SignInInfo.Role));
        }

        await _UserManager.AddToRoleAsync(User, SignInInfo.Role);

        status.StatusCode = 1;
        status.Message = "the user was created successfully.";
        return status;
    }
}