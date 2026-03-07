using System.ComponentModel.DataAnnotations;

namespace KumariCinemas.Web.Models;

public class Movie
{
    [Key]
    public int MovieId { get; set; }

    [Required]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Duration { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Language { get; set; } = string.Empty;

    [Required]
    [StringLength(30)]
    public string Genre { get; set; } = string.Empty;

    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
