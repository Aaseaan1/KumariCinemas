using System.ComponentModel.DataAnnotations;

namespace KumariCinemas.Web.Models;

public class EditTicketViewModel
{
    [Required]
    public int TicketId { get; set; }

    [Required]
    [Display(Name = "User")]
    public int UserId { get; set; }

    [Required]
    [Display(Name = "Showtime")]
    public int ShowtimeId { get; set; }

    [Required]
    [StringLength(20)]
    [Display(Name = "Booking Status")]
    public string BookingStatus { get; set; } = "Booked";
}
