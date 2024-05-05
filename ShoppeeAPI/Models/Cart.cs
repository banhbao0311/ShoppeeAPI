namespace ShoppeeAPI.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int Quanity { get; set; }
        public int Status { get; set; } = 0;
    }
}
