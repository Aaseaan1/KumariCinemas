using System.ComponentModel.DataAnnotations;

namespace KumariCinemas.Web.Models;

public class TheaterHall
{
    [Key]
    public int HallId { get; set; }

    [Required]
    [StringLength(100)]
    public string TheaterName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string TheaterCity { get; set; } = string.Empty;

    [Range(1, 1000)]
    public int HallCapacity { get; set; }

    public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
