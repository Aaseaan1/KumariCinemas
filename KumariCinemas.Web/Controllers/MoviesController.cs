using KumariCinemas.Web.Data;
using KumariCinemas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KumariCinemas.Web.Controllers;

public class MoviesController(CinemaDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var movies = await context.Movies
            .OrderBy(m => m.Title)
            .ToListAsync();

        return View(movies);
    }

    public async Task<IActionResult> Details(int id)
    {
        var movie = await context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
        if (movie is null)
        {
            return NotFound();
        }

        return View(movie);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new Movie { ReleaseDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Movie movie)
    {
        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        movie.MovieId = (await context.Movies.MaxAsync(m => (int?)m.MovieId) ?? 0) + 1;
        context.Movies.Add(movie);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var movie = await context.Movies.FindAsync(id);
        if (movie is null)
        {
            return NotFound();
        }

        return View(movie);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Movie movie)
    {
        if (id != movie.MovieId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(movie);
        }

        context.Movies.Update(movie);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var movie = await context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
        if (movie is null)
        {
            return NotFound();
        }

        return View(movie);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var inUse = await context.Showtimes.AnyAsync(s => s.MovieId == id);
        if (inUse)
        {
            TempData["Error"] = "Movie cannot be deleted because showtimes exist.";
            return RedirectToAction(nameof(Index));
        }

        var movie = await context.Movies.FindAsync(id);
        if (movie is null)
        {
            return NotFound();
        }

        context.Movies.Remove(movie);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
