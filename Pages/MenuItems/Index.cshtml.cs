
using FoodDeliveryAdminWebsite.services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;

namespace FoodDeliveryAdminWebsite.Pages.MenuItems
{
    public class IndexModel : PageModel
    {
        private readonly FirebaseClient _firebase;
        private readonly ILogger<IndexModel> _logger;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName = "cut-smartbanking-app.appspot.com";

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            // Initialize FirebaseClient
            _firebase = new FirebaseClient(
                "https://cut-smartbanking-app-default-rtdb.firebaseio.com",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("7VavjcjNQ62DXnryR3OaZ4O1dJ5zoJfZB2E1zKi8")
                });

            // Initialize Google Cloud Storage client
            var credential = GoogleCredential.FromFile("C:\\Users\\nedza\\OneDrive\\Desktop\\cut-smartbanking-app-firebase-adminsdk-888hh-d250e34f4c.json");
            _storageClient = StorageClient.Create(credential);
        }

        public List<MenuItemViewModel> MenuItems { get; set; } = new List<MenuItemViewModel>();

        [BindProperty]
        public string ItemToDeleteId { get; set; }

        public async Task OnGetAsync()
        {
            await LoadMenuItems();
        }

        private async Task LoadMenuItems()
        {
            try
            {
                var menuItems = await _firebase
                    .Child("menu")
                    .OnceAsync<Dictionary<string, object>>();

                MenuItems = menuItems.Select(item =>
                {
                    var imagePath = item.Object.ContainsKey("imageUrl") ? item.Object["imageUrl"].ToString() : "";
                    var imageUrl = !string.IsNullOrEmpty(imagePath)
                        ? $"https://firebasestorage.googleapis.com/v0/b/cut-smartbanking-app.appspot.com/o/{Uri.EscapeDataString(imagePath)}?alt=media"
                        : "";

                    return new MenuItemViewModel
                    {
                        Id = item.Key,
                        Name = item.Object["name"].ToString(),
                        Description = item.Object["description"].ToString(),
                        Price = Convert.ToDouble(item.Object["price"]),
                        Category = item.Object["category"].ToString(),
                        ImageUrl = imageUrl
                    };
                })
                .OrderBy(i => i.Category)
                .ThenBy(i => i.Name)
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading menu items");
                TempData["ErrorMessage"] = $"Error loading menu items: {ex.Message}";
            }
        }

        public async Task<IActionResult> OnPostDeleteMenuItem()
        {
            if (string.IsNullOrEmpty(ItemToDeleteId))
            {
                TempData["ErrorMessage"] = "Please select a menu item first.";
                return RedirectToPage();
            }

            try
            {
                await _firebase
                    .Child("menu")
                    .Child(ItemToDeleteId)
                    .DeleteAsync();

                TempData["SuccessMessage"] = "Menu item deleted successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting menu item");
                TempData["ErrorMessage"] = $"Error deleting menu item: {ex.Message}";
                return RedirectToPage();
            }
        }

        public class MenuItemViewModel
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }
            public string Category { get; set; }
            public string ImageUrl { get; set; }
        }
    }
}
