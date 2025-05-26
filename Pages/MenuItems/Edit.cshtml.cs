


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
        private readonly IWebHostEnvironment _env;

        public EditModel(IWebHostEnvironment env)
        {
            _firebase = new FirebaseClient(
                "https://cut-smartbanking-app-default-rtdb.firebaseio.com",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("7VavjcjNQ62DXnryR3OaZ4O1dJ5zoJfZB2E1zKi8")
                });
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();

        [BindProperty]
        public IFormFile? ImageFile { get; set; }

        public string? CurrentImageUrl { get; set; }

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

            public string? ImageUrl { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                return NotFound();

            var menuItem = await _firebase
                .Child("menu")
                .Child(id)
                .OnceSingleAsync<Dictionary<string, object>>();

            if (menuItem == null)
                return NotFound();

            string? rawImagePath = menuItem.ContainsKey("imageUrl") ? menuItem["imageUrl"]?.ToString() : "";

            // Convert storage path to full Firebase Storage URL if it exists
            if (!string.IsNullOrEmpty(rawImagePath))
            {
                CurrentImageUrl = $"https://firebasestorage.googleapis.com/v0/b/cut-smartbanking-app.appspot.com/o/{Uri.EscapeDataString(rawImagePath)}?alt=media";
            }

            Input = new InputModel
            {
                Id = id,
                Name = menuItem["name"]?.ToString(),
                Description = menuItem["description"]?.ToString(),
                Price = Convert.ToDouble(menuItem["price"]),
                Category = menuItem["category"]?.ToString(),
                ImageUrl = rawImagePath
            };

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
                return Page();

            string imagePath = Input.ImageUrl ?? "";

            if (ImageFile != null)
            {
                // Upload to Firebase Storage (ideally should be done using a Firebase SDK or external API)
                string fileName = $"menu_items/{Guid.NewGuid()}{Path.GetExtension(ImageFile.FileName)}";
                string localPath = Path.Combine(_env.WebRootPath, "temp", fileName);
                Directory.CreateDirectory(Path.GetDirectoryName(localPath)!);

                using (var stream = new FileStream(localPath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

                // Simulate Firebase Storage path saving (in real scenario, upload the file to Firebase Storage)
                imagePath = fileName;
            }

            var updatedItem = new Dictionary<string, object>
            {
                { "name", Input.Name },
                { "description", Input.Description },
                { "price", Input.Price },
                { "category", Input.Category },
                { "imageUrl", imagePath }
            };

            await _firebase
                .Child("menu")
                .Child(Input.Id)
                .PutAsync(updatedItem);

            return RedirectToPage("../Index");
        }
    }
}

