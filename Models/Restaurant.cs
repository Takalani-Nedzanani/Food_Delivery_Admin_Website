using System.ComponentModel.DataAnnotations;

public class Restaurant
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [StringLength(200)]
    public string Address { get; set; }

    [Required]
    [Phone]
    public string Phone { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    [Url]
    public string Website { get; set; }

    public string OpeningHoursWeekdays { get; set; }
    public string OpeningHoursWeekends { get; set; }

    [StringLength(500)]
    public string Description { get; set; }

    [Url]
    [Display(Name = "Image URL")]
    public string ImageUrl { get; set; }

    public bool IsActive { get; set; } = true;
}