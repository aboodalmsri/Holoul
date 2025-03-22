using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Holoul.Models;
using Microsoft.EntityFrameworkCore;
using Holoul.Areas.Identity.Data;

namespace Holoul.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly EDbContext _context;

    public HomeController(ILogger<HomeController> logger, EDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }
    public IActionResult contact()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult contact(Feedback feedback)
    {
        if (ModelState.IsValid)
        {
            try
            {
                _context.FeedBacks.Add(feedback);
                _context.SaveChanges();
                ViewBag.SuccessMessage = "Thank you for your feedback!";
                ModelState.Clear();
                return View(new Feedback());
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                ViewBag.Errormsg = "Something went wrong!";
                return View(feedback);
            }
        }
        return View(feedback);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
