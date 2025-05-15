using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Firebase.Database;
using Firebase.Database.Query;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

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
                var orders = await _firebase
                    .Child("orders")
                    .OnceAsync<object>();

                Orders = orders.Select(order =>
                {
                    var orderData = JObject.FromObject(order.Object);
                    var items = new List<OrderItemViewModel>();

                    if (orderData["items"] is JObject itemObject)
                    {
                        foreach (var itemProp in itemObject.Properties())
                        {
                            var item = itemProp.Value;
                            string name = item["name"]?.ToString() ?? "Unknown";
                            int quantity = 1;

                            if (item["quantity"] != null && int.TryParse(item["quantity"]?.ToString(), out int parsedQty))
                            {
                                quantity = parsedQty;
                            }

                            items.Add(new OrderItemViewModel
                            {
                                Name = name,
                                Quantity = quantity
                            });
                        }
                    }

                    return new OrderViewModel
                    {
                        Id = order.Key,
                        UserEmail = orderData["userEmail"]?.ToString() ?? "Unknown", // ✅ read userEmail
                        Status = orderData["status"]?.ToString() ?? "pending",
                        Total = double.TryParse(orderData["total"]?.ToString(), out double total) ? total : 0.0,
                        Timestamp = orderData["timestamp"] != null
                            ? DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(orderData["timestamp"])).DateTime
                            : DateTime.Now,
                        ItemCount = items.Sum(i => i.Quantity),
                        ItemDetails = items
                    };
                })
                .OrderByDescending(o => o.Timestamp)
                .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading orders");
                TempData["ErrorMessage"] = $"Error loading orders: {ex.Message}";
            }
        }

        public async Task<IActionResult> OnPostUpdateStatus()
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(SelectedOrderId) || string.IsNullOrEmpty(NewStatus))
            {
                TempData["ErrorMessage"] = "Invalid status update.";
                await LoadOrders();
                return Page();
            }

            try
            {
                await _firebase
                    .Child("orders")
                    .Child(SelectedOrderId)
                    .Child("status")
                    .PutAsync(JsonConvert.SerializeObject(NewStatus.ToLower()));

                TempData["SuccessMessage"] = $"Order status updated to '{NewStatus}'.";
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
            public string UserEmail { get; set; } // ✅ renamed from UserName
            public DateTime Timestamp { get; set; }
            public int ItemCount { get; set; }
            public double Total { get; set; }
            public string Status { get; set; }
            public List<OrderItemViewModel> ItemDetails { get; set; } = new();
        }

        public class OrderItemViewModel
        {
            public string Name { get; set; }
            public int Quantity { get; set; }
        }
    }
}


//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Firebase.Database;
//using Firebase.Database.Query;
//using System.ComponentModel.DataAnnotations;
//using Newtonsoft.Json.Linq;
//using Newtonsoft.Json;

//namespace FoodDeliveryAdminWebsite.Pages.Orders
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

//        public List<OrderViewModel> Orders { get; set; } = new();

//        [BindProperty]
//        public string SelectedOrderId { get; set; }

//        [BindProperty]
//        [Required]
//        public string NewStatus { get; set; }

//        public async Task OnGetAsync()
//        {
//            await LoadOrders();
//        }

//        private async Task LoadOrders()
//        {
//            try
//            {
//                var orders = await _firebase
//                    .Child("orders")
//                    .OnceAsync<object>();

//                Orders = orders.Select(order =>
//                {
//                    var orderData = JObject.FromObject(order.Object);
//                    var items = new List<OrderItemViewModel>();

//                    if (orderData["items"] is JObject itemObject)
//                    {
//                        foreach (var itemProp in itemObject.Properties())
//                        {
//                            var item = itemProp.Value;
//                            string name = item["name"]?.ToString() ?? "Unknown";
//                            int quantity = 1;

//                            if (item["quantity"] != null && int.TryParse(item["quantity"]?.ToString(), out int parsedQty))
//                            {
//                                quantity = parsedQty;
//                            }

//                            items.Add(new OrderItemViewModel
//                            {
//                                Name = name,
//                                Quantity = quantity
//                            });
//                        }
//                    }

//                    return new OrderViewModel
//                    {
//                        Id = order.Key,
//                        UserName = orderData["userName"]?.ToString() ?? "Unknown",
//                        Status = orderData["status"]?.ToString() ?? "pending",
//                        Total = double.TryParse(orderData["total"]?.ToString(), out double total) ? total : 0.0,
//                        Timestamp = orderData["timestamp"] != null
//                            ? DateTimeOffset.FromUnixTimeMilliseconds(Convert.ToInt64(orderData["timestamp"])).DateTime
//                            : DateTime.Now,
//                        ItemCount = items.Sum(i => i.Quantity),
//                        ItemDetails = items
//                    };
//                })
//                .OrderByDescending(o => o.Timestamp)
//                .ToList();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error loading orders");
//                TempData["ErrorMessage"] = $"Error loading orders: {ex.Message}";
//            }
//        }

//        public async Task<IActionResult> OnPostUpdateStatus()
//        {
//            if (!ModelState.IsValid || string.IsNullOrEmpty(SelectedOrderId) || string.IsNullOrEmpty(NewStatus))
//            {
//                TempData["ErrorMessage"] = "Invalid status update.";
//                await LoadOrders();
//                return Page();
//            }

//            try
//            {
//                await _firebase
//                    .Child("orders")
//                    .Child(SelectedOrderId)
//                    .Child("status")
//                    .PutAsync(JsonConvert.SerializeObject(NewStatus.ToLower())); 

//                TempData["SuccessMessage"] = $"Order status updated to '{NewStatus}'.";
//                return RedirectToPage();
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error updating order status in Firebase");
//                TempData["ErrorMessage"] = $"Error updating order status: {ex.Message}";
//                await LoadOrders();
//                return Page();
//            }
//        }


//        public class OrderViewModel
//        {
//            public string Id { get; set; }
//            public string UserName { get; set; }
//            public DateTime Timestamp { get; set; }
//            public int ItemCount { get; set; }
//            public double Total { get; set; }
//            public string Status { get; set; }
//            public List<OrderItemViewModel> ItemDetails { get; set; } = new();
//        }

//        public class OrderItemViewModel
//        {
//            public string Name { get; set; }
//            public int Quantity { get; set; }
//        }
//    }
//}





