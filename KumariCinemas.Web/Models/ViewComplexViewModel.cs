using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace KumariCinemas.Web.Models;

public class ViewComplexViewModel
{
    public List<MovieShowtimeDetail> MovieShowtimeDetails { get; set; } = new();

    public int TotalBookings => MovieShowtimeDetails.Count;

    public int BookedCount => MovieShowtimeDetails.Count(d => d.BookingStatus == "Booked");

    public int PendingCount => MovieShowtimeDetails.Count(d => d.BookingStatus == "Pending");

    public int CancelledCount => MovieShowtimeDetails.Count(d => d.BookingStatus == "Cancelled");
}

public class MovieShowtimeDetail
{
    public int TicketId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public string HallCity { get; set; } = string.Empty;

    public DateTime ShowDate { get; set; }

    public string ShowTimeName { get; set; } = string.Empty;

    public decimal TicketPrice { get; set; }

    public string Username { get; set; } = string.Empty;

    public string BookingStatus { get; set; } = string.Empty;
}

public class UserTicketViewModel
{
    public int? SelectedUserId { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public List<SelectListItem> Users { get; set; } = new();

    public List<UserTicketDetail> Tickets { get; set; } = new();
}

public class UserTicketDetail
{
    public int TicketId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public string TheaterCity { get; set; } = string.Empty;

    public string TheaterName { get; set; } = string.Empty;

    public DateTime ShowDate { get; set; }

    public string ShowTimeName { get; set; } = string.Empty;

    public decimal TicketPrice { get; set; }

    public string BookingStatus { get; set; } = string.Empty;
}

public class TheaterCityHallMovieViewModel
{
    public int? SelectedHallId { get; set; }

    public List<SelectListItem> Halls { get; set; } = new();

    public List<TheaterCityHallMovieDetail> Movies { get; set; } = new();
}

public class TheaterCityHallMovieDetail
{
    public string TheaterName { get; set; } = string.Empty;

    public string TheaterCity { get; set; } = string.Empty;

    public string MovieTitle { get; set; } = string.Empty;

    public string Duration { get; set; } = string.Empty;

    public string Genre { get; set; } = string.Empty;

    public string Language { get; set; } = string.Empty;

    public DateTime ShowDate { get; set; }

    public string ShowTimeName { get; set; } = string.Empty;

    public decimal TicketPrice { get; set; }
}

public class MovieTheaterCityHallOccupancyViewModel
{
    public int? SelectedMovieId { get; set; }

    public List<SelectListItem> Movies { get; set; } = new();

    public List<MovieTheaterCityHallOccupancyDetail> Occupancies { get; set; } = new();
}

public class MovieTheaterCityHallOccupancyDetail
{
    public string TheaterName { get; set; } = string.Empty;

    public string TheaterCity { get; set; } = string.Empty;

    public int HallCapacity { get; set; }

    public int PaidTickets { get; set; }

    public double OccupancyPercentage { get; set; }
}
