using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryAdminWebsite.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        [Phone]
        public string Phone { get; set; }

        [StringLength(200)]
        public string Address { get; set; }
    }
}