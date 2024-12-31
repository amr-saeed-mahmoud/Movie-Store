using Microsoft.AspNetCore.Mvc;
using MovieStore.Models.DTO;
using MovieStore.Repos.Abstract;

namespace MovieStore.Controllers;

public class AuthonticationController : Controller
{
    private readonly IUserAuthentication _Auth;

    public AuthonticationController(IUserAuthentication userAuthentication)
    {
        _Auth = userAuthentication;
    }

    public async Task<IActionResult> Register()
    {
       var model = new SignInDto
       {
           Email = "amr@gmail.com",
           UserName = "amr",
           FullName = "Amr Saeed",
           Password = "A553310m@",
           PasswordConfirm = "A553310m",
           Role = "Admin"
       };
       var result = await _Auth.SignInAsync(model);
       return Ok(result.Message);
    }

    [HttpGet]
    public IActionResult LogIn()
    {
        LogInDto model = new LogInDto();
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInDto model)
    {
        if (!ModelState.IsValid)
            return View(model);
        var result = await _Auth.LogInAsync(model);
        if (result.StatusCode == 1)
            return RedirectToAction("Index", "Home");
        else
        {
            TempData["msg"] = "Could not logged in..";
            return RedirectToAction(nameof(LogIn));
        }
    }

    public async Task<IActionResult> Logout()
    {
       await _Auth.LogoutAsync();
        return RedirectToAction(nameof(LogIn));
    }
}