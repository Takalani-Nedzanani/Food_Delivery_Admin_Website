using FoodDeliveryAdminWebsite.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryAdminWebsite.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();

        [Required]
        [Range(0.01, 10000)]
        public decimal Total { get; set; }

        [Required]
        public string Status { get; set; } // Pending, Preparing, Ready, Delivered, Cancelled

        [Required]
        public DateTime CreatedAt { get; set; }
    }

    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        public Order Order { get; set; }

        [Required]
        public int MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }
    }
}