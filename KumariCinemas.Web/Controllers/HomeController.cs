using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KumariCinemas.Web.Data;
using KumariCinemas.Web.Models;

namespace KumariCinemas.Web.Controllers;

public class HomeController : Controller
{
    private readonly CinemaDbContext _context;

    public HomeController(CinemaDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var booked = await _context.Tickets
            .CountAsync(t => t.BookingStatus == "Booked");

        var cancelled = await _context.Tickets
            .CountAsync(t => t.BookingStatus == "Cancelled");

        var pending = await _context.Tickets
            .CountAsync(t => t.BookingStatus == "Pending");

        ViewBag.Booked = booked;
        ViewBag.Cancelled = cancelled;
        ViewBag.Pending = pending;
        

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
        });
    }
}