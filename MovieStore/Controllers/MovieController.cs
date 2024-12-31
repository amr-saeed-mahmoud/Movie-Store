using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MovieStore.Models.Domain;
using MovieStore.Models.Dto;
using MovieStore.UnitOfWork.Abstract;
using MovieStore.UnitOfWork.Implementation;

namespace MovieStore.Controllers;

public class MovieController : Controller
{
    private readonly IMainUnit _MainUnit;

    public MovieController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [HttpGet]
    public async Task<IActionResult> Add()
    {
        var Genres = await _MainUnit.Genres.GetAllAsync();
        MovieDto model = new MovieDto()
        {
            GenresList = Genres.Select(item => new SelectListItem { Text = item.GenreName, Value = item.Id.ToString() })
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Add(MovieDto model)
    {
        string ImageName = string.Empty;
        if(!ModelState.IsValid)
        {
            // var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
            var Genres = await _MainUnit.Genres.GetAllAsync();
            model.GenresList = Genres.Select(item => new SelectListItem { Text = item.GenreName, Value = item.Id.ToString() });
            return View(model);
        }
        if(model.ImageFile != null)
        {
            var result = _MainUnit.FileService.SaveImage(model.ImageFile);
            if(result.Item1 == 0)
            {
                TempData["msg"] = "File could not saved";
                return View(model);
            }
            ImageName = result.Item2;
        }
        Movie NewMovie = new Movie
        {
            Title = model.Title,
            ReleaseYear = model.ReleaseYear,
            MovieImage = ImageName,
            Cast = model.Cast,
            Director = model.Director,
            Genres = model.Genres
        };
        var res = await _MainUnit.Movies.AddAsync(NewMovie);
        if(res)
        {
            TempData["msg"] = "Added Successfully";
            return RedirectToAction(nameof(MovieList));// _________________________________
        }
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        if(!ModelState.IsValid)
        {
            return View();
        }

        Movie? model = await _MainUnit.Movies.GetByIdAsync(id);
        var selectedGenres = _MainUnit.Movies.GetGenreByMovieId(model!.Id);
        MultiSelectList multiGenreList = new MultiSelectList(await _MainUnit.Genres.GetAllAsync(), "Id", "GenreName", selectedGenres);
        model.MultiGenreList = multiGenreList;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Movie model)
    {
        var selectedGenres = _MainUnit.Movies.GetGenreByMovieId(model.Id);
        if (!ModelState.IsValid)
        {
            MultiSelectList multiGenreList =  new MultiSelectList(await _MainUnit.Genres.GetAllAsync(), "Id", "GenreName", selectedGenres);
            model.MultiGenreList = multiGenreList;
            return View(model);
        }
        if (model.ImageFile != null)
        {
            var fileReult = _MainUnit.FileService.SaveImage(model.ImageFile);
            if (fileReult.Item1 == 0)
            {
                TempData["msg"] = "File could not saved";
                return View(model);
            }
            var imageName = fileReult.Item2;
            model.MovieImage = imageName;
        }
        var result = await _MainUnit.Movies.UpdateAsync(model);
        if (result)
        {
            TempData["msg"] = "Updated Successfully";
            return RedirectToAction(nameof(MovieList));
        }
        else
        {
            TempData["msg"] = "Error on server side";
            return View(model);
        }
    }

    public async Task<IActionResult> MovieList()
    {
        var data = await _MainUnit.Movies.FetchMoviesWithPagingAsync();
        return View(data);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _MainUnit.Movies.DeleteAsync(id);
        return RedirectToAction(nameof(MovieList));
    }

}