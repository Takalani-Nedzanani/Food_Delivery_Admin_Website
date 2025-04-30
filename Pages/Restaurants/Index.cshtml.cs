using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDeliveryAdminWebsite.views.Restaurants
{
    public class Restaurant : PageModel
    {
        private readonly ILogger<Restaurant> _logger;

        public Restaurant(ILogger<Restaurant> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
        }
    }
}
