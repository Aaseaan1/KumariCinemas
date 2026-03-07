using System.ComponentModel.DataAnnotations;

namespace KumariCinemas.Web.Models;

public class CreateTicketViewModel
{
    [Required]
    [Display(Name = "User")]
    public int UserId { get; set; }

    [Required]
    [Display(Name = "Showtime")]
    public int ShowtimeId { get; set; }
}
