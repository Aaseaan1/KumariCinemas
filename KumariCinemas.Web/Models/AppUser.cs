using System.ComponentModel.DataAnnotations;

namespace KumariCinemas.Web.Models;

public class AppUser
{
    [Key]
    public int UserId { get; set; }

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = string.Empty;

    [StringLength(100)]
    public string? Address { get; set; }

    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
