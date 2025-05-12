using FoodDeliveryAdminWebsite.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Firebase.Database;
using Firebase.Database.Query;

namespace FoodDeliveryAdminWebsite.Pages.MenuItems
{
    public class DeleteModel : PageModel
    {
        private readonly FirebaseClient _firebase;

        public DeleteModel()
        {
            _firebase = new FirebaseClient(
                "https://cut-smartbanking-app-default-rtdb.firebaseio.com",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("7VavjcjNQ62DXnryR3OaZ4O1dJ5zoJfZB2E1zKi8")
                });
        }

        public MenuItemViewModel MenuItem { get; set; } = new MenuItemViewModel();

        public class MenuItemViewModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
            public string Category { get; set; }
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

                MenuItem = new MenuItemViewModel
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

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            try
            {
                await _firebase
                    .Child("menu")
                    .Child(id)
                    .DeleteAsync();

                return RedirectToPage("../Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error deleting menu item: {ex.Message}");
                return Page();
            }
        }
    }
}
