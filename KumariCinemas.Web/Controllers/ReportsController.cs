using KumariCinemas.Web.Data;
using KumariCinemas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KumariCinemas.Web.Controllers;

public class ReportsController(CinemaDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var tickets = await context.Tickets
            .Include(t => t.Showtime)
            .ThenInclude(s => s!.Movie)
            .ToListAsync();

        var bookingsPerMovie = tickets
            .Where(t => t.Showtime?.Movie is not null)
            .GroupBy(t => t.Showtime!.Movie!.Title)
            .Select(group => new MovieBookingSummary
            {
                MovieTitle = group.Key,
                TotalTickets = group.Count(),
                BookedTickets = group.Count(t => t.BookingStatus == "Booked"),
                CancelledTickets = group.Count(t => t.BookingStatus == "Cancelled")
            })
            .OrderBy(summary => summary.MovieTitle)
            .ToList();

        var bookedCountByShowtime = tickets
            .Where(t => t.BookingStatus == "Booked")
            .GroupBy(t => t.ShowtimeId)
            .ToDictionary(group => group.Key, group => group.Count());

        var showtimes = await context.Showtimes
            .Include(s => s.Movie)
            .Include(s => s.Hall)
            .OrderBy(s => s.ShowDate)
            .ThenBy(s => s.ShowTimeName)
            .ToListAsync();

        var revenuePerShowtime = showtimes
            .Select(showtime =>
            {
                var bookedCount = bookedCountByShowtime.GetValueOrDefault(showtime.ShowTimeId);
                return new ShowtimeRevenueSummary
                {
                    ShowtimeId = showtime.ShowTimeId,
                    MovieTitle = showtime.Movie?.Title ?? "N/A",
                    HallCity = showtime.Hall?.TheaterCity ?? "N/A",
                    ShowDate = showtime.ShowDate,
                    ShowTimeName = showtime.ShowTimeName,
                    TicketPrice = showtime.TicketPrice,
                    BookedCount = bookedCount,
                    Revenue = showtime.TicketPrice * bookedCount
                };
            })
            .ToList();

        var model = new ReportsViewModel
        {
            BookingsPerMovie = bookingsPerMovie,
            RevenuePerShowtime = revenuePerShowtime
        };

        return View(model);
    }
}
