using KumariCinemas.Web.Data;
using KumariCinemas.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace KumariCinemas.Web.Controllers;

public class ViewComplexController(CinemaDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var details = await context.Tickets
            .Include(t => t.Showtime)
            .ThenInclude(s => s!.Movie)
            .Include(t => t.Showtime)
            .ThenInclude(s => s!.Hall)
            .Include(t => t.User)
            .Select(t => new MovieShowtimeDetail
            {
                TicketId = t.TicketId,
                MovieTitle = t.Showtime!.Movie!.Title,
                HallCity = t.Showtime.Hall!.TheaterCity,
                ShowDate = t.Showtime.ShowDate,
                ShowTimeName = t.Showtime.ShowTimeName,
                TicketPrice = t.Showtime.TicketPrice,
                Username = t.User!.Username,
                BookingStatus = t.BookingStatus
            })
            .OrderBy(d => d.TicketId)
            .ToListAsync();

        var model = new ViewComplexViewModel
        {
            MovieShowtimeDetails = details
        };

        return View(model);
    }

    public async Task<IActionResult> UserTicket(int? selectedUserId, DateTime? startDate, DateTime? endDate)
    {
        var model = new UserTicketViewModel();

        model.Users = await context.Users
            .OrderBy(u => u.Username)
            .Select(u => new SelectListItem
            {
                Value = u.UserId.ToString(),
                Text = u.Username
            })
            .ToListAsync();

        if (selectedUserId.HasValue)
        {
            model.SelectedUserId = selectedUserId.Value;

            var start = startDate ?? DateTime.Today.AddMonths(-6);
            var end = endDate ?? DateTime.Today;

            model.StartDate = start;
            model.EndDate = end;

            model.Tickets = await context.Tickets
                .Include(t => t.Showtime)
                .ThenInclude(s => s!.Movie)
                .Include(t => t.Showtime)
                .ThenInclude(s => s!.Hall)
                .Include(t => t.User)
                .Where(t => t.UserId == selectedUserId.Value &&
                            t.Showtime!.ShowDate >= start &&
                            t.Showtime!.ShowDate <= end)
                .OrderByDescending(t => t.Showtime!.ShowDate)
                .Select(t => new UserTicketDetail
                {
                    TicketId = t.TicketId,
                    MovieTitle = t.Showtime!.Movie!.Title,
                    TheaterCity = t.Showtime.Hall!.TheaterCity,
                    TheaterName = t.Showtime.Hall.TheaterName,
                    ShowDate = t.Showtime.ShowDate,
                    ShowTimeName = t.Showtime.ShowTimeName,
                    TicketPrice = t.Showtime.TicketPrice,
                    BookingStatus = t.BookingStatus
                })
                .ToListAsync();
        }
        else
        {
            model.StartDate = DateTime.Today.AddMonths(-6);
            model.EndDate = DateTime.Today;
        }

        return View(model);
    }

    public async Task<IActionResult> TheaterCityHallMovie(int? selectedHallId)
    {
        var model = new TheaterCityHallMovieViewModel();

        model.Halls = await context.TheaterCityHalls
            .OrderBy(h => h.TheaterName)
            .Select(h => new SelectListItem
            {
                Value = h.HallId.ToString(),
                Text = $"{h.TheaterName} - {h.TheaterCity}"
            })
            .ToListAsync();

        if (selectedHallId.HasValue)
        {
            model.SelectedHallId = selectedHallId.Value;

            model.Movies = await context.Showtimes
                .Include(s => s.Movie)
                .Include(s => s.Hall)
                .Where(s => s.HallId == selectedHallId.Value)
                .OrderBy(s => s.ShowDate)
                .ThenBy(s => s.ShowTimeName)
                .Select(s => new TheaterCityHallMovieDetail
                {
                    TheaterName = s.Hall!.TheaterName,
                    TheaterCity = s.Hall.TheaterCity,
                    MovieTitle = s.Movie!.Title,
                    Duration = s.Movie.Duration,
                    Genre = s.Movie.Genre,
                    Language = s.Movie.Language,
                    ShowDate = s.ShowDate,
                    ShowTimeName = s.ShowTimeName,
                    TicketPrice = s.TicketPrice
                })
                .ToListAsync();
        }

        return View(model);
    }

    public async Task<IActionResult> MovieTheaterCityHallOccupancy(int? selectedMovieId)
    {
        var model = new MovieTheaterCityHallOccupancyViewModel();

        model.Movies = await context.Movies
            .OrderBy(m => m.Title)
            .Select(m => new SelectListItem
            {
                Value = m.MovieId.ToString(),
                Text = m.Title
            })
            .ToListAsync();

        if (selectedMovieId.HasValue)
        {
            model.SelectedMovieId = selectedMovieId.Value;

            var showtimes = await context.Showtimes
                .Include(s => s.Hall)
                .Include(s => s.Tickets)
                .Where(s => s.MovieId == selectedMovieId.Value)
                .ToListAsync();

            var occupancyData = showtimes
                .GroupBy(s => new { s.Hall!.HallId, s.Hall.TheaterName, s.Hall.TheaterCity, s.Hall.HallCapacity })
                .Select(g =>
                {
                    var totalTickets = g.SelectMany(s => s.Tickets!).Count(t => t.BookingStatus == "Booked");
                    var capacity = g.Key.HallCapacity;
                    var percentage = capacity > 0 ? (double)totalTickets / capacity * 100 : 0;

                    return new MovieTheaterCityHallOccupancyDetail
                    {
                        TheaterName = g.Key.TheaterName,
                        TheaterCity = g.Key.TheaterCity,
                        HallCapacity = capacity,
                        PaidTickets = totalTickets,
                        OccupancyPercentage = Math.Round(percentage, 2)
                    };
                })
                .OrderByDescending(o => o.OccupancyPercentage)
                .Take(3)
                .ToList();

            model.Occupancies = occupancyData;
        }

        return View(model);
    }
}
