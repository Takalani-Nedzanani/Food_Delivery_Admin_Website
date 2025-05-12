using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Firebase.Database;
using Firebase.Database.Query;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryAdminWebsite.Pages.Orders
{
    public class IndexModel : PageModel
    {
        private readonly FirebaseClient _firebase;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            _firebase = new FirebaseClient(
                "https://cut-smartbanking-app-default-rtdb.firebaseio.com",
                new FirebaseOptions
                {
                    AuthTokenAsyncFactory = () => Task.FromResult("7VavjcjNQ62DXnryR3OaZ4O1dJ5zoJfZB2E1zKi8")
                });
        }

        public List<OrderViewModel> Orders { get; set; } = new();

        [BindProperty]
        public string SelectedOrderId { get; set; }

        [BindProperty]
        [Required]
        public string NewStatus { get; set; }

        public async Task OnGetAsync()
        {
            await LoadOrders();
        }

        private async Task LoadOrders()
        {
            try
            {
                var firebaseOrders = await _firebase
                    .Child("orders")
                    .OrderByKey()
                    .OnceAsync<Dictionary<string, object>>();

                Orders = firebaseOrders.Select(order => new OrderViewModel
                {
                    Id = order.Key,
                    UserName = order.Object.ContainsKey("userName") ? order.Object["userName"]?.ToString() ?? "Unknown" : "Unknown",
                    Status = order.Object.ContainsKey("status") ? order.Object["status"]?.ToString() ?? "pending" : "pending",
                    Total = order.Object.ContainsKey("total") ? Convert.ToDouble(order.Object["total"]) : 0.0,
                    Timestamp = order.Object.ContainsKey("timestamp")
                        ? DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(order.Object["timestamp"])).DateTime
                        : DateTime.MinValue,
                    ItemCount = order.Object.ContainsKey("items") && order.Object["items"] is Dictionary<string, object> items
                        ? items.Count
                        : 0
                })
                .OrderByDescending(o => o.Timestamp)
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading orders from Firebase");
                TempData["ErrorMessage"] = $"Error loading orders: {ex.Message}";
            }
        }

        public async Task<IActionResult> OnPostUpdateStatus()
        {
            if (!ModelState.IsValid || string.IsNullOrWhiteSpace(SelectedOrderId) || string.IsNullOrWhiteSpace(NewStatus))
            {
                TempData["ErrorMessage"] = "Please select a valid status.";
                await LoadOrders();
                return Page();
            }

            try
            {
                await _firebase
                    .Child("orders")
                    .Child(SelectedOrderId)
                    .Child("status")
                    .PutAsync(NewStatus);

                TempData["SuccessMessage"] = $"Order status updated to {NewStatus}.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status in Firebase");
                TempData["ErrorMessage"] = $"Error updating order status: {ex.Message}";
                await LoadOrders();
                return Page();
            }
        }

        public class OrderViewModel
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public DateTime Timestamp { get; set; }
            public int ItemCount { get; set; }
            public double Total { get; set; }
            public string Status { get; set; }
        }
    }
}
