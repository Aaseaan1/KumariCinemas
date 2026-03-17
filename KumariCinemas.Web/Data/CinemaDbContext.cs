
namespace KumariCinemas.Web.Data;

public class CinemaDbContext(DbContextOptions<CinemaDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<TheaterHall> TheaterCityHalls => Set<TheaterHall>();
    public DbSet<Showtime> Showtimes => Set<Showtime>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.ToTable("Users");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.Username).HasColumnName("Username");
            entity.Property(e => e.Address).HasColumnName("Address");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.ToTable("Movie");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.Title).HasColumnName("Title");
            entity.Property(e => e.Duration).HasColumnName("Duration");
            entity.Property(e => e.Language).HasColumnName("Language");
            entity.Property(e => e.Genre).HasColumnName("Genre");
            entity.Property(e => e.ReleaseDate).HasColumnName("ReleaseDate");
        });

        modelBuilder.Entity<TheaterHall>(entity =>
        {
            entity.ToTable("Theater_City_Hall");
            entity.Property(e => e.HallId).HasColumnName("HallID");
            entity.Property(e => e.TheaterName).HasColumnName("TheaterName");
            entity.Property(e => e.TheaterCity).HasColumnName("TheaterCity");
            entity.Property(e => e.HallCapacity).HasColumnName("HallCapacity");
        });

        modelBuilder.Entity<Showtime>(entity =>
        {
            entity.ToTable("Showtime");
            entity.Property(e => e.ShowTimeId).HasColumnName("ShowTimeID");
            entity.Property(e => e.MovieId).HasColumnName("MovieID");
            entity.Property(e => e.HallId).HasColumnName("HallID");
            entity.Property(e => e.ShowDate).HasColumnName("ShowDate");
            entity.Property(e => e.ShowTimeName).HasColumnName("ShowTime");
            entity.Property(e => e.TicketPrice).HasColumnName("TicketPrice");
        });

        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.ToTable("Ticket");
            entity.Property(e => e.TicketId).HasColumnName("TicketID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.ShowtimeId).HasColumnName("ShowtimeID");
            entity.Property(e => e.BookingStatus).HasColumnName("BookingStatus");
        });

        modelBuilder.Entity<Showtime>()
            .HasOne(s => s.Movie)
            .WithMany(m => m.Showtimes)
            .HasForeignKey(s => s.MovieId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Showtime>()
            .HasOne(s => s.Hall)
            .WithMany(h => h.Showtimes)
            .HasForeignKey(s => s.HallId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tickets)
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Ticket>()
            .HasOne(t => t.Showtime)
            .WithMany(s => s.Tickets)
            .HasForeignKey(t => t.ShowtimeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<AppUser>().HasData(
            new AppUser { UserId = 1, Username = "Pratibha Gurung", Address = "Pokhara" }
            
        );

        modelBuilder.Entity<Movie>().HasData(
            new Movie
            {
                MovieId = 1,
                Title = "Avatar: Fire and Ash",
                Duration = "3h",
                Language = "English",
                Genre = "Fiction",
                ReleaseDate = new DateTime(2025, 12, 19)
            },
            new Movie
            {
                MovieId = 3,
                Title = "Sully: Miracle On The Hudson",
                Duration = "1hr 35mins",
                Language = "English",
                Genre = "Drama-History",
                ReleaseDate = new DateTime(2016, 9, 9)
            }
        );

        modelBuilder.Entity<TheaterHall>().HasData(
            new TheaterHall
            {
                HallId = 1,
                TheaterName = "Kumari Cinemas",
                TheaterCity = "Pokhara Cineplex",
                HallCapacity = 326
            },
            new TheaterHall
            {
                HallId = 2,
                TheaterName = "Kumari Cinemas",
                TheaterCity = "Kathmandu",
                HallCapacity = 240
            }
        );

        modelBuilder.Entity<Showtime>().HasData(
            new Showtime
            {
                ShowTimeId = 1,
                MovieId = 1,
                HallId = 1,
                ShowDate = new DateTime(2025, 12, 21),
                ShowTimeName = "Evening",
                TicketPrice = 390
            }
            // Removed Dune: Part Two showtime (2026-03-01 Matinee Kathmandu 450)
        );

        modelBuilder.Entity<Ticket>().HasData(
            new Ticket
            {
                TicketId = 1,
                UserId = 1,
                ShowtimeId = 1,
                BookingStatus = "Booked"
            }
        );
    }
}
