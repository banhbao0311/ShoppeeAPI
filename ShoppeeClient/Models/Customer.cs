using System.ComponentModel.DataAnnotations;

namespace ShoppeeAPI.Models
{
    public class Customer
    {
        public int? Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string? Fullname { get; set; }
        [Required]
        public string? PhoneNumber { get; set; }        
        public int Status { get; set; } = 0;
    }
}
