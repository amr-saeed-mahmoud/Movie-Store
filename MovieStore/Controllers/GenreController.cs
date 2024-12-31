using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using MovieStore.Models.Domain;
using MovieStore.UnitOfWork.Abstract;

namespace MovieStore.Controllers;

public class GenreController : Controller
{
    private readonly IMainUnit _MainUnit;

    public GenreController(IMainUnit mainUnit)
    {
        _MainUnit = mainUnit;
    }

    [HttpGet]
    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add(Genre model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        var result = await _MainUnit.Genres.AddAsync(model);
        if(result)
        {
            TempData["msg"] = "Added Successfully";
            return RedirectToAction(nameof(GenreList));
        }
        TempData["msg"] = "Error on server side";
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int Id)
    {
        Genre? model = await _MainUnit.Genres.GetByIdAsync(Id);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Genre model)
    {
        if(!ModelState.IsValid)
        {
            return View(model);
        }
        var result = await _MainUnit.Genres.UpdateAsync(model);
        if(result)
        {
            TempData["msg"] = "Updated Successfully";
            return RedirectToAction(nameof(GenreList));
        }
        TempData["msg"] = "Error on server side";
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GenreList()
    {
        var Genres = await _MainUnit.Genres.GetAllAsync();
        return View(Genres);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int Id)
    {
        await _MainUnit.Genres.DeleteAsync(Id);
        TempData["msg"] = "Deleted Successfully";
        return RedirectToAction(nameof(GenreList));
    }

}