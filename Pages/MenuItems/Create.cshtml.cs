
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Firebase.Database;
using Firebase.Database.Query;
using Google.Cloud.Storage.V1;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Google.Apis.Auth.OAuth2;
using FirebaseAdmin;

namespace FoodDeliveryAdminWebsite.Pages.MenuItems
{
    public class CreateModel : PageModel
    {
        private readonly FirebaseClient _firebase;
        private readonly StorageClient _storageClient;
        private readonly string _bucketName;
        private readonly string _adminToken;

        public CreateModel(IConfiguration configuration)
        {
            _adminToken = configuration["Firebase:AdminToken"];
            _bucketName = "cut-smartbanking-app.appspot.com";

            // Step 1: Initialize FirebaseApp only if not already initialized
            if (FirebaseApp.DefaultInstance == null)
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromFile("C:\\Users\\nedza\\OneDrive\\Desktop\\cut-smartbanking-app-firebase-adminsdk-888hh-d250e34f4c.json"),
                    ProjectId = "cut-smartbanking-app",
                });
            }

            // Step 2: Create the Firebase Realtime Database client
            _firebase = new FirebaseClient(
                "https://cut-smartbanking-app-default-rtdb.firebaseio.com/",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult(_adminToken)
                });

            // Step 3: Create Google Cloud Storage client
            var credential = GoogleCredential.FromFile("C:\\Users\\nedza\\OneDrive\\Desktop\\cut-smartbanking-app-firebase-adminsdk-888hh-d250e34f4c.json");
            _storageClient = StorageClient.Create(credential);
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

            [Required(ErrorMessage = "Please upload an image file.")]
            public IFormFile ImageFile { get; set; }
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
                // Upload image to Firebase Storage
                string imagePath = await UploadImageToFirebase(Input.ImageFile);

                // Save item to Firebase Realtime Database
                var newItem = new Dictionary<string, object>
                {
                    { "name", Input.Name },
                    { "description", Input.Description },
                    { "price", Input.Price },
                    { "category", Input.Category },
                    { "imageUrl", imagePath },  // Just the path, not full URL
                    { "likes", 0 },
                    { "createdAt", DateTime.UtcNow.ToString("o") }
                };

                await _firebase.Child("menu").PostAsync(newItem);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return Page();
            }
        }

        private async Task<string> UploadImageToFirebase(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                throw new ArgumentException("Image file is empty");
            }

            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var fileExtension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!validExtensions.Contains(fileExtension))
            {
                throw new ArgumentException("Only JPG, PNG, and GIF images are allowed");
            }

            var fileName = $"menu_items/{Guid.NewGuid()}{fileExtension}";

            using (var memoryStream = new MemoryStream())
            {
                await imageFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                await _storageClient.UploadObjectAsync(
                    bucket: _bucketName,
                    objectName: fileName,
                    contentType: imageFile.ContentType,
                    source: memoryStream
                );

                // Return only the path so Flutter can fetch the full URL using Firebase SDK
                return fileName;
            }
        }
    }
}
