using KumariCinemas.Web.Data;
using KumariCinemas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KumariCinemas.Web.Controllers;

public class UsersController(CinemaDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = await context.Users
            .OrderBy(u => u.Username)
            .ToListAsync();

        return View(users);
    }

    public async Task<IActionResult> Details(int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user is null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AppUser());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(AppUser user)
    {
        if (!ModelState.IsValid)
        {
            return View(user);
        }

        user.UserId = (await context.Users.MaxAsync(u => (int?)u.UserId) ?? 0) + 1;
        context.Users.Add(user);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var user = await context.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, AppUser user)
    {
        if (id != user.UserId)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(user);
        }

        context.Users.Update(user);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserId == id);
        if (user is null)
        {
            return NotFound();
        }

        return View(user);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var hasTickets = await context.Tickets.AnyAsync(t => t.UserId == id);
        if (hasTickets)
        {
            TempData["Error"] = "User cannot be deleted because tickets exist.";
            return RedirectToAction(nameof(Index));
        }

        var user = await context.Users.FindAsync(id);
        if (user is null)
        {
            return NotFound();
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
