using KumariCinemas.Web.Data;
using KumariCinemas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KumariCinemas.Web.Controllers;

public class BookingsController(CinemaDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var tickets = await context.Tickets
            .Include(t => t.User)
            .Include(t => t.Showtime)
            .ThenInclude(s => s!.Movie)
            .Include(t => t.Showtime)
            .ThenInclude(s => s!.Hall)
            .OrderBy(t => t.TicketId)
            .ToListAsync();

        return View(tickets);
    }

    public async Task<IActionResult> Details(int id)
    {
        var ticket = await context.Tickets
            .Include(t => t.User)
            .Include(t => t.Showtime)
            .ThenInclude(s => s!.Movie)
            .Include(t => t.Showtime)
            .ThenInclude(s => s!.Hall)
            .FirstOrDefaultAsync(t => t.TicketId == id);

        if (ticket is null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await PopulateDropdownsAsync();
        return View(new CreateTicketViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTicketViewModel model)
    {
        await PopulateDropdownsAsync(model.UserId, model.ShowtimeId);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var showtime = await context.Showtimes
            .Include(s => s.Hall)
            .FirstOrDefaultAsync(s => s.ShowTimeId == model.ShowtimeId);

        if (showtime is null || showtime.Hall is null)
        {
            ModelState.AddModelError(string.Empty, "Invalid showtime selected.");
            return View(model);
        }

        var bookedCount = await context.Tickets.CountAsync(t =>
            t.ShowtimeId == model.ShowtimeId && t.BookingStatus == "Booked");

        if (bookedCount >= showtime.Hall.HallCapacity)
        {
            ModelState.AddModelError(string.Empty, "This showtime is full. Please choose another one.");
            return View(model);
        }

        var nextTicketId = (await context.Tickets.MaxAsync(t => (int?)t.TicketId) ?? 0) + 1;

        var ticket = new Ticket
        {
            TicketId = nextTicketId,
            UserId = model.UserId,
            ShowtimeId = model.ShowtimeId,
            BookingStatus = "Booked"
        };

        context.Tickets.Add(ticket);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var ticket = await context.Tickets.FindAsync(id);
        if (ticket is null)
        {
            return NotFound();
        }

        var model = new EditTicketViewModel
        {
            TicketId = ticket.TicketId,
            UserId = ticket.UserId,
            ShowtimeId = ticket.ShowtimeId,
            BookingStatus = ticket.BookingStatus
        };

        await PopulateDropdownsAsync(model.UserId, model.ShowtimeId);
        PopulateStatusDropdown(model.BookingStatus);
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditTicketViewModel model)
    {
        if (id != model.TicketId)
        {
            return BadRequest();
        }

        await PopulateDropdownsAsync(model.UserId, model.ShowtimeId);
        PopulateStatusDropdown(model.BookingStatus);

        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var ticket = await context.Tickets.FindAsync(id);
        if (ticket is null)
        {
            return NotFound();
        }

        if (string.Equals(model.BookingStatus, "Booked", StringComparison.OrdinalIgnoreCase))
        {
            var showtime = await context.Showtimes
                .Include(s => s.Hall)
                .FirstOrDefaultAsync(s => s.ShowTimeId == model.ShowtimeId);

            if (showtime is null || showtime.Hall is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid showtime selected.");
                return View(model);
            }

            var bookedCount = await context.Tickets.CountAsync(t =>
                t.ShowtimeId == model.ShowtimeId &&
                t.BookingStatus == "Booked" &&
                t.TicketId != model.TicketId);

            if (bookedCount >= showtime.Hall.HallCapacity)
            {
                ModelState.AddModelError(string.Empty, "This showtime is full. Please choose another one.");
                return View(model);
            }
        }

        ticket.UserId = model.UserId;
        ticket.ShowtimeId = model.ShowtimeId;
        ticket.BookingStatus = model.BookingStatus;

        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var ticket = await context.Tickets
            .Include(t => t.User)
            .Include(t => t.Showtime)
            .ThenInclude(s => s!.Movie)
            .Include(t => t.Showtime)
            .ThenInclude(s => s!.Hall)
            .FirstOrDefaultAsync(t => t.TicketId == id);

        if (ticket is null)
        {
            return NotFound();
        }

        return View(ticket);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var ticket = await context.Tickets.FindAsync(id);
        if (ticket is null)
        {
            return NotFound();
        }

        context.Tickets.Remove(ticket);
        await context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateDropdownsAsync(int? selectedUserId = null, int? selectedShowtimeId = null)
    {
        var users = await context.Users
            .OrderBy(u => u.Username)
            .ToListAsync();

        var showtimes = await context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .OrderBy(s => s.ShowDate)
            .ThenBy(s => s.ShowTimeName)
            .Select(s => new
            {
                s.ShowTimeId,
                Label = s.Movie!.Title + " | " + s.ShowDate.ToString("yyyy-MM-dd") + " " + s.ShowTimeName + " | " + s.Hall!.TheaterCity
            })
            .ToListAsync();

        ViewBag.Users = new SelectList(users, "UserId", "Username", selectedUserId);
        ViewBag.Showtimes = new SelectList(showtimes, "ShowTimeId", "Label", selectedShowtimeId);
    }

    private void PopulateStatusDropdown(string? selectedStatus = null)
    {
        var statuses = new[] { "Booked", "Cancelled" };
        ViewBag.Statuses = new SelectList(statuses, selectedStatus);
    }
}
