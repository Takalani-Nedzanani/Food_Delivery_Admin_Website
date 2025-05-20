

using Firebase.Database;
using Firebase.Database.Query;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryAdminWebsite.Pages
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

        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
        public List<MenuItemViewModel> MenuItems { get; set; } = new List<MenuItemViewModel>();
        public List<OrderViewModel> RecentConfirmedOrders { get; set; } = new List<OrderViewModel>();

        // New property to hold the daily aggregated data for graph
        public Dictionary<string, DailyOrderStats> OrdersPerDay { get; set; } = new();

        [BindProperty]
        public string SelectedOrderId { get; set; }

        [BindProperty]
        [Required]
        public string NewStatus { get; set; }

        [BindProperty]
        public string ItemToDeleteId { get; set; }

        public async Task OnGetAsync()
        {
            await LoadOrders();
            await LoadMenuItems();

            // Filter only confirmed orders within the past hour
            RecentConfirmedOrders = Orders
                .Where(o => o.Status == "collected" && o.Timestamp >= DateTime.UtcNow.AddHours(-1))
                .OrderByDescending(o => o.Timestamp)
                .ToList();

            // Group orders by day (yyyy-MM-dd) and calculate totals and confirmed counts
            OrdersPerDay = Orders
                .GroupBy(o => o.Timestamp.ToLocalTime().ToString("yyyy-MM-dd"))
                .OrderBy(g => g.Key)
                .ToDictionary(
                    g => g.Key,
                    g => new DailyOrderStats
                    {
                        Total = g.Count(),
                        Confirmed = g.Count(o => o.Status == "collected")
                    });
        }

        private async Task LoadOrders()
        {
            try
            {
                var orders = await _firebase
                    .Child("orders")
                    .OrderByKey()
                    .OnceAsync<Dictionary<string, object>>();

                Orders = orders.Select(order => new OrderViewModel
                {
                    Id = order.Key,
                    UserName = order.Object.ContainsKey("userName") ? order.Object["userName"].ToString() : "Unknown",
                    Status = order.Object.ContainsKey("status") ? order.Object["status"].ToString() : "pending",
                    Total = order.Object.ContainsKey("total") ? Convert.ToDouble(order.Object["total"]) : 0.0,
                    Timestamp = order.Object.ContainsKey("timestamp")
                        ? DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(order.Object["timestamp"])).UtcDateTime
                        : DateTime.UtcNow,
                    ItemCount = order.Object.ContainsKey("items") &&
                                order.Object["items"] is Dictionary<string, object> items ? items.Count : 0
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading orders");
                TempData["ErrorMessage"] = $"Error loading orders: {ex.Message}";
            }
        }

        private async Task LoadMenuItems()
        {
            try
            {
                var menuItems = await _firebase
                    .Child("menu")
                    .OnceAsync<Dictionary<string, object>>();

                MenuItems = menuItems.Select(item => new MenuItemViewModel
                {
                    Id = item.Key,
                    Name = item.Object["name"].ToString(),
                    Description = item.Object["description"].ToString(),
                    Price = Convert.ToDouble(item.Object["price"]),
                    Category = item.Object["category"].ToString()
                }).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading menu items");
                TempData["ErrorMessage"] = $"Error loading menu items: {ex.Message}";
            }
        }

        public async Task<IActionResult> OnPostUpdateStatus()
        {
            if (!ModelState.IsValid)
            {
                await LoadOrders();
                await LoadMenuItems();
                return Page();
            }

            try
            {
                await _firebase
                    .Child("orders")
                    .Child(SelectedOrderId)
                    .Child("status")
                    .PutAsync(NewStatus);

                TempData["SuccessMessage"] = "Order status updated successfully.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status");
                TempData["ErrorMessage"] = $"Error updating order status: {ex.Message}";
                await LoadOrders();
                await LoadMenuItems();
                return Page();
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
    }

    // View Models
    public class OrderViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public DateTime Timestamp { get; set; }
        public int ItemCount { get; set; }
        public double Total { get; set; }
        public string Status { get; set; }
    }

    public class MenuItemViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
    }

    // Class for daily aggregated stats for chart
    public class DailyOrderStats
    {
        public int Total { get; set; }
        public int Confirmed { get; set; }
    }
}



//using Firebase.Database;
//using Firebase.Database.Query;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.ComponentModel.DataAnnotations;

//namespace FoodDeliveryAdminWebsite.Pages
//{
//    public class IndexModel : PageModel
//    {
//        private readonly FirebaseClient _firebase;
//        private readonly ILogger<IndexModel> _logger;

