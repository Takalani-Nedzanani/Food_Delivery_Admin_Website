using FoodDeliveryAdminWebsite.Models;
using System.ComponentModel.DataAnnotations;

using System;
using System.Collections.Generic;


namespace FoodDeliveryAdminWebsite.Models
{
    public class MenuItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 1000)]
        public decimal Price { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        public Restaurant Restaurant { get; set; }
    }
}