using Microsoft.AspNetCore.Mvc;
using MovieStore.UnitOfWork.Abstract;

namespace MovieStore.Controllers;

public class HomeController : Controller
{
    private readonly IMainUnit _MainUnit;

    public HomeController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    public async Task<IActionResult> Index(string term="", int currentPage = 1)
    {
        var Movies = await _MainUnit.Movies.FetchMoviesWithPagingAsync(term,true,currentPage);
        return View(Movies);
    }

    public IActionResult About()
    {
        return View();
    }

    public async Task<IActionResult> MovieDetail(int movieId)
    {
        var movie = await _MainUnit.Movies.GetByIdAsync(movieId);
        return View(movie);
    }
}