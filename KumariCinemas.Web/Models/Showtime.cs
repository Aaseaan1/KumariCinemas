using System.ComponentModel.DataAnnotations;

namespace KumariCinemas.Web.Models;

public class Showtime
{
    [Key]
    public int ShowTimeId { get; set; }

    [Required]
    public int MovieId { get; set; }

    [Required]
    public int HallId { get; set; }

    [DataType(DataType.Date)]
    public DateTime ShowDate { get; set; }

    [Required]
    [StringLength(20)]
    public string ShowTimeName { get; set; } = string.Empty;

    [Range(0, 100000)]
    public decimal TicketPrice { get; set; }

    public Movie? Movie { get; set; }

    public TheaterHall? Hall { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
