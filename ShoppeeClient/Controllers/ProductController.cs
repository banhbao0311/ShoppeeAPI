using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppeeAPI.Models;
using System.Net;
using System.Security.Claims;

namespace ShoppeeClient.Controllers
{
    public class ProductController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private string uri = "https://localhost:7193/api";
        public IActionResult Index()
        {
            var result = _httpClient.GetStringAsync(uri+"/Product").Result;
            var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(result);
            ViewBag.Fullname = HttpContext.User.Identity.Name;
            return View(products);
        }
        [Authorize]
        public IActionResult CreateCart(int Id)
        {
            var result = _httpClient.GetStringAsync(uri+"/Product/"+Id).Result;
            var product = JsonConvert.DeserializeObject<Product>(result);   
            return View(product);
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateCart(Product productPost)
        {
            productPost.Image = "";
            var userId = HttpContext.User.FindFirstValue("UserId");
            var result = _httpClient.PostAsJsonAsync(uri+ $"/Product/Cart?userId={userId}", productPost).Result;
            if (!result.IsSuccessStatusCode)
            {
                return View();
            }
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpGet]
        public IActionResult ListCarts()
        {
            var customerId = HttpContext.User.FindFirstValue("UserId");
            var result = _httpClient.GetStringAsync(uri + $"/Product/Cart/Customer?customerId={customerId}").Result;
            var carts = JsonConvert.DeserializeObject<IEnumerable<Cart>>(result);
            ViewBag.Address = HttpContext.User.FindFirstValue("Address");
            return View(carts);
        }
        [Authorize]
        [HttpGet]
        public IActionResult EditCart(int Id)
        {
            var result = _httpClient.GetStringAsync(uri + $"/Product/Cart?cartId={Id}").Result;
            var cart = JsonConvert.DeserializeObject<Cart>(result);
            return View(cart);
        }
        [Authorize]
        [HttpPost]
        public IActionResult EditCart(Cart cart)
        {
            var result = _httpClient.PutAsJsonAsync(uri + $"/Product/Cart?idCart={cart.Id}&quanity={cart.Quanity}",cart).Result;
            if(!result.IsSuccessStatusCode)
            {
                return View();
            }
            return RedirectToAction("ListCarts");
        }
        [Authorize]
        [HttpGet]   
        public IActionResult DeleteCart(int Id)
        {
            var result = _httpClient.DeleteAsync(uri + $"/Product/Cart?idCart={Id}").Result;
            return RedirectToAction("ListCarts");
        }
        [Authorize]
        [HttpPost]
        public IActionResult CreateOrder(string Address)
        {
            var customerId = HttpContext.User.FindFirstValue("UserId");
            var result = _httpClient.PostAsJsonAsync(uri+ $"/Product/Order?CustomerId={customerId}&Address={Address}", Address).Result;
            return RedirectToAction("Index");
        }
        [Authorize]
        [HttpGet]
        public IActionResult ListOrdeDetails()
        {
            var customerId = HttpContext.User.FindFirstValue("UserId");
            var result = _httpClient.GetStringAsync(uri + $"/Product/OrderDetails?CustomerId={customerId}").Result;
            var orderDetails =  JsonConvert.DeserializeObject<List<OrderDetails>>(result);
            return View(orderDetails);
        }
    }
}
