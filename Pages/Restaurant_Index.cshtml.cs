using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FoodDeliveryAdminWebsite.Pages
{
    public class Restaurant_IndexModel : PageModel
    {
        private readonly ILogger<Restaurant_IndexModel> _logger;

        public Restaurant_IndexModel(ILogger<Restaurant_IndexModel> logger)
        {
            _logger = logger;
        }
        public void OnGet()
        {
        }
    }
}
