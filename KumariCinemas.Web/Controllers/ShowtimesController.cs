using KumariCinemas.Web.Data;
using KumariCinemas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KumariCinemas.Web.Controllers;

public class ShowtimesController(CinemaDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var showtimes = await context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .OrderBy(s => s.ShowDate)
            .ThenBy(s => s.ShowTimeName)
            .ToListAsync();

        return View(showtimes);
    }

    public async Task<IActionResult> Details(int id)
    {
        var showtime = await context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .FirstOrDefaultAsync(s => s.ShowTimeId == id);

        if (showtime is null)
        {
            return NotFound();
        }

        return View(showtime);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View(new Showtime { ShowDate = DateTime.Today });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Showtime showtime)
    {
        await PopulateDropdownsAsync();

        if (!ModelState.IsValid)
        {
            return View(showtime);
        }

        showtime.ShowTimeId = (await context.Showtimes.MaxAsync(s => (int?)s.ShowTimeId) ?? 0) + 1;
        context.Showtimes.Add(showtime);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var showtime = await context.Showtimes.FindAsync(id);
        if (showtime is null)
        {
            return NotFound();
        }

        await PopulateDropdownsAsync(showtime.MovieId, showtime.HallId);
        return View(showtime);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Showtime showtime)
    {
        if (id != showtime.ShowTimeId)
        {
            return BadRequest();
        }

        await PopulateDropdownsAsync(showtime.MovieId, showtime.HallId);

        if (!ModelState.IsValid)
        {
            return View(showtime);
        }

        context.Showtimes.Update(showtime);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var showtime = await context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .FirstOrDefaultAsync(s => s.ShowTimeId == id);

        if (showtime is null)
        {
            return NotFound();
        }

        return View(showtime);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hasTickets = await context.Tickets.AnyAsync(t => t.ShowtimeId == id);
        if (hasTickets)
        {
            TempData["Error"] = "Showtime cannot be deleted because tickets exist.";
            return RedirectToAction(nameof(Index));
        }

        var showtime = await context.Showtimes.FindAsync(id);
        if (showtime is null)
        {
            return NotFound();
        }

        context.Showtimes.Remove(showtime);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdownsAsync(int? selectedMovieId = null, int? selectedHallId = null)
    {
        var movies = await context.Movies
            .OrderBy(m => m.Title)
            .ToListAsync();

        var halls = await context.TheaterCityHalls
            .OrderBy(h => h.TheaterCity)
            .ToListAsync();

        ViewBag.Movies = new SelectList(movies, "MovieId", "Title", selectedMovieId);
        ViewBag.Halls = new SelectList(halls, "HallId", "TheaterCity", selectedHallId);
    }
}
