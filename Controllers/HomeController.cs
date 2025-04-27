using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Dashboard()
    {
        // Mock data
        ViewBag.TotalOrders = 342;
        ViewBag.TotalRevenue = 12845.75m;
        ViewBag.ActiveRestaurants = 24;
        ViewBag.PendingOrders = 12;

        return View();
    }
}