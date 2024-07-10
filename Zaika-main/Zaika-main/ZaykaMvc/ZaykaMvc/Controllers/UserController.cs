
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ZaykaMvc.Models;

namespace ZaykaMvc.Controllers
{
    public class UserController : Controller
    {
        private readonly ZaykaDbContext _context;

        public UserController(ZaykaDbContext context)
        {
            _context = context;
        }

        // GET: User/Create
        public IActionResult SignUp()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp([Bind("Id,Username,Password,ConfirmPassword,Email,City,Address")] User user)
        {
            if (ModelState.IsValid)
            {
                _context.Add(user);
                int result = await _context.SaveChangesAsync();                
                    ViewBag.message = "User Registeres";
                    return RedirectToAction("Login");
                
            }
            return View(user);
        }

        public IActionResult Login()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string Username,string Password)
        {
            try
            {
                var admin = await _context.Users.FirstOrDefaultAsync(a => a.Username == Username && a.Password == Password);
                if (admin != null)
                {
                    HttpContext.Session.SetString("username", Username);
                    return RedirectToAction("Index","Home");
                }
                else
                {
                    string error = "Invalid Credentials";
                    ViewBag.error = error;
                    return View();
                }
            }
            catch
            {
                return View();
            }
        }

     
        static HashSet<DishViewModel> cartdish = new HashSet<DishViewModel>();
        public IActionResult MyCart(int Id)
        {
            if (Id == null || _context.Dishes == null)
            {
                return NotFound();
            }

            var dish = _context.Dishes.Find(Id);
            if (dish == null)
            {
                return NotFound();
            }
            DishViewModel dishview = new DishViewModel();
            dishview.Id = dish.Id;
            dishview.Name = dish.Name;
            dishview.Price = dish.Price;
            cartdish.Add(dishview);
            return View(cartdish);
        }



  
        [HttpPost]
        public IActionResult SubmitCart(string SerializedMenu)
        {
            if (!string.IsNullOrEmpty(SerializedMenu))
            {
                var menu = JsonConvert.DeserializeObject<List<DishViewModel>>(SerializedMenu);
                decimal? totalbill = 0;
                foreach (var item in menu)
                {
                    Order order = new Order();
                    order.Username = HttpContext.Session.GetString("username");
                    order.DishName = item.Name;
                    order.Price = item.Price;
                    order.Quantity = item.Quantity;
                    totalbill = totalbill + (item.Price * item.Quantity);
                    _context.Orders.Add(order);
                }
                _context.SaveChanges(); // Save all changes once
                ViewBag.totalbill = totalbill;
                cartdish.Clear();
                return View("OrderPlaced");
            }

            return View("MyCart");
        }

        public IActionResult LogOut() {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

    }
}
