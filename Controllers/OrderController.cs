using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

public class OrdersController : Controller
{
    private static List<Order> _orders = new List<Order>
    {
        new Order {
            Id = 10045,
            CustomerName = "John Smith",
            Restaurant = "Burger Palace",
            Amount = 24.99m,
            Status = "Delivered",
            OrderDate = DateTime.Now.AddDays(-1)
        },
        new Order {
            Id = 10044,
            CustomerName = "Sarah Johnson",
            Restaurant = "Pizza Heaven",
            Amount = 32.50m,
            Status = "Processing",
            OrderDate = DateTime.Now.AddHours(-3)
        }
    };

    public IActionResult Index()
    {
        // Explicitly pass the model to the view
        return View(_orders);
    }

    public IActionResult Details(int id)
    {
        var order = _orders.FirstOrDefault(o => o.Id == id);
        if (order == null)
        {
            return NotFound();
        }
        return View(order);
    }
}

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public string Restaurant { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
    public DateTime OrderDate { get; set; }
}