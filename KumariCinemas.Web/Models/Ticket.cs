using System.ComponentModel.DataAnnotations;

namespace KumariCinemas.Web.Models;

public class Ticket
{
    [Key]
    public int TicketId { get; set; }

    [Required]
    public int UserId { get; set; }

    [Required]
    public int ShowtimeId { get; set; }

    [Required]
    [StringLength(20)]
    public string BookingStatus { get; set; } = "Booked";

    public AppUser? User { get; set; }

    public Showtime? Showtime { get; set; }
}
