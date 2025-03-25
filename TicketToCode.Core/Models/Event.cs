using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketToCode.Core.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Event name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Event name must be between 3-100 characters")]
        public string Name { get; set; } = "New event";

        [Required(ErrorMessage = "Description is required")]
        [StringLength(2000, ErrorMessage = "Description cannot exceed 2000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public EventType Type { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Start Time")]
        [FutureDateValidation(ErrorMessage = "Start time must be in the future")]
        public DateTime StartTime { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "End Time")]
        [CustomValidation(typeof(Event), "ValidateEndTime")]
        public DateTime EndTime { get; set; }

        [Required]
        [Range(1, 10000, ErrorMessage = "Max attendees must be between 1-10000")]
        public int MaxAttendees { get; set; }

        [NotMapped]
        public string Duration => $"{Math.Round((EndTime - StartTime).TotalHours, 1)} hours";

        [Url(ErrorMessage = "Invalid URL format")]
        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Required]
        [Range(0, 10000, ErrorMessage = "Price must be between 0-10000")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Location cannot exceed 100 characters")]
        public string Location { get; set; } = string.Empty;

        // Navigation properties
        public virtual ICollection<Booking>? Bookings { get; set; }

        // Validation method for EndTime
        public static ValidationResult? ValidateEndTime(DateTime endTime, ValidationContext context)
        {
            var eventInstance = (Event)context.ObjectInstance;
            return endTime > eventInstance.StartTime
                ? ValidationResult.Success
                : new ValidationResult("End time must be after start time");
        }
    }

    public enum EventType
    {
        Concert = 0,
        Festival,
        Theatre,
        Conference,
        Workshop,
        Sports,
        Exhibition,
        Other
    }

    // Custom validation attribute for future dates
    public class FutureDateValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                return date > DateTime.Now
                    ? ValidationResult.Success
                    : new ValidationResult("Date must be in the future");
            }
            return new ValidationResult("Invalid date");
        }
    }
}