//        public IndexModel(ILogger<IndexModel> logger)
//        {
//            _logger = logger;
//            _firebase = new FirebaseClient(
//                "https://cut-smartbanking-app-default-rtdb.firebaseio.com",
//                new FirebaseOptions
//                {
//                    AuthTokenAsyncFactory = () => Task.FromResult("7VavjcjNQ62DXnryR3OaZ4O1dJ5zoJfZB2E1zKi8")
//                });
//        }

//        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
//        public List<MenuItemViewModel> MenuItems { get; set; } = new List<MenuItemViewModel>();
//        public List<OrderViewModel> RecentConfirmedOrders { get; set; } = new List<OrderViewModel>();

//        [BindProperty]
//        public string SelectedOrderId { get; set; }

//        [BindProperty]
//        [Required]
//        public string NewStatus { get; set; }

//        [BindProperty]
//        public string ItemToDeleteId { get; set; }

//        public async Task OnGetAsync()
//        {
//            await LoadOrders();
//            await LoadMenuItems();

//            // Filter only confirmed orders within the past hour
//            RecentConfirmedOrders = Orders
//                .Where(o => o.Status == "collected" && o.Timestamp >= DateTime.UtcNow.AddHours(-1))
//                .OrderByDescending(o => o.Timestamp)
//                .ToList();
//        }

//        private async Task LoadOrders()
//        {
//            try
//            {
//                var orders = await _firebase
//                    .Child("orders")
//                    .OrderByKey()
//                    .OnceAsync<Dictionary<string, object>>();

//                Orders = orders.Select(order => new OrderViewModel
//                {
//                    Id = order.Key,
//                    UserName = order.Object.ContainsKey("userName") ? order.Object["userName"].ToString() : "Unknown",
//                    Status = order.Object.ContainsKey("status") ? order.Object["status"].ToString() : "pending",
//                    Total = order.Object.ContainsKey("total") ? Convert.ToDouble(order.Object["total"]) : 0.0,
//                    Timestamp = order.Object.ContainsKey("timestamp")
//                        ? DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(order.Object["timestamp"])).UtcDateTime
//                        : DateTime.UtcNow,
//                    ItemCount = order.Object.ContainsKey("items") &&
//                                order.Object["items"] is Dictionary<string, object> items ? items.Count : 0
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error loading orders");
//                TempData["ErrorMessage"] = $"Error loading orders: {ex.Message}";
//            }
//        }

//        private async Task LoadMenuItems()
//        {
//            try
//            {
//                var menuItems = await _firebase
//                    .Child("menu")
//                    .OnceAsync<Dictionary<string, object>>();

//                MenuItems = menuItems.Select(item => new MenuItemViewModel
//                {
//                    Id = item.Key,
//                    Name = item.Object["name"].ToString(),
//                    Description = item.Object["description"].ToString(),
//                    Price = Convert.ToDouble(item.Object["price"]),
//                    Category = item.Object["category"].ToString()
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error loading menu items");
//                TempData["ErrorMessage"] = $"Error loading menu items: {ex.Message}";
//            }
//        }

//        public async Task<IActionResult> OnPostUpdateStatus()
//        {
//            if (!ModelState.IsValid)
//            {
//                await LoadOrders();
//                await LoadMenuItems();
//                return Page();
//            }

//            try
//            {
//                await _firebase
//                    .Child("orders")
//                    .Child(SelectedOrderId)
//                    .Child("status")
//                    .PutAsync(NewStatus);

//                TempData["SuccessMessage"] = "Order status updated successfully.";
//                return RedirectToPage();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating order status");
//                TempData["ErrorMessage"] = $"Error updating order status: {ex.Message}";
//                await LoadOrders();
//                await LoadMenuItems();
//                return Page();
//            }
//        }

//        public async Task<IActionResult> OnPostDeleteMenuItem()
//        {
//            if (string.IsNullOrEmpty(ItemToDeleteId))
//            {
//                TempData["ErrorMessage"] = "Please select a menu item first.";
//                return RedirectToPage();
//            }

//            try
//            {
//                await _firebase
//                    .Child("menu")
//                    .Child(ItemToDeleteId)
//                    .DeleteAsync();

//                TempData["SuccessMessage"] = "Menu item deleted successfully.";
//                return RedirectToPage();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting menu item");
//                TempData["ErrorMessage"] = $"Error deleting menu item: {ex.Message}";
//                return RedirectToPage();
//            }
//        }
//    }

//    public class OrderViewModel
//    {
//        public string Id { get; set; }
//        public string UserName { get; set; }
//        public DateTime Timestamp { get; set; }
//        public int ItemCount { get; set; }
//        public double Total { get; set; }
//        public string Status { get; set; }
//    }

//    public class MenuItemViewModel
//    {
//        public string Id { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public double Price { get; set; }
//        public string Category { get; set; }
//    }
//}








