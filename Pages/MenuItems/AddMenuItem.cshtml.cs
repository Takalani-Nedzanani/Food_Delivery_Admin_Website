using FoodDeliveryAdminWebsite.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryAdminWebsite.Pages.MenuItems
{
    public class AddMenuItemModel : PageModel
    {
        private readonly ILogger<AddMenuItemModel> _logger;

        public AddMenuItemModel(ILogger<AddMenuItemModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public MenuItemInputModel MenuItem { get; set; } = new MenuItemInputModel();

        public List<string> Categories { get; } = new List<string>
        {
            "Appetizers",
            "Main Courses",
            "Desserts",
            "Drinks",
            "Specials"
        };

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Here you would save to Firebase or your database
            _logger.LogInformation($"Adding new menu item: {MenuItem.Name}");

            return RedirectToPage("./Index");
        }
        //public async Task<IActionResult> OnPost([FromServices] FirebaseServices firebaseService)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Page();
        //    }

        //    try
        //    {
        //        await firebaseService.GetClient()
        //            .Collection("menu")
        //            .AddAsync(new
        //            {
        //                Name = MenuItem.Name,
        //                Description = MenuItem.Description,
        //                Price = MenuItem.Price,
        //                Category = MenuItem.Category,
        //                ImageUrl = MenuItem.ImageUrl
        //            });

        //        return RedirectToPage("./Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        ModelState.AddModelError(string.Empty, $"Error saving menu item: {ex.Message}");
        //        return Page();
        //    }
        //}
    }

    public class MenuItemInputModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000, ErrorMessage = "Price must be between $0.01 and $1000")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }

        [Url(ErrorMessage = "Please enter a valid URL")]
        public string ImageUrl { get; set; }
    }
}
