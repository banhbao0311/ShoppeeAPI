using Microsoft.AspNetCore.Mvc;
using ShoppeeAPI.Models;
using System.Net.Http;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using System.Security.Claims;

namespace ShoppeeClient.Controllers
{
    public class CustomerController : Controller
    {
        private readonly HttpClient _httpClient = new HttpClient();
        private string uri = "https://localhost:7193/api";
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(Customer customerPost)
        {
            customerPost.PhoneNumber = "";
            customerPost.Address = "";
            customerPost.Status = 0;
            customerPost.Fullname = "";
            customerPost.Id = 0;
            var result = _httpClient.PostAsJsonAsync(uri + "/Customer/Login", customerPost).Result;
            if(result.IsSuccessStatusCode)
            {
                var responseContent = result.Content.ReadAsStringAsync().Result;
                var customer = JsonConvert.DeserializeObject<Customer>(responseContent);
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, customer.Fullname),
                    new Claim("UserId",$"{customer.Id}"),
                    new Claim("Address",$"{customer.Address}"),
                };
                var authProperties = new AuthenticationProperties
                {
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30),
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return Redirect("/Product/Index");
            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/Product/Index");
        }

        [HttpGet]
        public IActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Signup(Customer customerPost)
        {
            customerPost.Id = 0;
            var result = _httpClient.PostAsJsonAsync(uri + "/Customer/Signup", customerPost).Result;
            if(!result.IsSuccessStatusCode)
            {
                return RedirectToAction("Signup");
            }
            return Redirect("/Product/Index");
        }
    }
}