//using Firebase.Database;
//using Firebase.Database.Query;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using System.ComponentModel.DataAnnotations;

//namespace FoodDeliveryAdminWebsite.Pages
//{
//    public class IndexModel : PageModel
//    {
//        private readonly FirebaseClient _firebase;
//        private readonly ILogger<IndexModel> _logger;

//        public IndexModel(ILogger<IndexModel> logger)
//        {
//            _logger = logger;
//            _firebase = new FirebaseClient(
//                "https://cut-smartbanking-app-default-rtdb.firebaseio.com",
//                new FirebaseOptions
//                {
//                    AuthTokenAsyncFactory = () => Task.FromResult("7VavjcjNQ62DXnryR3OaZ4O1dJ5zoJfZB2E1zKi8")
//                });
//        }

//        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
//        public List<MenuItemViewModel> MenuItems { get; set; } = new List<MenuItemViewModel>();

//        [BindProperty]
//        public string SelectedOrderId { get; set; }

//        [BindProperty]
//        [Required]
//        public string NewStatus { get; set; }

//        [BindProperty]
//        public string ItemToDeleteId { get; set; }

//        public async Task OnGetAsync()
//        {
//            await LoadOrders();
//            await LoadMenuItems();
//        }

//        private async Task LoadOrders()
//        {
//            try
//            {
//                var orders = await _firebase
//                    .Child("orders")
//                    .OrderByKey()
//                    .OnceAsync<Dictionary<string, object>>();

//                Orders = orders.Select(order => new OrderViewModel
//                {
//                    Id = order.Key,
//                    UserName = order.Object.ContainsKey("userName") ? order.Object["userName"].ToString() : "Unknown",
//                    Status = order.Object["status"].ToString(),
//                    Total = Convert.ToDouble(order.Object["total"]),
//                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(
//                        Convert.ToInt64(order.Object["timestamp"])).DateTime,
//                    ItemCount = order.Object.ContainsKey("items") &&
//                               order.Object["items"] is Dictionary<string, object> items ? items.Count : 0
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error loading orders");
//                TempData["ErrorMessage"] = $"Error loading orders: {ex.Message}";
//            }
//        }

//        private async Task LoadMenuItems()
//        {
//            try
//            {
//                var menuItems = await _firebase
//                    .Child("menu")
//                    .OnceAsync<Dictionary<string, object>>();

//                MenuItems = menuItems.Select(item => new MenuItemViewModel
//                {
//                    Id = item.Key,
//                    Name = item.Object["name"].ToString(),
//                    Description = item.Object["description"].ToString(),
//                    Price = Convert.ToDouble(item.Object["price"]),
//                    Category = item.Object["category"].ToString()
//                }).ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error loading menu items");
//                TempData["ErrorMessage"] = $"Error loading menu items: {ex.Message}";
//            }
//        }

//        public async Task<IActionResult> OnPostUpdateStatus()
//        {
//            if (!ModelState.IsValid)
//            {
//                await LoadOrders();
//                await LoadMenuItems();
//                return Page();
//            }

//            try
//            {
//                await _firebase
//                    .Child("orders")
//                    .Child(SelectedOrderId)
//                    .Child("status")
//                    .PutAsync(NewStatus);

//                TempData["SuccessMessage"] = "Order status updated successfully.";
//                return RedirectToPage();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating order status");
//                TempData["ErrorMessage"] = $"Error updating order status: {ex.Message}";
//                await LoadOrders();
//                await LoadMenuItems();
//                return Page();
//            }
//        }

//        public async Task<IActionResult> OnPostDeleteMenuItem()
//        {
//            if (string.IsNullOrEmpty(ItemToDeleteId))
//            {
//                TempData["ErrorMessage"] = "Please select a menu item first.";
//                return RedirectToPage();
//            }

//            try
//            {
//                await _firebase
//                    .Child("menu")
//                    .Child(ItemToDeleteId)
//                    .DeleteAsync();

//                TempData["SuccessMessage"] = "Menu item deleted successfully.";
//                return RedirectToPage();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error deleting menu item");
//                TempData["ErrorMessage"] = $"Error deleting menu item: {ex.Message}";
//                return RedirectToPage();
//            }
//        }
//    }

//    public class OrderViewModel
//    {
//        public string Id { get; set; }
//        public string UserName { get; set; }
//        public DateTime Timestamp { get; set; }
//        public int ItemCount { get; set; }
//        public double Total { get; set; }
//        public string Status { get; set; }
//    }

//    public class MenuItemViewModel
//    {
//        public string Id { get; set; }
//        public string Name { get; set; }
//        public string Description { get; set; }
//        public double Price { get; set; }
//        public string Category { get; set; }
//    }
//}
