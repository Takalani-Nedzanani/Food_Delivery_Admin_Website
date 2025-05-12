using FoodDeliveryAdminWebsite.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Firebase.Database;
using Firebase.Database.Query;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryAdminWebsite.Pages.MenuItems
{
    public class CreateModel : PageModel
    {
        private readonly FirebaseClient _firebase;

        public CreateModel()
        {
            _firebase = new FirebaseClient(
                 "https://cut-smartbanking-app-default-rtdb.firebaseio.com",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("7VavjcjNQ62DXnryR3OaZ4O1dJ5zoJfZB2E1zKi8")
                });
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        public List<string> Categories { get; } = new List<string>
        {
            "Appetizers", "Main Courses", "Desserts", "Drinks", "Specials"
        };

        public class InputModel
        {
            [Required]
            [StringLength(100)]
            public string Name { get; set; }

            [StringLength(500)]
            public string Description { get; set; }

            [Required]
            [Range(0.01, 1000)]
            public double Price { get; set; }

            [Required]
            public string Category { get; set; }

            [Url]
            public string ImageUrl { get; set; }
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var newItem = new Dictionary<string, object>
                {
                    { "name", Input.Name },
                    { "description", Input.Description },
                    { "price", Input.Price },
                    { "category", Input.Category },
                    { "imageUrl", Input.ImageUrl }
                };

                await _firebase
                    .Child("menu")
                    .PostAsync(newItem);

                return RedirectToPage("../Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error adding menu item: {ex.Message}");
                return Page();
            }
        }
    }
}
