namespace KumariCinemas.Web.Models;

public class ReportsViewModel
{
    public List<MovieBookingSummary> BookingsPerMovie { get; set; } = new();

    public List<ShowtimeRevenueSummary> RevenuePerShowtime { get; set; } = new();
}

public class MovieBookingSummary
{
    public string MovieTitle { get; set; } = string.Empty;

    public int TotalTickets { get; set; }

    public int BookedTickets { get; set; }

    public int CancelledTickets { get; set; }
}

public class ShowtimeRevenueSummary
{
    public int ShowtimeId { get; set; }

    public string MovieTitle { get; set; } = string.Empty;

    public string HallCity { get; set; } = string.Empty;

    public DateTime ShowDate { get; set; }

    public string ShowTimeName { get; set; } = string.Empty;

    public decimal TicketPrice { get; set; }

    public int BookedCount { get; set; }

    public decimal Revenue { get; set; }
}
