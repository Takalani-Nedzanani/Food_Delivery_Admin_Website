using Microsoft.AspNetCore.Mvc;

public class RestaurantsController : Controller
{
    private static List<Restaurant> _restaurants = new List<Restaurant>
    {
        new Restaurant { Id = 1, Name = "Burger Palace", Address = "123 Main St", Phone = "555-1234", IsActive = true },
        new Restaurant { Id = 2, Name = "Pizza Heaven", Address = "456 Oak Ave", Phone = "555-5678", IsActive = true }
    };

    public IActionResult Index()
    {
        return View(_restaurants); // Pass the list to the view
    }
}