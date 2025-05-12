using FoodDeliveryAdminWebsite.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Firebase.Database;
using Firebase.Database.Query;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryAdminWebsite.Pages.MenuItems
{
    public class EditModel : PageModel
    {
        private readonly FirebaseClient _firebase;

        public EditModel()
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
            public string Id { get; set; }

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

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                var menuItem = await _firebase
                    .Child("menu")
                    .Child(id)
                    .OnceSingleAsync<Dictionary<string, object>>();

                if (menuItem == null)
                {
                    return NotFound();
                }

                Input = new InputModel
                {
                    Id = id,
                    Name = menuItem["name"].ToString(),
                    Description = menuItem["description"].ToString(),
                    Price = Convert.ToDouble(menuItem["price"]),
                    Category = menuItem["category"].ToString(),
                    ImageUrl = menuItem.ContainsKey("imageUrl") ? menuItem["imageUrl"].ToString() : ""
                };

                return Page();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error loading menu item: {ex.Message}");
                return Page();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var updatedItem = new Dictionary<string, object>
                {
                    { "name", Input.Name },
                    { "description", Input.Description },
                    { "price", Input.Price },
                    { "category", Input.Category },
                    { "imageUrl", Input.ImageUrl }
                };

                await _firebase
                    .Child("menu")
                    .Child(Input.Id)
                    .PutAsync(updatedItem);

                return RedirectToPage("../Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error updating menu item: {ex.Message}");
                return Page();
            }
        }
    }
}
