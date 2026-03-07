using KumariCinemas.Web.Data;
using KumariCinemas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KumariCinemas.Web.Controllers;

public class HallsController(CinemaDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var halls = await context.TheaterCityHalls
            .OrderBy(h => h.TheaterName)
            .ThenBy(h => h.TheaterCity)
            .ToListAsync();

        return View(halls);
    }

    public async Task<IActionResult> Details(int id)
    {
        var hall = await context.TheaterCityHalls.FirstOrDefaultAsync(h => h.HallId == id);
        if (hall is null)
        {
            return NotFound();
        }

        return View(hall);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new TheaterHall());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TheaterHall hall)
    {
        if (!ModelState.IsValid)
        {
            return View(hall);
        }

        hall.HallId = (await context.TheaterCityHalls.MaxAsync(h => (int?)h.HallId) ?? 0) + 1;
        context.TheaterCityHalls.Add(hall);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var hall = await context.TheaterCityHalls.FindAsync(id);
        if (hall is null)
        {
            return NotFound();
        }

        return View(hall);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TheaterHall hall)
    {
        if (id != hall.HallId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(hall);
        }

        context.TheaterCityHalls.Update(hall);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var hall = await context.TheaterCityHalls.FirstOrDefaultAsync(h => h.HallId == id);
        if (hall is null)
        {
            return NotFound();
        }

        return View(hall);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hasShowtimes = await context.Showtimes.AnyAsync(s => s.HallId == id);
        if (hasShowtimes)
        {
            TempData["Error"] = "Hall cannot be deleted because showtimes exist.";
            return RedirectToAction(nameof(Index));
        }

        var hall = await context.TheaterCityHalls.FindAsync(id);
        if (hall is null)
        {
            return NotFound();
        }

        context.TheaterCityHalls.Remove(hall);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
