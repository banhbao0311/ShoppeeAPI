using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppeeAPI.Models;

namespace ShoppeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DataContext _context;
        public ProductController(DataContext dataContext)
        {
            _context = dataContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }
        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var product = await _context.Products.SingleOrDefaultAsync(p => p.Id == productId);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

        [HttpPost("Cart")]
        public async Task<IActionResult> CreateCart(int userId, [FromBody] Product productPost)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == productPost.Id);
            if (product.Quanity < productPost.Quanity)
            {
                return BadRequest("QuanityCart must lower QuanityProduct");
            }
            var cart = new Cart();
            cart.CustomerId = userId;
            cart.ProductId = productPost.Id;
            cart.Quanity = productPost.Quanity;
            _context.Add(cart);
            var result = await _context.SaveChangesAsync();
            return Ok("");
        }
        [HttpGet("Cart/Customer")]
        public async Task<IActionResult> GetCarts(int customerId)
        {
            var carts = await _context.Carts.Where(c => c.CustomerId == customerId).Include(c => c.Product).Include(c=>c.Customer).ToListAsync();
            return Ok(carts);
        }

        [HttpGet("Cart")]
        public async Task<IActionResult> GetCart(int cartId)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == cartId);
            return Ok(cart);
        }

        [HttpPut("Cart")]
        public async Task<IActionResult> PutCart(int idCart, int quanity)
        {
            var cart = _context.Carts.FirstOrDefault(c => c.Id == idCart);
            cart.Quanity = quanity;
            _context.Update(cart);
            await _context.SaveChangesAsync();
            return Ok("");
        }
        [HttpDelete("Cart")]
        public async Task<IActionResult> DeleteCart(int idCart)
        {
            var cart = await _context.Carts.FirstOrDefaultAsync(c => c.Id == idCart);
            if (cart == null)
            {
                return NotFound();
            }
            _context.Carts.Remove(cart);
            await _context.SaveChangesAsync();
            return Ok("");
        }
        [HttpPost("Order")]
        public async Task<IActionResult> CreateOrder(int CustomerId, string Address)
        {
            var order = new Order();
            order.CustomerId = CustomerId;
            order.Address = Address;


            var orderDetailList = new List<OrderDetails>();
            var carts = await _context.Carts.Where(c => c.CustomerId == CustomerId).ToListAsync();
            foreach (var cart in carts)
            {
                var orderDetails = new OrderDetails();
                orderDetails.Order = order;
                orderDetails.ProductId = cart.ProductId;
                orderDetails.Quanity = cart.Quanity;
                orderDetailList.Add(orderDetails);
            }
            _context.AddRange(orderDetailList);
            _context.RemoveRange(carts);
            var result = await _context.SaveChangesAsync();
            return Ok("");
        }

        [HttpGet("OrderDetails")]
        public async Task<IActionResult> GetOrderDetails(int CustomerId)
        {
            var order = await _context.OrderDetails.Where(od => od.Order.CustomerId == CustomerId).Include(od => od.Order).Include(od => od.Product).ToListAsync();
            return Ok(order);
        }
    }
}
