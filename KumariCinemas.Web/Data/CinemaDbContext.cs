
using Microsoft.EntityFrameworkCore;
using KumariCinemas.Web.Models;
using System;

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

// Username and Address Data for AppUser
        modelBuilder.Entity<AppUser>().HasData(
            new AppUser { UserId = 1, Username = "Pratibha Gurung", Address = "Pokhara" },
            new AppUser { UserId = 2, Username = "Aaseaan Siwakoti", Address = "Lalitpur" },
            new AppUser { UserId = 3, Username = "Tamin Bin Al Thani", Address = "Kathmandu" },
            new AppUser { UserId = 4, Username = "Rachael Raphael", Address = "Pokhara" },
            new AppUser { UserId = 5, Username = "Evan Smith", Address = "Lalitpur" },
            new AppUser { UserId = 6, Username = "Andrew Hamilton", Address = "Kathmandu" },
            new AppUser { UserId = 7, Username = "Samantha Lee", Address = "Pokhara" }
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
                MovieId = 2,
                Title = "Michael",
                Duration = "127 mins",
                Language = "English",
                Genre = "Biographical Musical Drama",
                ReleaseDate = new DateTime(2026, 4, 24)
            },
            new Movie
            {
                MovieId = 3,
                Title = "Sully: Miracle On The Hudson",
                Duration = "1hr 35mins",
                Language = "English",
                Genre = "Drama-History",
                ReleaseDate = new DateTime(2016, 9, 9)
            },
            new Movie
            {
                MovieId = 4,
                Title = "The Odyssey",
                Duration = "2hrs 55mins",
                Language = "English",
                Genre = "Adventure",
                ReleaseDate = new DateTime(2026, 7, 23)
            }
        );

// Data for TheaterHalls Locations and Capacities
        modelBuilder.Entity<TheaterHall>().HasData(
            new TheaterHall
            {
                HallId = 1,
                TheaterName = "Kumari Cinemas",
                TheaterCity = "Kathmandu",
                HallCapacity = 500
            },
            new TheaterHall
            {
                HallId = 2,
                TheaterName = "Kumari Cinemas",
                TheaterCity = "Pokhara",
                HallCapacity = 340
            },
            new TheaterHall
            {
                HallId = 3,
                TheaterName = "Kumari Cinemas",
                TheaterCity = "Lalitpur",
                HallCapacity = 165
            }
        );


// Data for Showtimes with Movie, Hall, Date, Time, and Ticket Price
        modelBuilder.Entity<Showtime>().HasData(
            new Showtime
            {
                ShowTimeId = 1, //Date
                MovieId = 1, // Movie: Avatar: Fire and Ash
                HallId = 1, // Hall: Kumari Cinemas, Kathmandu
                ShowDate = new DateTime(2026, 04, 25),
                ShowTimeName = "06:30 PM",
                TicketPrice = 390
            },
            new Showtime
            {
                ShowTimeId = 2, //Date
                MovieId = 1, // Movie: Avatar: Fire and Ash
                HallId = 2, // Hall: Kumari Cinemas, Pokhara
                ShowDate = new DateTime(2026, 04, 25),
                ShowTimeName = "08:00 PM",
                TicketPrice = 390
            },
            new Showtime
            {
                ShowTimeId = 3, //Date
                MovieId = 1, // Movie: Avatar: Fire and Ash
                HallId = 3, // Hall: Kumari Cinemas, Lalitpur
                ShowDate = new DateTime(2026, 04, 26),
                ShowTimeName = "10:30 PM",
                TicketPrice = 390
            },
            new Showtime
            {
                ShowTimeId = 4, //Date
                MovieId = 2, // Movie: Michael
                HallId = 1, // Hall: Kumari Cinemas, Kathmandu
                ShowDate = new DateTime(2026, 04, 26),
                ShowTimeName = "09:30 AM",
                TicketPrice = 500
            },
            new Showtime
            {
                ShowTimeId = 5, //Date
                MovieId = 2, // Movie: Michael
                HallId = 2, // Hall: Kumari Cinemas, Pokhara
                ShowDate = new DateTime(2026, 04, 26),
                ShowTimeName = "11:45 AM",
                TicketPrice = 500
            },
            new Showtime
            {
                ShowTimeId = 6, //Date
                MovieId = 2, // Movie: Michael
                HallId = 3, // Hall: Kumari Cinemas, Lalitpur
                ShowDate = new DateTime(2026, 04, 26),
                ShowTimeName = "04:30 PM",
                TicketPrice = 500
            },
            new Showtime
            {
                ShowTimeId = 7, //Date
                MovieId = 3, // Movie: Sully: Miracle On The Hudson
                HallId = 1, // Hall: Kumari Cinemas, Kathmandu
                ShowDate = new DateTime(2026, 04, 27),
                ShowTimeName = "02:00 PM",
                TicketPrice = 450
            },
            new Showtime
            {
                ShowTimeId = 9, //Date
                MovieId = 3, // Movie: Sully: Miracle On The Hudson
                HallId = 3, // Hall: Kumari Cinemas, Lalitpur
                ShowDate = new DateTime(2026, 04, 27),
                ShowTimeName = "08:00 PM",
                TicketPrice = 450
            },
            new Showtime
            {
                ShowTimeId = 10, //Date
                MovieId = 4, // Movie: The Odyssey
                HallId = 1, // Hall: Kumari Cinemas, Kathmandu
                ShowDate = new DateTime(2026, 04, 28),
                ShowTimeName = "01:00 PM",
                TicketPrice = 400
            },
            new Showtime
            {
                ShowTimeId = 11, //Date
                MovieId = 4, // Movie: The Odyssey
                HallId = 2, // Hall: Kumari Cinemas, Pokhara
                ShowDate = new DateTime(2026, 04, 28),
                ShowTimeName = "03:30 PM",
                TicketPrice = 400
            }
        );

// Data for Tickets with User, Showtime, and Booking Status
        modelBuilder.Entity<Ticket>().HasData(
            new Ticket
            {
                TicketId = 1,
                UserId = 1,
                ShowtimeId = 1,
                BookingStatus = "Booked"
            },
            new Ticket
            {
                TicketId = 2,
                UserId = 2,
                ShowtimeId = 2,
                BookingStatus = "Booked"
            },
            new Ticket
            {
                TicketId = 3,
                UserId = 3,
                ShowtimeId = 1,
                BookingStatus = "Cancelled"
            },
            new Ticket
            {
                TicketId = 4,
                UserId = 4,
                ShowtimeId = 1,
                BookingStatus = "Pending"
            },
            new Ticket
            {
                TicketId = 5,
                UserId = 5,
                ShowtimeId = 2,
                BookingStatus = "Booked"
            },
            new Ticket
            {
                TicketId = 6,
                UserId = 6,
                ShowtimeId = 3,
                BookingStatus = "Pending"
            },
            new Ticket
            {
                TicketId = 7,
                UserId = 7,
                ShowtimeId = 4,
                BookingStatus = "Booked"
            }
        );
    }
}
