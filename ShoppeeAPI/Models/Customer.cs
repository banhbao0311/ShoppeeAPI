namespace ShoppeeAPI.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Address { get; set; }
        public string? Fullname { get; set; }
        public string? PhoneNumber { get; set; }        
        public int Status { get; set; } = 0;
    }
}
