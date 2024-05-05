using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppeeAPI.Models;

namespace ShoppeeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly DataContext _context;
        public CustomerController(DataContext dataContext)
        {
            _context = dataContext;
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Customer customerPost)
        {
            var customer = await _context.Customers.Where(c => c.Username == customerPost.Username && c.Password == customerPost.Password).FirstOrDefaultAsync();
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        [HttpPost("Signup")]
        public async Task<IActionResult> Signup([FromBody] Customer customerPost)
        {
            var customer = await _context.Customers.Where(c => c.Username == customerPost.Username).FirstOrDefaultAsync();
            if (customer != null)
            {
                return BadRequest("The username are available");
            }
            _context.Customers.Add(customerPost);
            var result =await _context.SaveChangesAsync();
            return Ok(customerPost);
        }

    }
}
