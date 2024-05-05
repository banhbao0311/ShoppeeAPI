namespace ShoppeeAPI.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quanity { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public string? Image { get; set; }      
        public int? Status { get; set; } = 0;
    }
}
