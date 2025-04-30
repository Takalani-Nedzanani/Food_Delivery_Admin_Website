//using FoodDeliveryAdminWebsite.Models;
//using Microsoft.AspNetCore.Mvc;
//using System;
//using System.Collections.Generic;
//using System.Linq;

//public class OrdersController : Controller
//{
//    private readonly ApplicationDbContext _context;

//    public OrdersController(ApplicationDbContext context)
//    {
//        _context = context;
//    }

//    [HttpGet]
//    public IActionResult Details(int id)
//    {
//        var order = _context.Orders
//            .Include(o => o.Restaurant)
//            .Include(o => o.OrderItems)
//                .ThenInclude(oi => oi.MenuItem)
//            .FirstOrDefault(o => o.Id == id);

//        if (order == null)
//        {
//            return NotFound();
//        }

//        return View(order);
//    }

//    [HttpGet]
//    public IActionResult Process(int id)
//    {
//        var order = _context.Orders
//            .Include(o => o.Restaurant)
//            .Include(o => o.OrderItems)
//            .FirstOrDefault(o => o.Id == id);

//        if (order == null)
//        {
//            return NotFound();
//        }

//        return View(order);
//    }

//    [HttpPost]
//    public IActionResult Process(Order model)
//    {
//        if (ModelState.IsValid)
//        {
//            var order = _context.Orders.Find(model.Id);
//            if (order == null)
//            {
//                return NotFound();
//            }

//            order.Status = model.Status;
//            order.AssignedDriver = model.AssignedDriver;
//            order.EstimatedDeliveryTime = model.EstimatedDeliveryTime;
//            order.AdminNotes = model.AdminNotes;

//            _context.SaveChanges();

//            return RedirectToAction("Details", new { id = model.Id });
//        }

//        // Reload related data if validation fails
//        model.Restaurant = _context.Restaurants.Find(model.RestaurantId);
//        model.OrderItems = _context.OrderItems
//            .Where(oi => oi.OrderId == model.Id)
//            .Include(oi => oi.MenuItem)
//            .ToList();

//        return View(model);
//    }
//}